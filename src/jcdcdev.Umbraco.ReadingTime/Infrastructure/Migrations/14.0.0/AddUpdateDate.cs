using jcdcdev.Umbraco.ReadingTime.Core;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseModelDefinitions;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure.Migrations;

public class AddUpdateDate(IMigrationContext context) : MigrationBase(context)
{
    protected override void Migrate()
    {
        Logger.LogInformation("Adding updateDate column to table {Table}", Constants.TableName);

        if (ColumnExists(Constants.TableName, "updateDate"))
        {
            return;
        }

        Alter.Table(Constants.TableName)
            .AddColumn("updateDate")
            .AsDateTime()
            .NotNullable()
            .WithDefault(SystemMethods.CurrentDateTime)
            .Do();
    }
}
