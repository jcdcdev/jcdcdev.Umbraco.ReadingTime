using jcdcdev.Umbraco.ReadingTime.Core.Composing;
using jcdcdev.Umbraco.ReadingTime.Infrastructure;
using jcdcdev.Umbraco.ReadingTime.Infrastructure.Indexing;
using jcdcdev.Umbraco.ReadingTime.Infrastructure.Migrations;
using jcdcdev.Umbraco.ReadingTime.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Extensions;

namespace jcdcdev.Umbraco.ReadingTime.Core.Extensions;

public static class UmbracoBuilderExtensions
{
    public static IUmbracoBuilder AddReadingTime(this IUmbracoBuilder builder)
    {
        builder.PackageMigrationPlans().Add<MigrationPlan>();
        builder.Services.AddSingleton<IReadingTimeService, ReadingTimeService>();
        builder.AddNotificationAsyncHandler<ContentPublishedNotification, ReadingTimeNotificationHandler>();
        builder.AddNotificationAsyncHandler<ContentDeletingNotification, ReadingTimeNotificationHandler>();
        builder.ReadingTimeValueProviders().Append<ReadingTimeTextValueProvider>();
        builder.Services.AddSingleton<IReadingTimeRepository, ReadingTimeRepository>();

        builder.ReadingTimeValueProviders().Append<BlockReadingTimeValueProvider>();

        return builder;
    }
}
