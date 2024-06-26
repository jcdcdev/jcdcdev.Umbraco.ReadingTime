﻿using Humanizer;
using Humanizer.Localisation;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;

namespace jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;

[DataEditor(Constants.PropertyEditorUIAlias)]
public class ReadingTimeDataEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper, ILogger<ReadingTimeDataEditor> logger)
    : DataEditor(dataValueEditorFactory)
{
    private readonly ILogger _logger = logger;

    protected override IConfigurationEditor CreateConfigurationEditor()
    {
        var config = new ReadingTimeConfigurationEditor(ioHelper);
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

public class DropDownPreValue(string label, int value)
{
    [JsonProperty("label")] public string Label = label;
    [JsonProperty("value")] public int Value = value;
}
