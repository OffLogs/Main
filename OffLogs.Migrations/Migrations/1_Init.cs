using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(1)]
    public class _1_UserImageTable : MyMigration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsInt64().Identity()
                .WithColumn("UserName").AsString(200)   
                .WithColumn("Email").AsString(200)
                .WithColumn("PasswordHash").AsBinary()
                .WithColumn("PasswordSalt").AsBinary()
                .WithColumn("CreateTime").AsDateTime()
                .WithColumn("UpdateTime").AsDateTime();
            
            Create.Table("Applications")
                .WithColumn("Id").AsInt64().Identity()
                .WithColumn("UserId").AsInt64()
                    .ForeignKey()
                    .ReferencedBy("FK_Applications_UserId", "Users", "Id")
                .WithColumn("Name").AsString(200)   
                .WithColumn("ApiToken").AsString(2048)
                .WithColumn("CreateTime").AsDateTime()
                .WithColumn("UpdateTime").AsDateTime();
            
            Create.Table("Logs")
                .WithColumn("Id").AsInt64().Identity()
                .WithColumn("ApplicationId").AsInt64()
                    .ForeignKey()
                    .ReferencedBy("FK_Logs_ApplicationId", "Applications", "Id")
                .WithColumn("Level").AsString(20)
                .WithColumn("Message").AsString(2048)
                .WithColumn("Message").AsString(2048)
                .WithColumn("LogTime").AsDateTime()
                .WithColumn("CreateTime").AsDateTime();
            
            Create.Table("LogProperties")
                .WithColumn("Id").AsInt64().Identity()
                .WithColumn("LogId").AsInt64()
                    .ForeignKey()
                    .ReferencedBy("FK_LogProperties_LogId", "Applications", "Id")
                .WithColumn("Level").AsString(20)
                .WithColumn("Key").AsString(200)
                .WithColumn("Value").AsString(2048)
                .WithColumn("CreateTime").AsDateTime();
            
            Create.Table("LogTraces")
                .WithColumn("Id").AsInt64().Identity()
                .WithColumn("LogId").AsInt64()
                    .ForeignKey()
                    .ReferencedBy("FK_LogTraces_LogId", "Applications", "Id")
                .WithColumn("Trace").AsString(2048)
                .WithColumn("CreateTime").AsDateTime();
            
            base.Up();
        }
    }
}
