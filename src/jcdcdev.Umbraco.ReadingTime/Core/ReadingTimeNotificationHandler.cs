using Humanizer.Localisation;
using jcdcdev.Umbraco.Core.Extensions;
using jcdcdev.Umbraco.ReadingTime.Core.Extensions;
using jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace jcdcdev.Umbraco.ReadingTime.Core;

public class ReadingTimeNotificationHandler :
    INotificationAsyncHandler<ContentSavedNotification>,
    INotificationAsyncHandler<SendingContentNotification>,
    INotificationAsyncHandler<ContentDeletingNotification>
{
    private readonly ILocalizedTextService _localizedTextService;
    private readonly IReadingTimeService _readingTimeService;
    private readonly IContentService _contentService;

    private const int ChildrenPageSize = 1000;

    public ReadingTimeNotificationHandler(IReadingTimeService readingTimeService, ILocalizedTextService localizedTextService, IContentService contentService)
    {
        _readingTimeService = readingTimeService;
        _localizedTextService = localizedTextService;
        _contentService = contentService;
    }

    // Both ContentDeletingNotification and ContentEmptyingRecycleBinNotification are called during emptying a global Recycle Bin empty operation
    // Since individual deletes from within the Bin trigger just the ContentDeletingNotification and not the latter, this should cover both use cases
    public async Task HandleAsync(ContentDeletingNotification notification, CancellationToken cancellationToken)
    {
        foreach (var item in notification.DeletedEntities)
        {
            var toDelete = new List<Guid> { item.Key };
            var pageIndex = 0;

            List<Guid> pagedDescendants;

            do
            {
                pagedDescendants = _contentService
                    .GetPagedDescendants(item.Id, pageIndex, ChildrenPageSize, out _)
                    .Select(x => x.Key)
                    .ToList();

                toDelete.AddRange(pagedDescendants);

                pageIndex++;
            } while (pagedDescendants.Count == ChildrenPageSize);

            await _readingTimeService.DeleteAsync(toDelete);
        }
    }

    public async Task HandleAsync(ContentSavedNotification notification, CancellationToken cancellationToken)
    {
        foreach (var item in notification.SavedEntities)
        {
            await _readingTimeService.Process(item);
        }
    }

    public async Task HandleAsync(SendingContentNotification notification, CancellationToken cancellationToken)
    {
        if (!notification.Content.Key.HasValue)
        {
            return;
        }

        var contentType = notification.Content.DocumentType;
        if (contentType == null)
        {
            return;
        }

        var properties = notification.Content.GetProperties(Constants.PropertyEditorAlias).ToList();
        if (!properties.Any())
        {
            return;
        }

        foreach (var property in properties)
        {
            var config = property.ConfigNullable ?? new Dictionary<string, object?>();
            var min = (TimeUnit)(config.TryGetValue(Constants.Configuration.MinUnit, out var mn) && mn is int minTime ? minTime : ReadingTimeConfiguration.DefaultMinTimeUnit);
            var max = (TimeUnit)(config.TryGetValue(Constants.Configuration.MaxUnit, out var mx) && mx is int maxTime ? maxTime : ReadingTimeConfiguration.DefaultMaxTimeUnit);
            var hideAlert = config.TryGetValue(Constants.Configuration.HideVariationWarning, out var ha) && ha is bool hide && hide;

            var model = await _readingTimeService.GetAsync(notification.Content.Key.Value, property.DataTypeKey);
            if (model == null)
            {
                property.Value = _localizedTextService.Localize(Constants.LocalisationKeys.Area, Constants.LocalisationKeys.SaveAndPublishToGenerateReadingTime);
                continue;
            }

            var culture = property.Culture;
            var value = model.Value(culture);
            if (value == null)
            {
                property.Value = _localizedTextService.Localize(Constants.LocalisationKeys.Area, Constants.LocalisationKeys.SaveAndPublishToGenerateReadingTime);
                continue;
            }

            var alert = string.Empty;
            if (property.Variations == ContentVariation.Nothing && contentType.Variations != ContentVariation.Nothing && !hideAlert)
            {
                var text = _localizedTextService.Localize(Constants.LocalisationKeys.Area, Constants.LocalisationKeys.VariationWarning);
                alert =
                    $"""
                     <div class="alert alert-warning">{text}</div>
                     """;
            }

            property.Value =
                $"""
                 <span>{value.ReadingTime.DisplayTime(min, max, culture)}</span>
                 {alert}
                 """;
        }
    }
}
