using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(7)]
    public class _6_Create_Enums_3 : MyMigration
    {
        public override void Up()
        {
            Delete.Column("level").FromTable("logs");
            Rename.Column("log_level").OnTable("logs").To("level");
            
            Delete.Column("type").FromTable("request_logs");
            Rename.Column("log_type").OnTable("request_logs").To("type");

            base.Up();
        }
    }
}
