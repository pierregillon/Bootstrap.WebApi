using FluentMigrator;

namespace Bootstrap.Infrastructure.DatabaseMigration.Migrations.Utils;

[Maintenance(MigrationStage.BeforeAll, TransactionBehavior.None)]
public class DbMigrationLockBefore : Migration
{
    public override void Up() => Execute.Sql("SELECT pg_advisory_lock(1);");

    public override void Down() => throw new System.NotImplementedException();
}

[Maintenance(MigrationStage.AfterAll, TransactionBehavior.None)]
public class DbMigrationUnlockAfter : Migration
{
    public override void Up() => Execute.Sql("SELECT pg_advisory_unlock(1);");

    public override void Down() => throw new System.NotImplementedException();
}