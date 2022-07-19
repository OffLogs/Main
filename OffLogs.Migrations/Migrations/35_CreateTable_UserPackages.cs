using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(35)]
    public class _35_CreateTable_UserPackages : MyMigration
    {
        public override void Up()
        {
            Create.Table("payment_package_types")
                .WithColumn("id").AsInt64().PrimaryKey()
                .WithColumn("name").AsString(50).NotNullable();
            
            Insert.IntoTable("payment_package_types")
                .Row(new { id = 1, name = "Basic" })
                .Row(new { id = 2, name = "Standart" })
                .Row(new { id = 3, name = "Pro" });
            
            Create.Table("user_payment_packages")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("user_id").AsInt64().NotNullable()
                .WithColumn("payment_package_type_id").AsInt64().NotNullable()
                .WithColumn("expiration_date").AsCustom("date").NotNullable()
                .WithColumn("create_time").AsCustom("timestamptz").NotNullable()
                .WithColumn("update_time").AsCustom("timestamptz").NotNullable();
            
            Create.ForeignKey()
                .FromTable("user_payment_packages").ForeignColumn("payment_package_type_id")
                .ToTable("payment_package_types").PrimaryColumn("id");
            
            Create.ForeignKey()
                .FromTable("user_payment_packages").ForeignColumn("user_id")
                .ToTable("users").PrimaryColumn("id");

            base.Up();
        }
    }
}
