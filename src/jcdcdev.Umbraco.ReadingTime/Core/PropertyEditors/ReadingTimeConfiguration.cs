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

    public TimeUnit Min => (TimeUnit)MinUnit;
    public TimeUnit Max => (TimeUnit)MaxUnit;

    public static TimeSpan GetReadingTime(TimeSpan? readingTime, TimeUnit min) => min switch
    {
        TimeUnit.Second => readingTime.GetValueOrDefault(TimeSpan.FromSeconds(1)),
        TimeUnit.Minute => readingTime.GetValueOrDefault(TimeSpan.FromMinutes(1)),
        TimeUnit.Hour => readingTime.GetValueOrDefault(TimeSpan.FromHours(1)),
        TimeUnit.Day => readingTime.GetValueOrDefault(TimeSpan.FromDays(1)),
        _ => readingTime.GetValueOrDefault(TimeSpan.FromMinutes(1))
    };
}
