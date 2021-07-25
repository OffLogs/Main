using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(15)]
    public class _15_AlterForeignKey_Logs : MyMigration
    {
        public override void Up()
        {
            // logs
            Delete.ForeignKey("FK_log_properties_log_id_logs_id").OnTable("log_properties");

            Create.ForeignKey()
                .FromTable("log_properties").ForeignColumn("log_id")
                .ToTable("logs").PrimaryColumn("id")
                .OnDelete(System.Data.Rule.Cascade);

            base.Up();
        }
    }
}
