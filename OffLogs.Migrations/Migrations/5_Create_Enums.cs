using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(5)]
    public class _5_Create_Enums : MyMigration
    {
        public override void Up()
        {
            // logs

            Create.Table("log_levels")
                .WithColumn("id").AsInt64().PrimaryKey()
                .WithColumn("name").AsString(100);

            Insert.IntoTable("log_levels")
                .Row(new { id = 1, name = "Error" })
                .Row(new { id = 2, name = "Warning" })
                .Row(new { id = 3, name = "Fatal" })
                .Row(new { id = 4, name = "Information" })
                .Row(new { id = 5, name = "Debug" });

            Alter.Table("logs")
                .AddColumn("log_level").AsInt64().WithDefaultValue(1);

            Create.ForeignKey()
                .FromTable("logs").ForeignColumn("log_level")
                .ToTable("log_levels").PrimaryColumn("id");

            // request_logs
            Create.Table("request_log_types")
               .WithColumn("id").AsInt64().PrimaryKey()
               .WithColumn("name").AsString(100);

            Insert.IntoTable("request_log_types")
                .Row(new { id = 1, name = "Log" })
                .Row(new { id = 2, name = "Request" });

            Alter.Table("request_logs")
                .AddColumn("log_type").AsInt64().WithDefaultValue(1);

            Create.ForeignKey()
                .FromTable("request_logs").ForeignColumn("log_type")
                .ToTable("request_log_types").PrimaryColumn("id");

            base.Up();
        }
    }
}
