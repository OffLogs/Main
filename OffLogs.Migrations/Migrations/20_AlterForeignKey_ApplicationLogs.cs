using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(20)]
    public class _20_AlterForeignKey_ApplicationLogs : MyMigration
    {
        public override void Up()
        {
            Delete.ForeignKey("FK_application_users_application_id_applications_id").OnTable("application_users");

            Create.ForeignKey()
                .FromTable("application_users").ForeignColumn("application_id")
                .ToTable("applications").PrimaryColumn("id")
                .OnDelete(System.Data.Rule.Cascade);
            base.Up();
        }
    }
}
