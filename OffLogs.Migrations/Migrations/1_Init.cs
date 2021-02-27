using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(1)]
    public class _1_UserImageTable : MyMigration
    {
        public override void Up()
        {
            Create.Table("UserImage")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserID").AsInt32().ForeignKey("FK_UserImage_UserID", "User", "ID")
                .WithColumn("Hash").AsString(200)
                .WithColumn("ImageBytes").AsCustom("varbinary(max)")
                .WithColumn("CreateDate").AsDateTime();
            
            base.Up();
        }
    }
}
