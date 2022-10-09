namespace Bootstrap.Infrastructure.DatabaseMigration.Migrations.Utils;

public class MigrationAttribute : global::FluentMigrator.MigrationAttribute
{
    public MigrationAttribute(Year year, Month month, Day day, Hour hour = Hour._00, Minutes minute = Minutes._00)
        : base(ComputeVersion(year, month, day, hour, minute))
    {
    }

    private static long ComputeVersion(Year year, Month month, Day day, Hour hour, Minutes minute)
    {
        var dateOfMigration = DateOfMigration(year, month, day, hour, minute);

        if (dateOfMigration > Tomorrow)
            throw new ArgumentException($"Migration can't be in future {dateOfMigration:O}");

        return dateOfMigration.ToVersion();
    }

    private static DateTime DateOfMigration(Year year, Month month, Day day, Hour hour, Minutes minute) =>
        new((int)year, (int)month, (int)day, (int)hour, (int)minute, 0);

    private static DateTime Tomorrow => DateTime.Today.AddDays(1);
}