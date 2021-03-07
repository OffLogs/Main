using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(2)]
    public class _2_CreateTypes : MyMigration
    {
        public override void Up()
        {
            ExecuteScriptByName("2_CreateTypes");
            
            base.Up();
        }
    }
}
