using jcdcdev.Umbraco.ReadingTime.Core.Models;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;

public class ReadingTimePropertyValueConverter(
    IReadingTimeService readingTimeService,
    IVariationContextAccessor variationContextAccessor,
    ILogger<ReadingTimePropertyValueConverter> logger)
    : PropertyValueConverterBase
{
    private readonly ILogger _logger = logger;

    public override object? ConvertIntermediateToObject(
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

        var model = readingTimeService.GetAsync(key, propertyType.DataType.Id).GetAwaiter().GetResult();
        var culture = variationContextAccessor.VariationContext?.Culture;
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

        return new ReadingTimeValueModel(output.ReadingTime, config.Min, config.Max, output.Culture);
    }

    public override object? ConvertSourceToIntermediate(
        IPublishedElement owner,
        IPublishedPropertyType propertyType,
        object? source,
        bool preview) => owner.Key;

    public override Type GetPropertyValueType(IPublishedPropertyType propertyType) => typeof(ReadingTimeValueModel);

    public override bool IsConverter(IPublishedPropertyType propertyType) => propertyType.EditorAlias == Constants.PropertyEditorAlias;
}
