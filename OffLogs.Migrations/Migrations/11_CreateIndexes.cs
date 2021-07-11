using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(11)]
    public class _11_CreateIndexes : MyMigration
    {
        public override void Up()
        {
            // logs
            Create.Index("ix_logs_log_time")
                .OnTable("logs")
                .OnColumn("log_time");
            
            base.Up();
        }
    }
}
