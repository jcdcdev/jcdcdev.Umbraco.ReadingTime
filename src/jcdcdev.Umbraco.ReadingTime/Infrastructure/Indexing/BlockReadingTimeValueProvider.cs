using jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure.Indexing;

public class BlockReadingTimeValueProvider : ReadingTimeValueProviderBase
{
    private readonly IBlockValuePropertyIndexValueFactory _converter;

    public BlockReadingTimeValueProvider(IBlockValuePropertyIndexValueFactory converter)
    {
        _converter = converter;
    }

    public override bool CanConvert(IPropertyType type) =>
        type.PropertyEditorAlias is Constants.PropertyEditors.Aliases.BlockGrid or Constants.PropertyEditors.Aliases.BlockList;

    public override TimeSpan? GetReadingTime(IProperty property, string? culture, string? segment, IEnumerable<string> availableCultures, ReadingTimeConfiguration config)
    {
        var values = _converter.GetIndexValues(property, culture, segment, true, availableCultures);
        return ProcessIndexValues(values, config.WordsPerMinute);
    }
}
