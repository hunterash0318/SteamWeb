using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using SteamWeb.Models;

namespace SteamWeb.Maps
{
    public class GameMap : ClassMap<Game>
    {
        
        public GameMap()
        {
            Table("Games");
            Id(u => u.Id).Not.Nullable();
            Map(u => u.Title).Not.Nullable();
            Map(u => u.Developer).Not.Nullable();
            Map(u => u.Genre).Not.Nullable();
            Map(u => u.Price).Not.Nullable();
            Map(u => u.Description).Not.Nullable();
            Map(u => u.ReleaseDate).Not.Nullable();
        }
    }
}
