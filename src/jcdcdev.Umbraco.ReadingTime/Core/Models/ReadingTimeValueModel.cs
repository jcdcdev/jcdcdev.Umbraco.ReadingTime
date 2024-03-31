using System.Globalization;
using Humanizer;
using Humanizer.Localisation;
using jcdcdev.Umbraco.ReadingTime.Core.Extensions;

namespace jcdcdev.Umbraco.ReadingTime.Core.Models;

public class ReadingTimeValueModel
{
    public ReadingTimeValueModel(TimeSpan? readingTime, TimeUnit minUnit, TimeUnit maxUnit, string? culture)
    {
        MinUnit = minUnit;
        MaxUnit = maxUnit;
        ReadingTime = readingTime;
        Culture = culture;
    }

    public string? Culture { get; }
    public TimeSpan? ReadingTime { get; }
    public TimeUnit MinUnit { get; }
    public TimeUnit MaxUnit { get; }

    public string DisplayTime(TimeUnit? minUnit = null, TimeUnit? maxUnit = null)
    {
        var min = minUnit ?? MinUnit;
        var max = maxUnit ?? MaxUnit;
        return ReadingTime.DisplayTime(minUnit: min, maxUnit: max, Culture);
    }
}
