using jcdcdev.Umbraco.ReadingTime.Core;
using jcdcdev.Umbraco.ReadingTime.Core.Extensions;
using jcdcdev.Umbraco.ReadingTime.Core.PropertyEditors;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;

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

    protected TimeSpan ProcessIndexValues(IEnumerable<IndexValue> indexValues, int wpm)
    {
        var time = new TimeSpan();
        foreach (var indexValue in indexValues)
        {
            if (indexValue.FieldName.StartsWith("__Raw"))
            {
                continue;
            }

            foreach (var value in indexValue.Values.OfType<string>())
            {
                time += value.GetReadingTime(wpm);
            }
        }

        return time;
    }
}
