using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(18)]
    public class _18_AlterForeignKey_Logs : MyMigration
    {
        public override void Up()
        {
            Delete.UniqueConstraint("UC_log_shares_log_id").FromTable("log_shares");
            Delete.ForeignKey("FK_log_shares_log_id_logs_id").OnTable("log_shares");

            Create.ForeignKey()
                .FromTable("log_shares").ForeignColumn("log_id")
                .ToTable("logs").PrimaryColumn("id")
                .OnDelete(System.Data.Rule.Cascade);

            Create.UniqueConstraint()
                .OnTable("log_shares")
                .Columns("log_id");

            base.Up();
        }
    }
}
