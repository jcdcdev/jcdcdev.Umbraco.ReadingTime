using jcdcdev.Umbraco.ReadingTime.Core.Composing;
using jcdcdev.Umbraco.ReadingTime.Infrastructure;
using jcdcdev.Umbraco.ReadingTime.Infrastructure.Indexing;
using jcdcdev.Umbraco.ReadingTime.Infrastructure.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Infrastructure.Manifest;
using Umbraco.Extensions;

namespace jcdcdev.Umbraco.ReadingTime.Core.Extensions;

public static class UmbracoBuilderExtensions
{
    public static IUmbracoBuilder AddReadingTime(this IUmbracoBuilder builder)
    {
        builder.PackageMigrationPlans().Add<MigrationPlan>();
        builder.Services.AddScoped<IReadingTimeService, ReadingTimeService>();
        builder.AddNotificationAsyncHandler<ContentSavingNotification, ReadingTimeNotificationHandler>();
        builder.ReadingTimeValueProviders().Append<ReadingTimeTextValueProvider>();
        builder.ReadingTimeValueProviders().Append<BlockReadingTimeValueProvider>();
        builder.Services.AddSingleton<IPackageManifestReader, PackageManifestReader>();

        return builder;
    }
}
