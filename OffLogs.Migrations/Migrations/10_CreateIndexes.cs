using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(10)]
    public class _10_CreateIndexes : MyMigration
    {
        public override void Up()
        {
            // logs
            Create.Index("ix_logs_level")
                .OnTable("logs")
                .OnColumn("level");

            base.Up();
        }
    }
}
