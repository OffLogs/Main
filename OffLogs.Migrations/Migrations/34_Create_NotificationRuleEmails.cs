using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(34)]
    public class _34_Create_NotificationRuleEmails : MyMigration
    {
        public override void Up()
        {
            Create.Table("notification_rule_emails")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("notification_rule_id").AsInt64().NotNullable()
                .WithColumn("user_email_id").AsInt64().NotNullable();
            
            Create.ForeignKey()
                .FromTable("notification_rule_emails").ForeignColumn("notification_rule_id")
                .ToTable("notification_rules").PrimaryColumn("id");
            
            Create.ForeignKey()
                .FromTable("notification_rule_emails").ForeignColumn("user_email_id")
                .ToTable("user_emails").PrimaryColumn("id");

            Create.UniqueConstraint()
                .OnTable("notification_rule_emails")
                .Columns(new string[]
                {
                    "notification_rule_id",
                    "user_email_id"
                });
            
            base.Up();
        }
    }
}
