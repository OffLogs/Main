using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(16)]
    public class _16_AlterForeignKey_Logs : MyMigration
    {
        public override void Up()
        {
            // logs
            Delete.ForeignKey("FK_log_traces_log_id_logs_id").OnTable("log_traces");

            Create.ForeignKey()
                .FromTable("log_traces").ForeignColumn("log_id")
                .ToTable("logs").PrimaryColumn("id")
                .OnDelete(System.Data.Rule.Cascade);

            base.Up();
        }
    }
}
