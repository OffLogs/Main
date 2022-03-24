using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(27)]
    public class _27_AlterUsers : MyMigration
    {
        public override void Up()
        {
            Alter.Table("users")
                .AddColumn("signed_data").AsBinary().NotNullable()
                .AddColumn("sign").AsBinary().NotNullable();
            base.Up();
        }
    }
}
