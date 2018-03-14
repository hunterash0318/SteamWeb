using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using SteamWeb.Models;

namespace SteamWeb.Maps
{
    public class AdminMap : ClassMap<Admin>
    {
        public AdminMap()
        {
            Table("Admins");
            Id(u => u.Id).Not.Nullable();
            Map(u => u.Username).Not.Nullable();
            Map(u => u.Password).Nullable();
            Map(u => u.AddPermissions).Not.Nullable();
            Map(u => u.EditPermissions).Nullable();
            Map(u => u.DeletePermissions).Not.Nullable();
            Map(u => u.UserPermissions).Not.Nullable();
        }
    }
}
