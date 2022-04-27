using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(29)]
    public class _29_RenameMessageTemplates : MyMigration
    {
        public override void Up()
        {
            Rename.Table("notification_messages").To("notification_message_templates");
            
            base.Up();
        }
    }
}
