using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using SteamWeb.Models;

namespace SteamWeb.Maps
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("Users");
            Id(u => u.Id).Not.Nullable();
            Map(u => u.Username).Not.Nullable();
            Map(u => u.Bio).Nullable();
            Map(u => u.Wallet).Not.Nullable();
            Map(u => u.Location).Nullable();
            Map(u => u.Password).Not.Nullable();
            //Problems here
            HasManyToMany(u => u.GamesOwned);
            HasManyToMany(u => u.Friends)
                .Table("UserRelationships")
                .ParentKeyColumn("UserId1")
                .ChildKeyColumn("UserId2");
        }
    }
}
