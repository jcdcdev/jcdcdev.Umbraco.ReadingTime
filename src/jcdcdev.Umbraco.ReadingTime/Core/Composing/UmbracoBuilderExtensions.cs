using Umbraco.Cms.Core.DependencyInjection;

namespace jcdcdev.Umbraco.ReadingTime.Core.Composing;

public static class UmbracoBuilderExtensions
{
    public static ReadingTimeValueProviderCollectionBuilder ReadingTimeValueProviders(this IUmbracoBuilder builder) => builder.WithCollectionBuilder<ReadingTimeValueProviderCollectionBuilder>();
}