namespace jcdcdev.Umbraco.ReadingTime.Web.Models;

public class ReadingTimeResponse(string readingTime, DateTime updateDate)
{
    public DateTime UpdateDate { get; } = updateDate;
    public string ReadingTime { get; } = readingTime;
}
