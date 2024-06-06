using jcdcdev.Umbraco.ReadingTime.Core;
using jcdcdev.Umbraco.ReadingTime.Core.Extensions;
using jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;
using Umbraco.Cms.Core.Models;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure.Indexing;

public abstract class ReadingTimeValueProviderBase : IReadingTimeValueProvider
{
    public abstract bool CanConvert(IPropertyType type);

    public abstract TimeSpan? GetReadingTime(
        IProperty property,
        string? culture,
        string? segment,
        IEnumerable<string> availableCultures,
        ReadingTimeConfiguration config);

    protected TimeSpan ProcessIndexValues(IEnumerable<KeyValuePair<string, IEnumerable<object?>>> values, int wpm)
    {
        var time = new TimeSpan();
        foreach (var kvp in values)
        {
            if (kvp.Key.StartsWith("__Raw"))
            {
                continue;
            }

            foreach (var value in kvp.Value.OfType<string>())
            {
                time += value.GetReadingTime(wpm);
            }
        }

        return time;
    }
}
