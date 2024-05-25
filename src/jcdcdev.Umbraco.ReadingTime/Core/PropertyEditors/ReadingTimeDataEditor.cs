using Humanizer;
using Humanizer.Localisation;
using jcdcdev.Umbraco.ReadingTime.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;

namespace jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;

[DataEditor(Constants.PropertyEditorAlias)]
public class ReadingTimeDataEditor : DataEditor
{
    private readonly IIOHelper _ioHelper;
    private readonly ReadingTimeOptions _options;
    private readonly ILogger _logger;

    public ReadingTimeDataEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper, IOptions<ReadingTimeOptions> options, ILogger<ReadingTimeDataEditor> logger) : base(
        dataValueEditorFactory)
    {
        _ioHelper = ioHelper;
        _options = options.Value;
        _logger = logger;
    }

    protected override IConfigurationEditor CreateConfigurationEditor()
    {
        var config = new ReadingTimeConfigurationEditor(_ioHelper, _options);

        foreach (var field in config.Fields)
        {
            field.Config["prevalues"] = new List<DropDownPreValue>
            {
                new(GetName(TimeUnit.Second), (int)TimeUnit.Second),
                new(GetName(TimeUnit.Minute), (int)TimeUnit.Minute),
                new(GetName(TimeUnit.Hour), (int)TimeUnit.Hour),
                new(GetName(TimeUnit.Day), (int)TimeUnit.Day)
            };
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
    [JsonProperty("label")] public string Label;
    [JsonProperty("value")] public int Value;

    public DropDownPreValue(string label, int value)
    {
        Label = label;
        Value = value;
    }
}
