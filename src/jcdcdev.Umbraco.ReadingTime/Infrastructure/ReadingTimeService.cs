using jcdcdev.Umbraco.ReadingTime.Core;
using jcdcdev.Umbraco.ReadingTime.Core.Composing;
using jcdcdev.Umbraco.ReadingTime.Core.Models;
using jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;
using jcdcdev.Umbraco.ReadingTime.Infrastructure.Persistence;
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

    public ReadingTimeService(
        IContentService contentService,
        ReadingTimeValueProviderCollection convertors,
        IReadingTimeRepository readingTimeRepository,
        IDataTypeService dataTypeService)
    {
        _contentService = contentService;
        _convertors = convertors;
        _readingTimeRepository = readingTimeRepository;
        _dataTypeService = dataTypeService;
    }

    public async Task<ContentReadingTimeModel?> GetAsync(Guid key) => await _readingTimeRepository.GetByKey(key);
    public async Task<int> DeleteAsync(Guid key) => await _readingTimeRepository.DeleteAsync(key);

    public async Task ScanTree(int homeId)
    {
        var content = _contentService.GetById(homeId);
        if (content == null)
        {
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

            await Process(current);
        }
    }

    public async Task Process(IContent item)
    {
        if (item.Properties.All(x => x.PropertyType.PropertyEditorAlias != Constants.PropertyEditorAlias))
        {
            return;
        }

        var dto = await _readingTimeRepository.GetOrCreate(item.Key);
        var models = new List<ReadingTimeValueModel>();

        var readingTimeProperty = item.Properties.FirstOrDefault(x => x.PropertyType.PropertyEditorAlias == Constants.PropertyEditorAlias);
        if (readingTimeProperty == null)
        {
            return;
        }

        var dataType = _dataTypeService.GetDataType(readingTimeProperty.PropertyType.DataTypeId);
        if (dataType == null)
        {
            return;
        }

        var config = dataType.ConfigurationAs<ReadingTimeConfiguration>();
        if (config == null)
        {
            return;
        }

        foreach (var culture in item.AvailableCultures)
        {
            var model = GetModel(item, culture, null, config);
            models.Add(model);
        }

        var invariant = GetModel(item, null, null, config);
        models.Add(invariant);

        dto.Data.Clear();
        dto.Data.AddRange(models);

        await _readingTimeRepository.PersistAsync(dto);
    }

    private ReadingTimeValueModel GetModel(IContent item, string? culture, string? segment, ReadingTimeConfiguration config)
    {
        var readingTime = GetReadingTime(item, culture, segment, config);
        var model = new ReadingTimeValueModel
        {
            ReadingTime = readingTime,
            Culture = culture
        };
        return model;
    }

    private TimeSpan? GetReadingTime(IContent item, string? culture, string? segment, ReadingTimeConfiguration config)
    {
        var time = TimeSpan.Zero;
        foreach (var property in item.Properties)
        {
            var convertor = _convertors.FirstOrDefault(x => x.CanConvert(property.PropertyType));
            var readingTime = convertor?.GetReadingTime(property, culture, segment, item.AvailableCultures, config);
            if (!readingTime.HasValue)
            {
                continue;
            }

            time += readingTime.Value;
        }

        return time;
    }
}