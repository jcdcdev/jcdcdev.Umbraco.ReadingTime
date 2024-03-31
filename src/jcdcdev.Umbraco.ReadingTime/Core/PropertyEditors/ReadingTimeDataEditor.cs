using System.Runtime.Serialization;
using Humanizer;
using Humanizer.Localisation;
using jcdcdev.Umbraco.ReadingTime.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;

[DataEditor(Constants.PropertyEditorAlias, EditorType.PropertyValue, "ReadingTime Information", "readonlyvalue")]
public class ReadingTimeDataEditor : DataEditor
{
    private readonly IEditorConfigurationParser _editorConfigurationParser;
    private readonly IIOHelper _ioHelper;
    private readonly ReadingTimeOptions _options;
    private readonly ILocalizedTextService _localizedTextService;
    private ILogger _logger;

    public ReadingTimeDataEditor(
        IDataValueEditorFactory dataValueEditorFactory,
        IOptions<ReadingTimeOptions> options,
        IIOHelper ioHelper,
        IEditorConfigurationParser editorConfigurationParser,
        ILocalizedTextService localizedTextService,
        ILogger<ReadingTimeDataEditor> logger,
        EditorType type = EditorType.PropertyValue) : base(dataValueEditorFactory, type)
    {
        _ioHelper = ioHelper;
        _editorConfigurationParser = editorConfigurationParser;
        _localizedTextService = localizedTextService;
        _logger = logger;
        _options = options.Value;
    }

    protected override IConfigurationEditor CreateConfigurationEditor()
    {
        var config = new ReadingTimeConfigurationEditor(_ioHelper, _editorConfigurationParser, _options);

        foreach (var field in config.Fields.Where(x => x.View == "dropdown"))
        {
            field.Config["prevalues"] = new List<DropDownPreValue>
            {
                new(GetName(TimeUnit.Second), (int)TimeUnit.Second),
                new(GetName(TimeUnit.Minute), (int)TimeUnit.Minute),
                new(GetName(TimeUnit.Hour), (int)TimeUnit.Hour),
                new(GetName(TimeUnit.Day), (int)TimeUnit.Day)
            };
        }

        foreach (var field in config.Fields)
        {
            var descriptionKey = field.Description;
            var nameKey = field.Name;
            if (descriptionKey.IsNullOrWhiteSpace() || nameKey.IsNullOrWhiteSpace())
            {
                continue;
            }

            field.Description = _localizedTextService.Localize(Constants.LocalisationKeys.Area, descriptionKey);
            field.Name = _localizedTextService.Localize(Constants.LocalisationKeys.Area, nameKey);
        }

        return config;
    }

    private string GetName(TimeUnit timeUnit)
    {
        try
        {
            return Resources.GetResource(ResourceKeys.TimeSpanHumanize.GetResourceKey(timeUnit, 2, true)).Remove(0, 4).ToFirstUpper();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting name for time unit {TimeUnit}", timeUnit);
            return timeUnit.Humanize();
        }
    }
}

public class DropDownPreValue
{
    [JsonProperty("value")] public int Value;
    [JsonProperty("label")] public string Label;

    public DropDownPreValue(string label, int value)
    {
        Label = label;
        Value = value;
    }
}
