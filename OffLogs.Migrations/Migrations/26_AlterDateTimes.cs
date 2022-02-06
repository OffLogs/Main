using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(26)]
    public class _26_AlterDateTimes : MyMigration
    {
        public override void Up()
        {
            Alter.Table("users")
                .AlterColumn("verification_time").AsCustom("timestamptz").Nullable();
            base.Up();
        }
    }
}
