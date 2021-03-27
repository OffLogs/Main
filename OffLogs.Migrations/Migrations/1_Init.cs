using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(1)]
    public class _1_Init : MyMigration
    {
        public override void Up()
        {
            ExecuteScriptByName("1_create_types");
            
            Create.Table("users")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("user_name").AsString(200).Unique()
                .WithColumn("email").AsString(200).Unique()
                .WithColumn("password_hash").AsBinary()
                .WithColumn("password_salt").AsBinary()
                .WithColumn("create_time").AsDateTime()
                .WithColumn("update_time").AsDateTime();
            
            Create.Table("applications")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("user_id").AsInt64()
                .WithColumn("name").AsString(200)   
                .WithColumn("api_token").AsString(2048)
                .WithColumn("create_time").AsDateTime()
                .WithColumn("update_time").AsDateTime();

            Create.ForeignKey()
                .FromTable("applications").ForeignColumn("user_id")
                .ToTable("users").PrimaryColumn("id");
            
            Create.Table("logs")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("application_id").AsInt64()
                .WithColumn("level").AsString(5)
                .WithColumn("message").AsString(2048)
                .WithColumn("log_time").AsDateTime()
                .WithColumn("create_time").AsDateTime();
            
            Execute.Sql("ALTER TABLE logs ADD CONSTRAINT cs_logs_check_log_level CHECK(level IN ('E', 'W', 'F', 'I'))");
            
            Create.ForeignKey()
                .FromTable("logs").ForeignColumn("application_id")
                .ToTable("applications").PrimaryColumn("id");
            
            Create.Table("log_properties")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("log_id").AsInt64()
                .WithColumn("key").AsString(200)
                .WithColumn("value").AsString(2048)
                .WithColumn("create_time").AsDateTime();
            
            Create.ForeignKey()
                .FromTable("log_properties").ForeignColumn("log_id")
                .ToTable("logs").PrimaryColumn("id");
            
            Create.Table("log_traces")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("log_id").AsInt64()
                .WithColumn("trace").AsString(2048)
                .WithColumn("create_time").AsDateTime();
            
            Create.ForeignKey()
                .FromTable("log_traces").ForeignColumn("log_id")
                .ToTable("logs").PrimaryColumn("id");
            
            base.Up();
        }
    }
}
