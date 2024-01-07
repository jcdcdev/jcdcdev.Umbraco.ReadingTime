using jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure.Indexing;

public class LegacyBlockNestedContentReadingTimeValueProvider : ReadingTimeValueProviderBase
{
    private readonly DefaultPropertyIndexValueFactory _converter;

    public LegacyBlockNestedContentReadingTimeValueProvider() : base()
    {
        _converter = new DefaultPropertyIndexValueFactory();
    }

    public override bool CanConvert(IPropertyType type) =>
        type.PropertyEditorAlias is Constants.PropertyEditors.Aliases.BlockGrid or Constants.PropertyEditors.Aliases.BlockList or Constants.PropertyEditors.Aliases.NestedContent;

    public override TimeSpan? GetReadingTime(IProperty property, string? culture, string? segment, IEnumerable<string> availableCultures, ReadingTimeConfiguration config)
    {
        var values = _converter.GetIndexValues(property, culture, segment, true);
        return ProcessIndexValues(values, config.WordsPerMinute);
    }
}