using System.Globalization;
using Humanizer;
using Humanizer.Localisation;
using jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;
using Umbraco.Extensions;

namespace jcdcdev.Umbraco.ReadingTime.Core.Extensions;

public static class StringExtensions
{
    public static TimeSpan GetReadingTime(this string input, int wpm = 200)
    {
        input = input.StripHtml();
        var wordCount = input.CountWords();
        var readingTime = CalculateReadingTime(wordCount, wpm);
        return TimeSpan.FromMinutes(readingTime);
    }

    public static int CountWords(this string input) => input.Split(new[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;

    private static double CalculateReadingTime(int wordCount, int wordsPerMinute) => (double)wordCount / Math.Max(wordsPerMinute, 1);

    public static string DisplayTime(this TimeSpan? time, TimeUnit minUnit, TimeUnit maxUnit, string? culture = null)
    {
        var cc = CultureInfo.CurrentCulture;
        if (!string.IsNullOrWhiteSpace(culture))
        {
            cc = new CultureInfo(culture);
        }

        var output = ReadingTimeConfiguration.GetReadingTime(time, minUnit);
        return output.Humanize(culture: cc, maxUnit: maxUnit, minUnit: minUnit);
    }
}
