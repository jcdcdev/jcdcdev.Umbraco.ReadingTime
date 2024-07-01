using Umbraco.Cms.Core.Composing;

namespace jcdcdev.Umbraco.ReadingTime.Core.Composing;

public class ReadingTimeValueProviderCollection(Func<IEnumerable<IReadingTimeValueProvider>> items) : BuilderCollectionBase<IReadingTimeValueProvider>(items);
