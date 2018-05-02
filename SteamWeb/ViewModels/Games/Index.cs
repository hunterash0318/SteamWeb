using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SteamWeb.Models;

namespace SteamWeb.ViewModels.Games
{
    public class Index
    {
        public IEnumerable<GameItem> Games { get; set; }
        public bool UserIsAdmin { get; set; }
        public bool MyLibrary { get; set; }
    }
    public class GameItem
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsOwned { get; set; }

        public int Id { get; set; }
    }
}
