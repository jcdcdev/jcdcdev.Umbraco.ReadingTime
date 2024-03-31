namespace jcdcdev.Umbraco.ReadingTime.Core;

public static class Constants
{
    public const string TableName = "jcdcdevReadingTime";
    public const string PropertyEditorAlias = "jcdcdev.ReadingTime";

    public static class Package
    {
        public const string Name = "jcdcdev.Umbraco.ReadingTime";
    }

    public static class LocalisationKeys
    {
        public const string SaveAndPublishToGenerateReadingTime = "saveAndPublishToGenerateReadingTime";
        public const string Area = "readingTime";
        public const string WordsPerMinutesDescription = "wordsPerMinutesDescription";
        public const string WordsPerMinutesName = "wordsPerMinutes";
        public const string MinUnit = "minUnit";
        public const string MaxUnit = "maxUnit";
        public const string? MinUnitDescription = "minUnitDescription";
        public const string? MaxUnitDescription = "maxUnitDescription";
    }

    public static class Configuration
    {
        public const string Wpm = "wpm";
        public const string MinUnit = "minUnit";
        public const string MaxUnit = "maxUnit";
    }
}
