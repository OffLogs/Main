﻿using FluentMigrator;
using OffLogs.Migrations.Code;

namespace OffLogs.Migrations.Migrations
{
    [Migration(4)]
    public class _4_CreateTable_RequestLog : MyMigration
    {
        public override void Up()
        {
            Create.Table("request_logs")
                .WithColumn("id").AsInt64().PrimaryKey().Identity()
                .WithColumn("type").AsString(4)
                .WithColumn("token").AsString(512).Indexed().Nullable()
                .WithColumn("client_ip").AsString(200).Nullable()
                .WithColumn("data").AsCustom("text").Nullable()
                .WithColumn("create_time").AsDateTime();
            
            Execute.Sql("ALTER TABLE request_logs ADD CONSTRAINT cs_request_logs_check_type CHECK(type IN ('L', 'R'))");
            
            base.Up();
        }
    }
}