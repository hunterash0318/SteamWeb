using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using SteamWeb.Models;

namespace SteamWeb.Maps
{
    public class GiftMap : ClassMap<Gift>
    {

        public GiftMap()
        {
            Table("Gifts");
            Id(u => u.Id).Not.Nullable();
            Map(u => u.ReceiverId).Not.Nullable();
            Map(u => u.SenderId).Not.Nullable();
            Map(u => u.GameId).Not.Nullable();
            Map(u => u.Returned).Not.Nullable();
            Map(u => u.Message).Nullable();
        }
    }
}