using jcdcdev.Umbraco.ReadingTime.Core;
using Umbraco.Cms.Core.Packaging;
using Umbraco.Cms.Infrastructure.Migrations;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure.Migrations;

public class MigrationPlan : PackageMigrationPlan
{
    public MigrationPlan() : base(Constants.Package.Name)
    {
    }

    protected override void DefinePlan()
    {
        From(string.Empty);
        To<InitialMigration>();
        To<MultiplePropertyEditorSupport>();
        To<RebuildDatabase>();
        To<AddUpdateDate>();
    }

    private void To<T>() where T : MigrationBase
    {
        To<T>(typeof(T).Name);
    }
}
