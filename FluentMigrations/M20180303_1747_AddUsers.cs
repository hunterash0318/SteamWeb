using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace FluentMigrations
{
    [Migration(201803031747)]
    public class M20180303_1747_AddUsers : Migration
    {
        public override void Up()
        {
            /* Create Users Table */
            Create.Table("Users")
                .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
                .WithColumn("Username").AsString(100).NotNullable()
                .WithColumn("Bio").AsString().Nullable()
                .WithColumn("Wallet").AsDecimal(5, 2).NotNullable()
                .WithColumn("Location").AsString(50).Nullable()
                .WithColumn("Password").AsString(30).NotNullable();


            /* Create GameOwnerships Table */
            Create.Table("GameOwnerships")
                .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
                .WithColumn("UserId").AsInt32().NotNullable()
                .WithColumn("GameId").AsInt32().NotNullable();

            Create.ForeignKey()
                .FromTable("GameOwnerships").ForeignColumn("UserId")
                .ToTable("Users").PrimaryColumn("Id");

            Create.ForeignKey()
                .FromTable("GameOwnerships").ForeignColumn("GameId")
                .ToTable("Games").PrimaryColumn("Id");


            /* Create Relationships Table */
            // NOTE: This really functions as more of a "favorite users" list that any user can put any other users on.
            // If we wanted "friends" and "friend requests" (mutual vs. non-mutual friendships), we would need to add some stuff to this table.
            Create.Table("UserRelationships")
                .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
                .WithColumn("UserId1").AsInt32().NotNullable()
                .WithColumn("UserId2").AsInt32().NotNullable();

            Create.ForeignKey()
                .FromTable("UserRelationships").ForeignColumn("UserId1")
                .ToTable("Users").PrimaryColumn("Id");

            Create.ForeignKey()
                .FromTable("UserRelationships").ForeignColumn("UserId2")
                .ToTable("Users").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.Table("Users");
            Delete.Table("GameOwnerships");
            Delete.Table("UserRelationships");
        }
    }
}
