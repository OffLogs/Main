using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(19)]
    public class _19_AlterForeignKey_ApplicationLogs : MyMigration
    {
        public override void Up()
        {
            Delete.ForeignKey("FK_logs_application_id_applications_id").OnTable("logs");

            Create.ForeignKey()
                .FromTable("logs").ForeignColumn("application_id")
                .ToTable("applications").PrimaryColumn("id")
                .OnDelete(System.Data.Rule.Cascade);
            base.Up();
        }
    }
}
