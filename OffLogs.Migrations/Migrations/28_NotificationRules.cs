using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(28)]
    public class _28_NotificationRules : MyMigration
    {
        public override void Up()
        {
            Create.Table("notification_types")
                .WithColumn("id").AsInt64().PrimaryKey()
                .WithColumn("name").AsString(100);

            Insert.IntoTable("notification_types")
                .Row(new { id = 1, name = "Email" });
            
            Create.Table("logic_operator_types")
                .WithColumn("id").AsInt64().PrimaryKey()
                .WithColumn("name").AsString(100);

            Insert.IntoTable("logic_operator_types")
                .Row(new { id = 1, name = "Or" })
                .Row(new { id = 2, name = "And" });
            
            Create.Table("notification_messages")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("subject").AsString().NotNullable()
                .WithColumn("body").AsString().NotNullable()
                .WithColumn("create_time").AsDateTime()
                .WithColumn("update_time").AsDateTime();
            
            Create.Table("notification_rules")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("user_id").AsInt64().NotNullable()
                .WithColumn("application_id").AsInt64().Nullable()
                .WithColumn("notification_message_id").AsInt64().NotNullable()
                .WithColumn("notification_type_id").AsInt64().NotNullable()
                .WithColumn("logic_operator_type_id").AsInt64().NotNullable()
                
                .WithColumn("period").AsInt32().NotNullable()
                .WithColumn("last_execution_time").AsDateTime().Nullable()
                .WithColumn("is_executing").AsBoolean().NotNullable().WithDefaultValue(false)
                
                .WithColumn("create_time").AsDateTime()
                .WithColumn("update_time").AsDateTime();

            Create.Table("notification_conditions")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("notification_rule_id").AsInt64().NotNullable()
                .WithColumn("field_type").AsString(20).NotNullable()
                .WithColumn("create_time").AsDateTime()
                .WithColumn("update_time").AsDateTime();
            
            Create.ForeignKey()
                .FromTable("notification_rules").ForeignColumn("user_id")
                .ToTable("applications").PrimaryColumn("id");
            
            Create.ForeignKey()
                .FromTable("notification_rules").ForeignColumn("application_id")
                .ToTable("users").PrimaryColumn("id");

            Create.ForeignKey()
                .FromTable("notification_rules").ForeignColumn("notification_message_id")
                .ToTable("notification_messages").PrimaryColumn("id");

            Create.ForeignKey()
                .FromTable("notification_rules").ForeignColumn("logic_operator_type_id")
                .ToTable("logic_operator_types").PrimaryColumn("id");

            Create.ForeignKey()
                .FromTable("notification_rules").ForeignColumn("notification_type_id")
                .ToTable("notification_types").PrimaryColumn("id");
            
            Create.ForeignKey()
                .FromTable("notification_conditions").ForeignColumn("notification_rule_id")
                .ToTable("notification_rules").PrimaryColumn("id");
            
            Create.ForeignKey()
                .FromTable("notification_rules").ForeignColumn("notification_type_id")
                .ToTable("notification_types").PrimaryColumn("id");
            
            base.Up();
        }
    }
}
