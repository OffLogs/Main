using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(32)]
    public class _32_AlterLogs_AddIndexes : MyMigration
    {
        public override void Up()
        {
            Create.Index("ix_logs_create_time")
                .OnTable("logs")
                .OnColumn("create_time");
            
            base.Up();
        }
    }
}
