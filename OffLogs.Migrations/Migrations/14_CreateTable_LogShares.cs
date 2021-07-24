using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(14)]
    public class _14_CreateTable_LogShares : MyMigration
    {
        public override void Up()
        {
            // logs
            Create.Table("log_shares")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("log_id").AsInt64().NotNullable()
                .WithColumn("token").AsString().NotNullable()
                .WithColumn("create_time").AsDateTime().NotNullable()
                .WithColumn("update_time").AsDateTime().NotNullable();

            Create.ForeignKey()
                .FromTable("log_shares").ForeignColumn("log_id")
                .ToTable("logs").PrimaryColumn("id");

            Create.UniqueConstraint()
                .OnTable("log_shares")
                .Columns("log_id");

            base.Up();
        }
    }
}
