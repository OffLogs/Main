using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(25)]
    public class _25_AlterDateTimes : MyMigration
    {
        public override void Up()
        {
            Alter.Table("request_logs")
                .AlterColumn("create_time").AsCustom("timestamptz").NotNullable();
            
            Alter.Table("logs")
                .AlterColumn("create_time").AsCustom("timestamptz").NotNullable();
            
            Alter.Table("log_traces")
                .AlterColumn("create_time").AsCustom("timestamptz").NotNullable();
            
            Alter.Table("log_traces")
                .AlterColumn("create_time").AsCustom("timestamptz").NotNullable();
            
            Alter.Table("log_shares")
                .AlterColumn("create_time").AsCustom("timestamptz").NotNullable()
                .AlterColumn("update_time").AsCustom("timestamptz").NotNullable();
            
            Alter.Table("log_properties")
                .AlterColumn("create_time").AsCustom("timestamptz").NotNullable();
            
            Alter.Table("applications")
                .AlterColumn("create_time").AsCustom("timestamptz").NotNullable()
                .AlterColumn("update_time").AsCustom("timestamptz").NotNullable();
            base.Up();
        }
    }
}
