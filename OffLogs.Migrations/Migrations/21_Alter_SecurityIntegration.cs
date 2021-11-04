using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(21)]
    public class _21 : MyMigration
    {
        public override void Up()
        {
            Alter.Table("users")
                .AlterColumn("user_name").AsString(100).Nullable()
                .AddColumn("status").AsInt16().NotNullable().WithDefaultValue(1)
                .AddColumn("public_key").AsBinary().NotNullable();
            Delete.Column("password_hash").FromTable("users");
            Delete.Column("password_salt").FromTable("users");
            
            Alter.Table("log_traces")
                .AddColumn("encrypted_trace").AsBinary().NotNullable();
            Delete.Column("trace").FromTable("log_traces");
            
            Alter.Table("log_properties")
                .AddColumn("encrypted_key").AsBinary().NotNullable();
            Alter.Table("log_properties")
                .AddColumn("encrypted_value").AsBinary().NotNullable();
            Delete.Column("key").FromTable("log_properties");
            Delete.Column("value").FromTable("log_properties");

            Alter.Table("applications")
                .AddColumn("public_key").AsBinary().NotNullable()
                .AddColumn("encrypted_private_key").AsBinary().NotNullable();
                
            Alter.Table("logs")
                .AddColumn("encrypted_message").AsBinary().NotNullable();
            Delete.Column("message").FromTable("logs");
            
            base.Up();
        }
    }
}
