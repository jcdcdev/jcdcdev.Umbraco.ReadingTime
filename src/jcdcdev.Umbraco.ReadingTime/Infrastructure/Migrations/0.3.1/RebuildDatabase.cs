using jcdcdev.Umbraco.ReadingTime.Core;
using jcdcdev.Umbraco.ReadingTime.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Persistence;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure.Migrations;

public class RebuildDatabase : MigrationBase
{
    public RebuildDatabase(IMigrationContext context) : base(context)
    {
    }

    protected override void Migrate()
    {
        Logger.LogInformation("Rebuilding ReadingTime database");
        if (TableExists(Constants.TableName))
        {
            // Check if foreign key exists
            var fak = "FK_jcdcdevReadingTime_umbracoNode_uniqueId";
            var tableName = Constants.TableName;
            if (ConstraintExists(Context.Database, tableName, fak))
            {
                Delete.ForeignKey(fak).OnTable(Constants.TableName).Do();
            }

            Delete.Table(Constants.TableName).Do();
        }

        Create.Table<ReadingTimePoco>().Do();
    }

    private static bool ConstraintExists(IUmbracoDatabase database, string tableName, string key)
    {
        string sql;
        if (database.SqlContext.DatabaseType == NPoco.DatabaseType.SQLite)
        {
            sql = $"SELECT COUNT(*) FROM sqlite_master WHERE type = 'index' AND name = '{key}' AND tbl_name = '{tableName}'";
        }
        else
        {
            sql = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE CONSTRAINT_NAME = '{key}' AND TABLE_NAME = '{tableName}'";
        }

        var count = database.ExecuteScalar<int>(sql);
        return count > 0;
    }
}
