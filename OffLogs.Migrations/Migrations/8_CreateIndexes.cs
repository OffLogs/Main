using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(8)]
    public class _8_CreateIndexes : MyMigration
    {
        public override void Up()
        {
            // logs
            Create.Index("ix_logs_application_id")
                .OnTable("logs")
                .OnColumn("application_id");

            base.Up();
        }
    }
}
