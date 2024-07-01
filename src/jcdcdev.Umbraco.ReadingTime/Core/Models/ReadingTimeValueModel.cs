using Humanizer.Localisation;
using jcdcdev.Umbraco.ReadingTime.Core.Extensions;

namespace jcdcdev.Umbraco.ReadingTime.Core.Models;

public class ReadingTimeValueModel(TimeSpan? readingTime, TimeUnit minUnit, TimeUnit maxUnit, string? culture)
{
    public string? Culture { get; } = culture;
    public TimeSpan? ReadingTime { get; } = readingTime;
    public TimeUnit MinUnit { get; } = minUnit;
    public TimeUnit MaxUnit { get; } = maxUnit;

    public string DisplayTime(TimeUnit? minUnit = null, TimeUnit? maxUnit = null)
    {
        var min = minUnit ?? MinUnit;
        var max = maxUnit ?? MaxUnit;
        return ReadingTime.DisplayTime(min, max, Culture);
    }
}
