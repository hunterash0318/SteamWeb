using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace FluentMigrations
{
    [Migration(201803141220)]
    public class M20180314_1220_AddAdmins : Migration
    {
        public override void Up()
        {
            /* Create Users Table */
            Create.Table("Admins")
                .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey()
                .WithColumn("Username").AsString(100).NotNullable()
                .WithColumn("Password").AsString().NotNullable()
                .WithColumn("AddPermissions").AsBoolean().NotNullable()
                .WithColumn("EditPermissions").AsBoolean().NotNullable()
                .WithColumn("DeletePermissions").AsBoolean().NotNullable()
                .WithColumn("UserPermissions").AsBoolean().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Admins");
        }
    }
}
