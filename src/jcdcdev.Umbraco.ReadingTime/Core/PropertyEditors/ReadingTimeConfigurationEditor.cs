using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;

public class ReadingTimeConfigurationEditor(IIOHelper ioHelper) : ConfigurationEditor<ReadingTimeConfiguration>(ioHelper);
