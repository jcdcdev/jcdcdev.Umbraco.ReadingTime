using System.Runtime.Serialization;

namespace jcdcdev.Umbraco.ReadingTime.Core.Models;

public class ReadingTimeVariantModel
{
    [DataMember(Name = "culture")] public string? Culture { get; set; }
    [DataMember(Name = "readingTime")] public TimeSpan? ReadingTime { get; set; }
}
