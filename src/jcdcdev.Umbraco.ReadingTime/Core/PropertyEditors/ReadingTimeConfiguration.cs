using Umbraco.Cms.Core.PropertyEditors;

namespace jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;

public class ReadingTimeConfiguration
{
    [ConfigurationField("wpm", "Words per minute", "number", Description = "The average number of words per minute a person can read (studies suggest 150-250)")]
    public int WordsPerMinute { get; set; }
}