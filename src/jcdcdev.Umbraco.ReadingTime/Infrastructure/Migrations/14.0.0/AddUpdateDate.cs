using jcdcdev.Umbraco.ReadingTime.Core;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseModelDefinitions;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure.Migrations;

public class AddUpdateDate(IMigrationContext context) : AsyncMigrationBase(context)
{
    protected override Task MigrateAsync()
    {
        Logger.LogInformation("Adding updateDate column to table {Table}", Constants.TableName);

        if (ColumnExists(Constants.TableName, "updateDate"))
        {
            return Task.CompletedTask;
        }

        Alter.Table(Constants.TableName)
            .AddColumn("updateDate")
            .AsDateTime()
            .NotNullable()
            .WithDefault(SystemMethods.CurrentDateTime)
            .Do();

        return Task.CompletedTask;
    }
}
