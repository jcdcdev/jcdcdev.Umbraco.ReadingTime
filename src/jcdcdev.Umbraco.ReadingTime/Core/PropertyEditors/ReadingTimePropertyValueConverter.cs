using jcdcdev.Umbraco.ReadingTime.Core.Models;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;

public class ReadingTimePropertyValueConverter(
    IVariationContextAccessor variationContextAccessor,
    ILogger<ReadingTimePropertyValueConverter> logger)
    : PropertyValueConverterBase
{
    public override bool IsConverter(IPublishedPropertyType propertyType) =>
        propertyType.EditorAlias == Constants.PropertyEditorAlias;

    public override Type GetPropertyValueType(IPublishedPropertyType propertyType) =>
        typeof(ReadingTimeValueModel);

    public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) =>
        PropertyCacheLevel.Element;

    public override object? ConvertSourceToIntermediate(
        IPublishedElement owner,
        IPublishedPropertyType propertyType,
        object? source,
        bool preview)
    {
        if (source is int seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        if (source is string str && int.TryParse(str, out var parsed))
        {
            return TimeSpan.FromSeconds(parsed);
        }

        return null;
    }

    public override object? ConvertIntermediateToObject(
        IPublishedElement owner,
        IPublishedPropertyType propertyType,
        PropertyCacheLevel referenceCacheLevel,
        object? inter,
        bool preview)
    {
        if (inter is not TimeSpan readingTime)
        {
            return null;
        }

        var config = propertyType.DataType.ConfigurationAs<ReadingTimeConfiguration>();
        if (config is null)
        {
            logger.LogError("ReadingTime configuration is missing.");
            return null;
        }

        var culture = variationContextAccessor.VariationContext?.Culture;
        return new ReadingTimeValueModel(readingTime, config.Min, config.Max, culture);
    }
}
