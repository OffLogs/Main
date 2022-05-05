using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(31)]
    public class _31_AlterNotificationRule : MyMigration
    {
        public override void Up()
        {
            Alter.Table("notification_rules")
                .AddColumn("title").AsString(512).NotNullable().WithDefaultValue("");
            
            base.Up();
        }
    }
}
