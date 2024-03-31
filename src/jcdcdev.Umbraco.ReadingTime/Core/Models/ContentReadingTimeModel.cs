using Umbraco.Extensions;

namespace jcdcdev.Umbraco.ReadingTime.Core.Models;

public class ContentReadingTimeModel
{
    public List<ReadingTimeVariantModel?> Data { get; init; } = new();
    public int Id { get; init; }
    public Guid Key { get; init; }
    public ReadingTimeVariantModel? Value(string? culture = null) => Data.FirstOrDefault(x => x?.Culture.InvariantEquals(culture) ?? false);
}
