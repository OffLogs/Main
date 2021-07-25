using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(17)]
    public class _17_AlterForeignKey_Logs : MyMigration
    {
        public override void Up()
        {
            // logs
            Delete.UniqueConstraint("UC_log_favorites_log_id_user_id").FromTable("log_favorites");
            Delete.ForeignKey("FK_log_favorites_log_id_logs_id").OnTable("log_favorites");

            Create.ForeignKey()
                .FromTable("log_favorites").ForeignColumn("log_id")
                .ToTable("logs").PrimaryColumn("id")
                .OnDelete(System.Data.Rule.Cascade);

            Create.UniqueConstraint()
                .OnTable("log_favorites")
                .Columns("user_id", "log_id");

            base.Up();
        }
    }
}
