using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(7)]
    public class _7_CreateIndexes : MyMigration
    {
        public override void Up()
        {
            // logs
            Create.Index("ix_log_properties_log_id")
                .OnTable("log_properties")
                .OnColumn("log_id");
            
            Create.Index("ix_log_traces_log_id")
                .OnTable("log_traces")
                .OnColumn("log_id");

            base.Up();
        }
    }
}
