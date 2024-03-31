using jcdcdev.Umbraco.ReadingTime.Core.Composing;
using jcdcdev.Umbraco.ReadingTime.Core.Models;
using jcdcdev.Umbraco.ReadingTime.Infrastructure;
using jcdcdev.Umbraco.ReadingTime.Infrastructure.Indexing;
using jcdcdev.Umbraco.ReadingTime.Infrastructure.Migrations;
using jcdcdev.Umbraco.ReadingTime.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Extensions;

namespace jcdcdev.Umbraco.ReadingTime.Core.Extensions;

public static class UmbracoBuilderExtensions
{
    public static IUmbracoBuilder AddReadingTime(this IUmbracoBuilder builder)
    {
        builder.PackageMigrationPlans()?.Add<MigrationPlan>();
        builder.Services.AddSingleton<IReadingTimeService, ReadingTimeService>();
        builder.Services.AddOptions<ReadingTimeOptions>().BindConfiguration("ReadingTime");
        builder.AddNotificationAsyncHandler<ContentSavedNotification, ReadingTimeNotificationHandler>();
        builder.AddNotificationAsyncHandler<ContentDeletingNotification, ReadingTimeNotificationHandler>();
        builder.AddNotificationAsyncHandler<SendingContentNotification, ReadingTimeNotificationHandler>();
        builder.ReadingTimeValueProviders().Append<ReadingTimeTextValueProvider>();

#if NET6_0
        builder.ReadingTimeValueProviders().Append<LegacyBlockNestedContentReadingTimeValueProvider>();
#endif
# if NET7_0_OR_GREATER
        builder.ReadingTimeValueProviders().Append<BlockReadingTimeValueProvider>();
        builder.ReadingTimeValueProviders().Append<NestedContentReadingTimeValueProvider>();
#endif

        builder.Services.AddSingleton<IReadingTimeRepository, ReadingTimeRepository>();
        builder.ManifestFilters().Append<ManifestFilter>();
        return builder;
    }
}
