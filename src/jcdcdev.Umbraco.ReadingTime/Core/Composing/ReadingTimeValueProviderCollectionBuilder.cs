using Umbraco.Cms.Core.Composing;

namespace jcdcdev.Umbraco.ReadingTime.Core.Composing;

public class ReadingTimeValueProviderCollectionBuilder : OrderedCollectionBuilderBase<ReadingTimeValueProviderCollectionBuilder,
    ReadingTimeValueProviderCollection, IReadingTimeValueProvider>
{
    protected override ReadingTimeValueProviderCollectionBuilder This => this;
}