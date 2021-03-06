using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(1)]
    public class _1_Init : MyMigration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserName").AsString(200).Unique()
                .WithColumn("Email").AsString(200).Nullable()
                .WithColumn("PasswordHash").AsBinary()
                .WithColumn("PasswordSalt").AsBinary()
                .WithColumn("CreateTime").AsDateTime()
                .WithColumn("UpdateTime").AsDateTime();
            
            Create.Table("Applications")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserId").AsInt64()
                .WithColumn("Name").AsString(200)   
                .WithColumn("ApiToken").AsString(2048)
                .WithColumn("CreateTime").AsDateTime()
                .WithColumn("UpdateTime").AsDateTime();

            Create.ForeignKey()
                .FromTable("Applications").ForeignColumn("UserId")
                .ToTable("Users").PrimaryColumn("Id");
            
            Create.Table("Logs")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("ApplicationId").AsInt64()
                .WithColumn("Level").AsString(20)
                .WithColumn("Message").AsString(2048)
                .WithColumn("LogTime").AsDateTime()
                .WithColumn("CreateTime").AsDateTime();
            
            Create.ForeignKey()
                .FromTable("Logs").ForeignColumn("ApplicationId")
                .ToTable("Applications").PrimaryColumn("Id");
            
            Create.Table("LogProperties")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("LogId").AsInt64()
                .WithColumn("Level").AsString(20)
                .WithColumn("Key").AsString(200)
                .WithColumn("Value").AsString(2048)
                .WithColumn("CreateTime").AsDateTime();
            
            Create.ForeignKey()
                .FromTable("LogProperties").ForeignColumn("LogId")
                .ToTable("Logs").PrimaryColumn("Id");
            
            Create.Table("LogTraces")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("LogId").AsInt64()
                .WithColumn("Trace").AsString(2048)
                .WithColumn("CreateTime").AsDateTime();
            
            Create.ForeignKey()
                .FromTable("LogTraces").ForeignColumn("LogId")
                .ToTable("Logs").PrimaryColumn("Id");
            
            base.Up();
        }
    }
}
