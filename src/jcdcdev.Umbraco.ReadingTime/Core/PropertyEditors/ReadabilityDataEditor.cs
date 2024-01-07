using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;

[DataEditor(Constants.PropertyEditorAlias, EditorType.PropertyValue, "ReadingTime Information", "readonlyvalue")]
public class ReadingTimeDataEditor : DataEditor
{
    private readonly Models.ReadingTimeOptions _options;
    private readonly IIOHelper _ioHelper;
    private readonly IEditorConfigurationParser _editorConfigurationParser;

    public ReadingTimeDataEditor(
        IDataValueEditorFactory dataValueEditorFactory,
        IOptions<Models.ReadingTimeOptions> options,
        IIOHelper ioHelper,
        IEditorConfigurationParser editorConfigurationParser,
        EditorType type = EditorType.PropertyValue) : base(dataValueEditorFactory, type)
    {
        _ioHelper = ioHelper;
        _editorConfigurationParser = editorConfigurationParser;
        _options = options.Value;
    }

    protected override IConfigurationEditor CreateConfigurationEditor() => new ReadingTimeConfigurationEditor(_ioHelper, _editorConfigurationParser, _options);
}