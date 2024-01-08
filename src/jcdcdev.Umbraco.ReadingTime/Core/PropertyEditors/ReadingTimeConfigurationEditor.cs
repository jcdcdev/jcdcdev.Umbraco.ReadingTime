using jcdcdev.Umbraco.ReadingTime.Core.Models;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;

public class ReadingTimeConfigurationEditor : ConfigurationEditor<ReadingTimeConfiguration>
{
    private readonly ReadingTimeOptions _options;

    [Obsolete("Obsolete")]
    public ReadingTimeConfigurationEditor(IIOHelper ioHelper, ReadingTimeOptions options) : base(ioHelper)
    {
        _options = options;
    }

    public ReadingTimeConfigurationEditor(IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser, ReadingTimeOptions options) : base(ioHelper, editorConfigurationParser)
    {
        _options = options;
    }

    public override object DefaultConfigurationObject => new ReadingTimeConfiguration
    {
        WordsPerMinute = _options.WordsPerMinute
    };
}