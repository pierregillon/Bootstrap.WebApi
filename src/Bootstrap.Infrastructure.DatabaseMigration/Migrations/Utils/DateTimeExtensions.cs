namespace Bootstrap.Infrastructure.DatabaseMigration.Migrations.Utils;

public static class DateTimeExtensions
{
    public static long ToVersion(this DateTime dateOfVersion) =>
        dateOfVersion.Year * 100000000L +
        dateOfVersion.Month * 1000000L +
        dateOfVersion.Day * 10000L +
        dateOfVersion.Hour * 100L +
        dateOfVersion.Minute;
}