using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SteamWeb.ViewModels.Games
{
    public class GameDetail
    {
        public string Title { get; set; }

        public string Developer { get; set; }

        public string Description { get; set; }

        public string Genre { get; set; }

        public decimal Price { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
