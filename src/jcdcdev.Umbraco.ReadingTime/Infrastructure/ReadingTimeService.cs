using jcdcdev.Umbraco.ReadingTime.Core;
using jcdcdev.Umbraco.ReadingTime.Core.Composing;
using jcdcdev.Umbraco.ReadingTime.Core.Models;
using jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;
using jcdcdev.Umbraco.ReadingTime.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure;

public class ReadingTimeService(
    IContentService contentService,
    ReadingTimeValueProviderCollection convertors,
    IReadingTimeRepository readingTimeRepository,
    IDataTypeService dataTypeService,
    ILogger<ReadingTimeService> logger)
    : IReadingTimeService
{
    private readonly ILogger _logger = logger;

    public async Task<ReadingTimeDto?> GetAsync(Guid key, Guid dataTypeKey) => await readingTimeRepository.Get(key, dataTypeKey);

    public async Task<ReadingTimeDto?> GetAsync(Guid key, int dataTypeId) => await readingTimeRepository.Get(key, dataTypeId);

    public async Task<int> DeleteAsync(Guid key)
    {
        _logger.LogDebug("Deleting reading time for {Key}", key);
        return await readingTimeRepository.DeleteAsync(key);
    }

    public async Task ScanTree(int homeId)
    {
        var content = contentService.GetById(homeId);
        if (content == null)
        {
            _logger.LogWarning("Content with id {HomeId} not found", homeId);
            return;
        }

        var queue = new Queue<IContent>();
        queue.Enqueue(content);

        while (queue.TryDequeue(out var current))
        {
            var moreRecords = true;
            var page = 0;
            while (moreRecords)
            {
                var children = contentService
                    .GetPagedChildren(current.Id, page, 100, out var totalRecords)
                    .ToList();

                foreach (var child in children)
                {
                    queue.Enqueue(child);
                }

                page++;
                moreRecords = (page + 1) * 100 <= totalRecords;
            }

            if (current.Published)
            {
                await Process(current);
            }
        }
    }

    public async Task ScanAll()
    {
        var root = contentService.GetRootContent().ToList();
        _logger.LogInformation("Scanning {Count} root content items", root.Count);
        foreach (var content in root)
        {
            await ScanTree(content.Id);
        }
    }

    public async Task Process(IContent item)
    {
        var props = item.Properties.Where(x => x.PropertyType.PropertyEditorAlias == Constants.PropertyEditorAlias).ToList();
        if (!props.Any())
        {
            return;
        }

        _logger.LogDebug("Processing {Id}:{Item}", item.Id, item.Name);
        foreach (var property in props)
        {
            await ProcessPropertyEditor(item, property);
        }
    }

    private async Task ProcessPropertyEditor(IContent item, IProperty readingTimeProperty)
    {
        var dataType = await dataTypeService.GetAsync(readingTimeProperty.PropertyType.DataTypeKey);
        if (dataType == null)
        {
            _logger.LogWarning("DataType not found for property {PropertyId}", readingTimeProperty.Id);
            return;
        }

        var config = dataType.ConfigurationAs<ReadingTimeConfiguration>();
        if (config == null)
        {
            _logger.LogWarning("Configuration not found for property {PropertyId}", readingTimeProperty.Id);
            return;
        }

        var dto = await readingTimeRepository.GetOrCreate(item.Key, dataType);
        dto.UpdateDate = DateTime.UtcNow;
        var models = new List<ReadingTimeVariantDto?>();
        var propertyType = readingTimeProperty.PropertyType;
        if (propertyType.VariesByCulture())
        {
            _logger.LogDebug("Processing culture variants for {Id}:{Item}", item.Id, item.Name);
            foreach (var culture in item.AvailableCultures)
            {
                _logger.LogDebug("Processing culture {Culture}", culture);
                var model = GetModel(item, culture, null, config);
                models.Add(model);
            }
        }

        _logger.LogDebug("Processing invariant variant for {Id}:{Item}", item.Id, item.Name);
        var invariant = GetModel(item, null, null, config);
        models.Add(invariant);

        var merge = dto.Data.Where(x => !models.Select(y => y.Culture).Contains(x?.Culture)).ToList();
        if (merge.Any())
        {
            models.AddRange(merge);
            _logger.LogDebug("Merging {Count} existing models", merge.Count());
        }

        dto.Data.Clear();
        dto.Data.AddRange(models);

        await readingTimeRepository.PersistAsync(dto);
    }

    private ReadingTimeVariantDto GetModel(IContent item, string? culture, string? segment, ReadingTimeConfiguration config)
    {
        var readingTime = GetReadingTime(item, culture, segment, config);
        var model = new ReadingTimeVariantDto
        {
            Culture = culture,
            ReadingTime = readingTime
        };

        return model;
    }

    private TimeSpan? GetReadingTime(IContent item, string? culture, string? segment, ReadingTimeConfiguration config)
    {
        var time = TimeSpan.Zero;
        foreach (var property in item.Properties)
        {
            var convertor = convertors.FirstOrDefault(x => x.CanConvert(property.PropertyType));
            if (convertor == null)
            {
                _logger.LogDebug("No convertor found for {PropertyId}:{PropertyEditorAlias}", property.Id, property.PropertyType.PropertyEditorAlias);
                continue;
            }

            _logger.LogDebug("Processing property {PropertyId}:{PropertyEditorAlias}", property.Id, property.PropertyType.PropertyEditorAlias);

            var cCulture = property.PropertyType.VariesByCulture() ? culture : null;
            var cSegment = property.PropertyType.VariesBySegment() ? segment : null;
            var readingTime = convertor?.GetReadingTime(property, cCulture, cSegment, item.AvailableCultures, config);
            if (!readingTime.HasValue)
            {
                _logger.LogDebug("No reading time found for {PropertyId}:{PropertyEditorAlias}", property.Id, property.PropertyType.PropertyEditorAlias);
                continue;
            }

            _logger.LogDebug("Reading time found for {PropertyId}:{PropertyEditorAlias} ({Time})", property.Id, property.PropertyType.PropertyEditorAlias, readingTime.Value);
            time += readingTime.Value;
        }

        return time;
    }
}
