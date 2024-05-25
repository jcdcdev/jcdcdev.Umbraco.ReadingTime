using jcdcdev.Umbraco.ReadingTime.Core.Models;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;

public class ReadingTimeConfigurationEditor : ConfigurationEditor<ReadingTimeConfiguration>
{
    private readonly ReadingTimeOptions _options;

    public ReadingTimeConfigurationEditor(IIOHelper ioHelper, ReadingTimeOptions options) : base(ioHelper)
    {
        _options = options;
    }
}
