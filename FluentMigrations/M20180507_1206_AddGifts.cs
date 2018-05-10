using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace FluentMigrations
{
    [Migration(201805071206)]
    public class M20180507_1206_AddGifts : Migration
    {
        public override void Up()
        {
            Create.Table("Gifts")
                .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
                .WithColumn("ReceiverId").AsInt32().NotNullable()
                .WithColumn("SenderId").AsInt32().NotNullable()
                .WithColumn("GameId").AsInt32().NotNullable()
                .WithColumn("Returned").AsBoolean().NotNullable()
                .WithColumn("Message").AsString().Nullable();

            Create.Table("GiftOwnerships")
                .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
                .WithColumn("UserId").AsInt32().NotNullable()
                .WithColumn("GiftId").AsInt32().NotNullable();

            Create.ForeignKey()
                .FromTable("GiftOwnerships").ForeignColumn("UserId")
                .ToTable("Users").PrimaryColumn("Id");

            Create.ForeignKey()
                .FromTable("GiftOwnerships").ForeignColumn("GiftId")
                .ToTable("Gifts").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("Gifts");
            Delete.Table("GiftOwnerships");
        }
    }
}
