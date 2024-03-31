using Umbraco.Cms.Core.Composing;

namespace jcdcdev.Umbraco.ReadingTime.Core.Composing;

public class ReadingTimeValueProviderCollection : BuilderCollectionBase<IReadingTimeValueProvider>
{
    public ReadingTimeValueProviderCollection(Func<IEnumerable<IReadingTimeValueProvider>> items) : base(items)
    {
    }
}
