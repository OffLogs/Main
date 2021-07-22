using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(12)]
    public class _12_CreateTable_Application2Users : MyMigration
    {
        public override void Up()
        {
            // logs
            Create.Table("application_users")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("user_id").AsInt64().NotNullable()
                .WithColumn("application_id").AsInt64().NotNullable();

            Create.ForeignKey()
                .FromTable("application_users").ForeignColumn("user_id")
                .ToTable("users").PrimaryColumn("id");

            Create.ForeignKey()
                .FromTable("application_users").ForeignColumn("application_id")
                .ToTable("applications").PrimaryColumn("id");

            Create.UniqueConstraint()
                .OnTable("application_users")
                .Columns("application_id", "user_id");

            base.Up();
        }
    }
}
