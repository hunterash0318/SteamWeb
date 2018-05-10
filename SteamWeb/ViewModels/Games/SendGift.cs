using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SteamWeb.ViewModels.Games
{
    public class SendGift
    {
        [Required]
        [Display(Name = "Receiver Name")]
        public string ReceiverName { get; set; }

        [Required]
        [Display(Name = "Game Title")]
        public string GameTitle { get; set; }

        public string Message { get; set; }
    }
}
