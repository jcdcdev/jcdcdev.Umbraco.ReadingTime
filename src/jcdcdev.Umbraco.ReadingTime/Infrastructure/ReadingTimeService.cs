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

public class ReadingTimeService : IReadingTimeService
{
    private readonly IContentService _contentService;
    private readonly ReadingTimeValueProviderCollection _convertors;
    private readonly IDataTypeService _dataTypeService;
    private readonly IReadingTimeRepository _readingTimeRepository;
    private readonly ILogger _logger;

    public ReadingTimeService(
        IContentService contentService,
        ReadingTimeValueProviderCollection convertors,
        IReadingTimeRepository readingTimeRepository,
        IDataTypeService dataTypeService,
        ILogger<ReadingTimeService> logger)
    {
        _contentService = contentService;
        _convertors = convertors;
        _readingTimeRepository = readingTimeRepository;
        _dataTypeService = dataTypeService;
        _logger = logger;
    }

    public async Task<ReadingTimeDto?> GetAsync(Guid key, Guid dataTypeKey) => await _readingTimeRepository.Get(key, dataTypeKey);

    public async Task<ReadingTimeDto?> GetAsync(Guid key, int dataTypeId) => await _readingTimeRepository.Get(key, dataTypeId);

    public async Task<int> DeleteAsync(Guid key)
    {
        _logger.LogDebug("Deleting reading time for {Key}", key);
        return await _readingTimeRepository.DeleteAsync(key);
    }

    public async Task ScanTree(int homeId)
    {
        var content = _contentService.GetById(homeId);
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
                var children = _contentService
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
        var root = _contentService.GetRootContent().ToList();
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
        var dataType = _dataTypeService.GetDataType(readingTimeProperty.PropertyType.DataTypeId);
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

        var dto = await _readingTimeRepository.GetOrCreate(item.Key, dataType);
        var models = new List<ReadingTimeVariantDto>();
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

        dto.Data.Clear();
        dto.Data.AddRange(models);

        await _readingTimeRepository.PersistAsync(dto);
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
            var convertor = _convertors.FirstOrDefault(x => x.CanConvert(property.PropertyType));
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
