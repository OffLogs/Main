using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(6)]
    public class _6_Create_Enums_2 : MyMigration
    {
        public override void Up()
        {
            Execute.Sql("UPDATE logs SET log_level = 1 WHERE level = 'E';");
            Execute.Sql("UPDATE logs SET log_level = 2 WHERE level = 'W';");
            Execute.Sql("UPDATE logs SET log_level = 3 WHERE level = 'F';");
            Execute.Sql("UPDATE logs SET log_level = 4 WHERE level = 'I';");
            Execute.Sql("UPDATE logs SET log_level = 5 WHERE level = 'D';");

            Execute.Sql("UPDATE request_logs SET log_type = 1 WHERE type = 'L';");
            Execute.Sql("UPDATE request_logs SET log_type = 2 WHERE type = 'R';");

            base.Up();
        }
    }
}
