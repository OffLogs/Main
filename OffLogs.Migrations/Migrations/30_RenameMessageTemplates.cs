using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(30)]
    public class _30_RenameMessageTemplates : MyMigration
    {
        public override void Up()
        {
            Rename.Column("notification_message_id")
                .OnTable("notification_rules")
                .To("notification_template_id");
            
            base.Up();
        }
    }
}
