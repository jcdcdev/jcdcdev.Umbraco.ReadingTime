namespace jcdcdev.Umbraco.ReadingTime.Core;

public static class Constants
{
    public const string TableName = "jcdcdevReadingTime";
    public const string PropertyEditorAlias = "jcdcdev.ReadingTime";
    public const string PropertyEditorUiAlias = "jcdcdev.ReadingTime";

    public static class Package
    {
        public const string Name = "jcdcdev.Umbraco.ReadingTime";
    }

    public static class Configuration
    {
        public const string Wpm = "wpm";
        public const string MinUnit = "minUnit";
        public const string MaxUnit = "maxUnit";
        public const string HideVariationWarning = "hideVariationWarning";
    }

    public static class HealthCheck
    {
        public const string Id = "E1F5B4A2-3C6D-4E8F-9A0B-1C2D3E4F5A6B";
        public const string Name = "Reading Time Data";
        public const string Description = "Checks that all content items with Reading Time properties have calculated values.";
        public const string Group = "Content";
    }
}
