namespace jcdcdev.Umbraco.ReadingTime.Core.Models;

public class ContentReadingTimeModel
{
    public List<ReadingTimeValueModel?> Data { get; init; } = new();
    public int Id { get; init; }
    public Guid Key { get; init; }
    public ReadingTimeValueModel? Value(string? culture = null) => Data.FirstOrDefault(x => x?.Culture == culture);
}