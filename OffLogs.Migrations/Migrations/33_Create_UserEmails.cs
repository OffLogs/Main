using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(33)]
    public class _33_Create_UserEmails : MyMigration
    {
        public override void Up()
        {
            Create.Table("user_emails")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("user_id").AsInt64().NotNullable()
                .WithColumn("email").AsString(200).NotNullable()
                .WithColumn("verification_token").AsString(512).Nullable()
                .WithColumn("verification_time").AsDateTime().Nullable()
                .WithColumn("create_time").AsDateTime().NotNullable()
                .WithColumn("update_time").AsDateTime().NotNullable();
            
            Create.ForeignKey()
                .FromTable("user_emails").ForeignColumn("user_id")
                .ToTable("users").PrimaryColumn("id");

            Create.UniqueConstraint()
                .OnTable("user_emails")
                .Columns(new string[]
                {
                    "user_id",
                    "email"
                });
            
            base.Up();
        }
    }
}
