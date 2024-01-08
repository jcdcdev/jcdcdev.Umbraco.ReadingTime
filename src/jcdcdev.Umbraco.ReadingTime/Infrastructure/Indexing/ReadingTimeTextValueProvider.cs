using jcdcdev.Umbraco.ReadingTime.Core.Extensions;
using jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure.Indexing;

internal class ReadingTimeTextValueProvider : ReadingTimeValueProviderBase
{
    private readonly string[] _supported =
    {
        Constants.PropertyEditors.Aliases.MarkdownEditor,
        Constants.PropertyEditors.Aliases.TextArea,
        Constants.PropertyEditors.Aliases.TextBox,
        Constants.PropertyEditors.Aliases.MultipleTextstring,
        Constants.PropertyEditors.Aliases.TinyMce
    };

    public override bool CanConvert(IPropertyType type) => _supported.Contains(type.PropertyEditorAlias);

    public override TimeSpan? GetReadingTime(IProperty property, string? culture, string? segment, IEnumerable<string> availableCultures, ReadingTimeConfiguration config)
    {
        var value = property.GetValue(culture, segment, true);
        if (value is string text)
        {
            return text.GetReadingTime(config.WordsPerMinute);
        }

        return null;
    }
}