using jcdcdev.Umbraco.ReadingTime.Core;
using jcdcdev.Umbraco.ReadingTime.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Infrastructure.Migrations;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure.Migrations;

public class RebuildDatabase : MigrationBase
{
    private readonly IReadingTimeService _readingTimeService;

    public RebuildDatabase(IMigrationContext context, IReadingTimeService readingTimeService) : base(context)
    {
        _readingTimeService = readingTimeService;
    }

    protected override void Migrate()
    {
        Logger.LogInformation("Rebuilding ReadingTime database");
        if (TableExists(Constants.TableName))
        {
            Delete.ForeignKey("FK_jcdcdevReadingTime_umbracoNode_uniqueId").OnTable(Constants.TableName).Do();
            Delete.Table(Constants.TableName).Do();
        }

        Create.Table<ReadingTimePoco>().Do();

        _readingTimeService.ScanAll();
    }
}
