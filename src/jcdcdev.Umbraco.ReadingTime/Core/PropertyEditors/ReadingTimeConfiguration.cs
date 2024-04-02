using Humanizer.Localisation;
using Umbraco.Cms.Core.PropertyEditors;

namespace jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;

public class ReadingTimeConfiguration
{
    public const int DefaultMinTimeUnit = (int)TimeUnit.Minute;
    public const int DefaultMaxTimeUnit = (int)TimeUnit.Minute;

    [ConfigurationField(Constants.Configuration.Wpm, Constants.LocalisationKeys.WordsPerMinutesName, "number", Description = Constants.LocalisationKeys.WordsPerMinutesDescription)]
    public int WordsPerMinute { get; set; }

    [ConfigurationField(Constants.Configuration.MinUnit, Constants.LocalisationKeys.MinUnit, "dropdown", Description = Constants.LocalisationKeys.MinUnitDescription)]
    public int MinUnit { get; set; } = DefaultMinTimeUnit;

    [ConfigurationField(Constants.Configuration.MaxUnit, Constants.LocalisationKeys.MaxUnit, "dropdown", Description = Constants.LocalisationKeys.MaxUnitDescription)]
    public int MaxUnit { get; set; } = DefaultMaxTimeUnit;

    [ConfigurationField(Constants.Configuration.HideVariationWarning, Constants.LocalisationKeys.HideVariationWarning, "boolean", Description = Constants.LocalisationKeys.HideVariationWarningDescription)]
    public bool HideVariationWarning { get; set; }

    public TimeUnit Min => (TimeUnit)MinUnit;
    public TimeUnit Max => (TimeUnit)MaxUnit;

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
