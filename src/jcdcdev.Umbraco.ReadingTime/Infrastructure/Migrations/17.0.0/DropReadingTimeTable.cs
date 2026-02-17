using jcdcdev.Umbraco.ReadingTime.Core;
using Microsoft.Extensions.Logging;
using NPoco;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Persistence;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure.Migrations;

public class DropReadingTimeTable(IMigrationContext context) : AsyncMigrationBase(context)
{
    protected override Task MigrateAsync()
    {
        if (!TableExists(Constants.TableName))
        {
            Logger.LogInformation("Table {TableName} does not exist, nothing to drop", Constants.TableName);
            return Task.CompletedTask;
        }

        Logger.LogInformation("Dropping table {TableName}", Constants.TableName);

        var foreignKeys = new[]
        {
            "FK_jcdcdevReadingTime_content_umbracoNode_uniqueId",
            "FK_jcdcdevReadingTime_dataTypeKey_umbracoNode_uniqueId",
            "FK_jcdcdevReadingTime_dataTypeId_umbracoNode_uniqueId",
            "FK_jcdcdevReadingTime_umbracoNode_uniqueId"
        };

        foreach (var fk in foreignKeys)
        {
            if (ConstraintExists(Context.Database, Constants.TableName, fk))
            {
                Delete.ForeignKey(fk).OnTable(Constants.TableName).Do();
            }
        }

        Delete.Table(Constants.TableName).Do();

        Logger.LogInformation("Table {TableName} dropped successfully", Constants.TableName);
        return Task.CompletedTask;
    }

    private static bool ConstraintExists(IUmbracoDatabase database, string tableName, string key)
    {
        string sql;
        if (database.SqlContext.DatabaseType == DatabaseType.SQLite)
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
