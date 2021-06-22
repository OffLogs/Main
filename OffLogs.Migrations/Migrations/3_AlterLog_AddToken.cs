using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(3)]
    public class _3_AlterLog_AddToken : MyMigration
    {
        public override void Up()
        {
            Alter.Table("logs")
                .AddColumn("token").AsString(40).Nullable().Unique();
            base.Up();
        }
    }
}
