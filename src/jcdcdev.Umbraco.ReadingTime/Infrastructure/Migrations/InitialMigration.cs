using jcdcdev.Umbraco.ReadingTime.Core;
using jcdcdev.Umbraco.ReadingTime.Infrastructure.Persistence;
using Umbraco.Cms.Infrastructure.Migrations;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure.Migrations;

public class InitialMigration : MigrationBase
{
    public InitialMigration(IMigrationContext context) : base(context)
    {
    }

    protected override void Migrate()
    {
        if (TableExists(Constants.TableName))
        {
            Delete.Table(Constants.TableName).Do();
        }

        Create.Table<ContentReadingTimePoco>().Do();
    }
}