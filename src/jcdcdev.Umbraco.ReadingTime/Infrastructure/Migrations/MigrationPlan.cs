using jcdcdev.Umbraco.ReadingTime.Core;
using Umbraco.Cms.Core.Packaging;

namespace jcdcdev.Umbraco.ReadingTime.Infrastructure.Migrations;

public class MigrationPlan : PackageMigrationPlan
{
    public MigrationPlan() : base(Constants.Package.Name)
    {
    }

    protected override void DefinePlan()
    {
        From(string.Empty);
        To<InitialMigration>(nameof(InitialMigration));
    }
}