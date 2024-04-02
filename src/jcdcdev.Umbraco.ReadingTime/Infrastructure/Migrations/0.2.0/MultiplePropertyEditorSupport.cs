using jcdcdev.Umbraco.ReadingTime.Core;
using Umbraco.Cms.Infrastructure.Migrations;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure.Migrations;

public class MultiplePropertyEditorSupport : MigrationBase
{
    public MultiplePropertyEditorSupport(IMigrationContext context) : base(context)
    {
    }

    protected override void Migrate()
    {
        if (!ColumnExists(Constants.TableName, "dataTypeId"))
        {
            Alter.Table(Constants.TableName)
                .AddColumn("dataTypeId")
                .AsInt32()
                .ForeignKey(global::Umbraco.Cms.Core.Constants.DatabaseSchema.Tables.Node, "id")
                .NotNullable()
                .Do();
        }

        if (!ColumnExists(Constants.TableName, "dataTypeKey"))
        {
            Alter.Table(Constants.TableName)
                .AddColumn("dataTypeKey")
                .AsInt32()
                .ForeignKey(global::Umbraco.Cms.Core.Constants.DatabaseSchema.Tables.Node, "uniqueId")
                .NotNullable()
                .Do();
        }
    }
}
