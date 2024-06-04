using System.Text.Json.Serialization;
using Humanizer.Localisation;
using Umbraco.Cms.Core.PropertyEditors;

namespace jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;

public class ReadingTimeConfiguration
{
    [ConfigurationField(Constants.Configuration.Wpm)]
    [JsonPropertyName(Constants.Configuration.Wpm)]
    public int WordsPerMinute { get; set; }

    [ConfigurationField(Constants.Configuration.MinUnit)]
    [JsonPropertyName(Constants.Configuration.MinUnit)]
    public string[] MinUnit { get; set; } = Array.Empty<string>();

    [ConfigurationField(Constants.Configuration.MaxUnit)]
    [JsonPropertyName(Constants.Configuration.MaxUnit)]
    public string[] MaxUnit { get; set; } = Array.Empty<string>();

    [ConfigurationField(Constants.Configuration.HideVariationWarning)]
    [JsonPropertyName(Constants.Configuration.HideVariationWarning)]
    public bool HideVariationWarning { get; set; }

    public TimeUnit Min => DetermineUnit(MinUnit.FirstOrDefault());

    public TimeUnit Max => DetermineUnit(MaxUnit.FirstOrDefault());

    private static TimeUnit DetermineUnit(string? minUnit, TimeUnit fallBack = TimeUnit.Minute)
    {
        return minUnit?.ToLowerInvariant() switch
        {
            "millisecond" => TimeUnit.Millisecond,
            "second" => TimeUnit.Second,
            "minute" => TimeUnit.Minute,
            "hour" => TimeUnit.Hour,
            "day" => TimeUnit.Day,
            "week" => TimeUnit.Week,
            "month" => TimeUnit.Month,
            "year" => TimeUnit.Year,
            _ => fallBack
        };
    }

    public static TimeSpan GetReadingTime(TimeSpan? readingTime, TimeUnit min)
    {
        var time = readingTime.GetValueOrDefault();
        return min switch
        {
            TimeUnit.Second => time < TimeSpan.FromSeconds(1) ? TimeSpan.FromSeconds(1) : time,
            TimeUnit.Minute => time < TimeSpan.FromMinutes(1) ? TimeSpan.FromMinutes(1) : time,
            TimeUnit.Hour => time < TimeSpan.FromHours(1) ? TimeSpan.FromHours(1) : time,
            TimeUnit.Day => time < TimeSpan.FromDays(1) ? TimeSpan.FromDays(1) : time,
            _ => time < TimeSpan.FromMinutes(1) ? TimeSpan.FromMinutes(1) : time
        };
    }
}
