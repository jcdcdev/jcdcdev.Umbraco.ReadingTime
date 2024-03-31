using jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;
using Umbraco.Cms.Core.Models;

namespace jcdcdev.Umbraco.ReadingTime.Core;

public interface IReadingTimeValueProvider
{
    bool CanConvert(IPropertyType type);
    TimeSpan? GetReadingTime(IProperty property, string? culture, string? segment, IEnumerable<string> availableCultures, ReadingTimeConfiguration config);
}
