using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(22)]
    public class _22_AlterForeignKey_Applications : MyMigration
    {
        public override void Up()
        {
            Delete.ForeignKey("FK_applications_user_id_users_id").OnTable("applications");

            Create.ForeignKey()
                .FromTable("applications").ForeignColumn("user_id")
                .ToTable("users").PrimaryColumn("id")
                .OnDelete(System.Data.Rule.Cascade);
            base.Up();
        }
    }
}
