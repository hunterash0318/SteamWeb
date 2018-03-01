using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SteamWeb.ViewModels.Games
{
    public class Edit
    {
        public int  Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Developer { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }
    }
}
