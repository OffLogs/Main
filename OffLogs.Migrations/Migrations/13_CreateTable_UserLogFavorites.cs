using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(13)]
    public class _13_CreateTable_UserLogFavorites : MyMigration
    {
        public override void Up()
        {
            // logs
            Create.Table("log_favorites")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("user_id").AsInt64().NotNullable()
                .WithColumn("log_id").AsInt64().NotNullable();

            Create.ForeignKey()
                .FromTable("log_favorites").ForeignColumn("user_id")
                .ToTable("users").PrimaryColumn("id");

            Create.ForeignKey()
                .FromTable("log_favorites").ForeignColumn("log_id")
                .ToTable("logs").PrimaryColumn("id");

            Create.UniqueConstraint()
                .OnTable("log_favorites")
                .Columns("log_id", "user_id");

            Delete.Column("is_favorite").FromTable("logs");

            base.Up();
        }
    }
}
