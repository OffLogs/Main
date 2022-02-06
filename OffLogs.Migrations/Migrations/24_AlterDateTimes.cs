using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(24)]
    public class _24_AlterDateTimes : MyMigration
    {
        public override void Up()
        {
            Alter.Table("users")
                .AlterColumn("create_time").AsCustom("timestamptz").NotNullable()
                .AlterColumn("update_time").AsCustom("timestamptz").NotNullable();
            base.Up();
        }
    }
}
