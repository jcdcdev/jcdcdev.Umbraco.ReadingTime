using Umbraco.Extensions;

namespace jcdcdev.Umbraco.ReadingTime.Core.Models;

public class ReadingTimeDto
{
    public List<ReadingTimeVariantDto?> Data { get; init; } = new();
    public int Id { get; init; }
    public Guid Key { get; init; }
    public int DataTypeId { get; init; }
    public Guid DataTypeKey { get; set; }
    public DateTime UpdateDate { get; set; }

    public ReadingTimeVariantDto? Value(string? culture = null) => Data.FirstOrDefault(x => x?.Culture.InvariantEquals(culture) ?? false);
}
