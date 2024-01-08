namespace jcdcdev.Umbraco.ReadingTime.Core.Models;

public class ReadingTimeValueModel
{
    public string? Culture { get; set; }
    public TimeSpan? ReadingTime { get; set; }

    public string DisplayTime()
    {
        var minutes = Math.Ceiling(ReadingTime.GetValueOrDefault().TotalMinutes);
        if (minutes < 1)
        {
            minutes = 1;
        }

        return $"{minutes} {(minutes > 1 ? "minutes" : "minute")}";
    }
}