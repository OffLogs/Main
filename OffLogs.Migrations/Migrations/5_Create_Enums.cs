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

            Execute.Sql("UPDATE logs SET log_level = 1 WHERE level = 'E';");
            Execute.Sql("UPDATE logs SET log_level = 2 WHERE level = 'W';");
            Execute.Sql("UPDATE logs SET log_level = 3 WHERE level = 'F';");
            Execute.Sql("UPDATE logs SET log_level = 4 WHERE level = 'I';");
            Execute.Sql("UPDATE logs SET log_level = 5 WHERE level = 'D';");

            Delete.Column("level").FromTable("logs");

            Rename.Column("log_level").OnTable("logs").To("level");

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

            Execute.Sql("UPDATE request_logs SET log_type = 1 WHERE type = 'L';");
            Execute.Sql("UPDATE request_logs SET log_type = 2 WHERE type = 'R';");

            Delete.Column("type").FromTable("request_logs");

            Rename.Column("log_type").OnTable("request_logs").To("type");

            base.Up();
        }
    }
}
