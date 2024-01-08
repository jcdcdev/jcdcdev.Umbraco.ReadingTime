using jcdcdev.Umbraco.ReadingTime.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;

public class ReadingTimePropertyValueConverter : IPropertyValueConverter
{
    private readonly IReadingTimeService _readingTimeService;
    private readonly IVariationContextAccessor _variationContextAccessor;

    public ReadingTimePropertyValueConverter(
        IReadingTimeService readingTimeService,
        IVariationContextAccessor variationContextAccessor)
    {
        _readingTimeService = readingTimeService;
        _variationContextAccessor = variationContextAccessor;
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

        var model = _readingTimeService.GetAsync(key).GetAwaiter().GetResult();
        var culture = _variationContextAccessor.VariationContext?.Culture;
        return model?.Value(culture) ?? model?.Value();
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