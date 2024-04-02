using jcdcdev.Umbraco.ReadingTime.Core.Models;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;

public class ReadingTimePropertyValueConverter : IPropertyValueConverter
{
    private readonly ILogger _logger;
    private readonly IReadingTimeService _readingTimeService;
    private readonly IVariationContextAccessor _variationContextAccessor;

    public ReadingTimePropertyValueConverter(
        IReadingTimeService readingTimeService,
        IVariationContextAccessor variationContextAccessor,
        ILogger<ReadingTimePropertyValueConverter> logger)
    {
        _readingTimeService = readingTimeService;
        _variationContextAccessor = variationContextAccessor;
        _logger = logger;
    }

    public object? ConvertIntermediateToObject(
        IPublishedElement owner,
        IPublishedPropertyType propertyType,
        PropertyCacheLevel referenceCacheLevel,
        object? inter,
        bool preview)
    {
        if (inter is not Guid key)
        {
            return null;
        }

        var model = _readingTimeService.GetAsync(key, propertyType.DataType.Id).GetAwaiter().GetResult();
        var culture = _variationContextAccessor.VariationContext?.Culture;
        var config = propertyType.DataType.ConfigurationAs<ReadingTimeConfiguration>();
        if (config is null)
        {
            _logger.LogError("ReadingTime configuration is missing.");
            return null;
        }

        var output = model?.Value(culture) ?? model?.Value();

        if (output is null)
        {
            return null;
        }

        return new ReadingTimeValueModel(
            output.ReadingTime,
            config.Min,
            config.Max,
            output.Culture);
    }

    public object? ConvertIntermediateToXPath(
        IPublishedElement owner,
        IPublishedPropertyType propertyType,
        PropertyCacheLevel referenceCacheLevel,
        object? inter,
        bool preview) => inter;

    public object? ConvertSourceToIntermediate(
        IPublishedElement owner,
        IPublishedPropertyType propertyType,
        object? source,
        bool preview) => owner.Key;

    public PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) =>
        PropertyCacheLevel.Snapshot;

    public Type GetPropertyValueType(IPublishedPropertyType propertyType) => typeof(ReadingTimeValueModel);

    public bool IsConverter(IPublishedPropertyType propertyType) =>
        propertyType.EditorAlias == Constants.PropertyEditorAlias;

    public bool? IsValue(object? value, PropertyValueLevel level) => level switch
    {
        PropertyValueLevel.Source => value is Guid,
        PropertyValueLevel.Inter => value is Guid,
        PropertyValueLevel.Object => value is ReadingTimeValueModel,
        _ => throw new NotSupportedException($"Invalid level: {level}.")
    };
}
