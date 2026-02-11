using jcdcdev.Umbraco.ReadingTime.Core;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.HealthChecks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure.HealthChecks;

[HealthCheck(
    Constants.HealthCheck.Id,
    Constants.HealthCheck.Name,
    Description = Constants.HealthCheck.Description,
    Group = Constants.HealthCheck.Group)]
public class ReadingTimeHealthCheck : HealthCheck
{
    private const int BatchSize = 300;
    private const int PageSize = 100;

    private readonly IContentService _contentService;
    private readonly IReadingTimeService _readingTimeService;
    private readonly ILogger<ReadingTimeHealthCheck> _logger;

    public ReadingTimeHealthCheck(
        IContentService contentService,
        IReadingTimeService readingTimeService,
        ILogger<ReadingTimeHealthCheck> logger)
    {
        _contentService = contentService;
        _readingTimeService = readingTimeService;
        _logger = logger;
    }

    public override Task<IEnumerable<HealthCheckStatus>> GetStatusAsync()
    {
        var (total, missing) = CountContentWithMissingReadingTime();

        if (total == 0)
        {
            var noContent = new HealthCheckStatus("No content types use a Reading Time property.")
            {
                ResultType = StatusResultType.Info
            };
            return Task.FromResult<IEnumerable<HealthCheckStatus>>([noContent]);
        }

        if (missing == 0)
        {
            var allGood = new HealthCheckStatus($"All {total} content items with Reading Time properties have calculated values.")
            {
                ResultType = StatusResultType.Success
            };
            return Task.FromResult<IEnumerable<HealthCheckStatus>>([allGood]);
        }

        var status = new HealthCheckStatus($"{missing} of {total} content items with Reading Time properties have missing values.")
        {
            ResultType = StatusResultType.Warning,
            Actions = new List<HealthCheckAction>
            {
                new("recalculate-batch", Id)
                {
                    Name = "Recalculate next batch",
                    Description = $"Recalculates reading time for the next {BatchSize} content items with missing values."
                }
            }
        };

        return Task.FromResult<IEnumerable<HealthCheckStatus>>([status]);
    }

    public override async Task<HealthCheckStatus> ExecuteActionAsync(HealthCheckAction action)
    {
        if (action.Alias != "recalculate-batch")
        {
            return new HealthCheckStatus("Unknown action.")
            {
                ResultType = StatusResultType.Error
            };
        }

        try
        {
            var (processed, remaining) = await RecalculateBatch();

            if (remaining > 0)
            {
                return new HealthCheckStatus($"Recalculated {processed} items. {remaining} remaining.")
                {
                    ResultType = StatusResultType.Warning,
                    Actions = new List<HealthCheckAction>
                    {
                        new("recalculate-batch", Id)
                        {
                            Name = "Recalculate next batch",
                            Description = $"Recalculates reading time for the next {BatchSize} content items with missing values."
                        }
                    }
                };
            }

            return new HealthCheckStatus($"Recalculated {processed} items. All items now have values.")
            {
                ResultType = StatusResultType.Success
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recalculating reading times");
            return new HealthCheckStatus($"Error recalculating: {ex.Message}")
            {
                ResultType = StatusResultType.Error
            };
        }
    }

    private async Task<(int processed, int remaining)> RecalculateBatch()
    {
        var processed = 0;
        var remaining = 0;
        var batchComplete = false;
        var rootContent = _contentService.GetRootContent().ToList();

        foreach (var root in rootContent)
        {
            if (batchComplete)
            {
                remaining += CountMissingInTree(root);
                continue;
            }

            (processed, remaining, batchComplete) = await ProcessTree(root, processed);
        }

        return (processed, remaining);
    }

    private async Task<(int processed, int remaining, bool batchComplete)> ProcessTree(IContent content, int processed)
    {
        var remaining = 0;
        var batchComplete = false;

        if (!batchComplete)
        {
            var hasReadingTimeProperty = content.Properties
                .Any(x => x.PropertyType.PropertyEditorAlias == Constants.PropertyEditorAlias);

            if (hasReadingTimeProperty && IsMissingValue(content))
            {
                if (processed >= BatchSize)
                {
                    batchComplete = true;
                    remaining++;
                }
                else
                {
                    var wasPublished = content.Published;
                    await _readingTimeService.CalculateAndSetReadingTime(content);

                    _contentService.Save(content);
                    if (wasPublished)
                    {
                        _contentService.Publish(content, content.AvailableCultures.ToArray());
                    }

                    processed++;
                    if (_logger.IsEnabled(LogLevel.Debug))
                    {
                        _logger.LogDebug("Recalculated reading time for {ContentName} (Id: {ContentId})", content.Name, content.Id);
                    }
                }
            }
        }

        var page = 0;
        var moreRecords = true;
        while (moreRecords)
        {
            var children = _contentService
                .GetPagedChildren(content.Id, page, PageSize, out var totalRecords)
                .ToList();

            foreach (var child in children)
            {
                if (batchComplete)
                {
                    remaining += CountMissingInTree(child);
                }
                else
                {
                    var result = await ProcessTree(child, processed);
                    processed = result.processed;
                    remaining += result.remaining;
                    batchComplete = result.batchComplete;
                }
            }

            page++;
            moreRecords = (page + 1) * PageSize <= totalRecords;
        }

        return (processed, remaining, batchComplete);
    }

    private int CountMissingInTree(IContent content)
    {
        var missing = 0;

        var hasReadingTimeProperty = content.Properties
            .Any(x => x.PropertyType.PropertyEditorAlias == Constants.PropertyEditorAlias);

        if (hasReadingTimeProperty && IsMissingValue(content))
        {
            missing++;
        }

        var page = 0;
        var moreRecords = true;
        while (moreRecords)
        {
            var children = _contentService
                .GetPagedChildren(content.Id, page, PageSize, out var totalRecords)
                .ToList();

            foreach (var child in children)
            {
                missing += CountMissingInTree(child);
            }

            page++;
            moreRecords = (page + 1) * PageSize <= totalRecords;
        }

        return missing;
    }

    private (int total, int missing) CountContentWithMissingReadingTime()
    {
        var total = 0;
        var missing = 0;

        var rootContent = _contentService.GetRootContent().ToList();
        foreach (var root in rootContent)
        {
            CountInTree(root, ref total, ref missing);
        }

        return (total, missing);
    }

    private void CountInTree(IContent content, ref int total, ref int missing)
    {
        var readingTimeProperties = content.Properties
            .Where(x => x.PropertyType.PropertyEditorAlias == Constants.PropertyEditorAlias)
            .ToList();

        if (readingTimeProperties.Count > 0)
        {
            total++;
            if (IsMissingValue(content))
            {
                missing++;
            }
        }

        var page = 0;
        var moreRecords = true;
        while (moreRecords)
        {
            var children = _contentService
                .GetPagedChildren(content.Id, page, PageSize, out var totalRecords)
                .ToList();

            foreach (var child in children)
            {
                CountInTree(child, ref total, ref missing);
            }

            page++;
            moreRecords = (page + 1) * PageSize <= totalRecords;
        }
    }

    private static bool IsMissingValue(IContent content)
    {
        var readingTimeProperties = content.Properties
            .Where(x => x.PropertyType.PropertyEditorAlias == Constants.PropertyEditorAlias)
            .ToList();

        return !readingTimeProperties.Any(p =>
        {
            if (p.PropertyType.VariesByCulture())
            {
                return content.AvailableCultures.Any(c => p.GetValue(c) != null);
            }

            return p.GetValue() != null;
        });
    }
}
