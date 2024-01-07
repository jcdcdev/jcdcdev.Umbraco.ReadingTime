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
}