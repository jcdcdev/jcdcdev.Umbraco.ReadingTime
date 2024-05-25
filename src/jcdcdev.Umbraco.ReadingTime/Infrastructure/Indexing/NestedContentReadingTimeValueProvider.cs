using jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure.Indexing;

public class NestedContentReadingTimeValueProvider : ReadingTimeValueProviderBase
{
    private readonly INestedContentPropertyIndexValueFactory _converter;

    public NestedContentReadingTimeValueProvider(INestedContentPropertyIndexValueFactory converter)
    {
        _converter = converter;
    }

    public override bool CanConvert(IPropertyType type) => type.PropertyEditorAlias == Constants.PropertyEditors.Aliases.NestedContent;

    public override TimeSpan? GetReadingTime(IProperty property, string? culture, string? segment, IEnumerable<string> availableCultures, ReadingTimeConfiguration config)
    {
        var values = _converter.GetIndexValues(property, culture, segment, true, availableCultures);
        return ProcessIndexValues(values, config.WordsPerMinute);
    }
}
