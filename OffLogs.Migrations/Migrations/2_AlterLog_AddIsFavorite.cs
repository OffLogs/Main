using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(2)]
    public class _2_AlterLog_AddIsFavorite : MyMigration
    {
        public override void Up()
        {
            Alter.Table("logs")
                .AddColumn("is_favorite").AsBoolean().WithDefaultValue(false).NotNullable();
            base.Up();
        }
    }
}
