using Bootstrap.Infrastructure.DatabaseMigration.Migrations.Utils;
using FluentMigrator;

namespace Bootstrap.Infrastructure.DatabaseMigration.Migrations;

[Utils.Migration(Year._2022, Month.October, Day._9)]
public class CreateCustomerTable : AutoReversingMigration
{
    public override void Up()
    {
        this.Create
            .Table("Customer")
            .WithColumn("Id")
                .AsGuid()
                .NotNullable()
                .PrimaryKey("PK_Customer")
            .WithColumn("FirstName")
                .AsString()
                .NotNullable()
            .WithColumn("LastName")
                .AsString()
                .NotNullable();
    }
}
