using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SteamWeb.Models;

namespace SteamWeb.ViewModels.Games
{
    public class IndexGift
    {
        public IEnumerable<GiftItem> Gifts { get; set; }
    }

    public class GiftItem
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int GameId { get; set; }

        public bool Returned { get; set; }
        public string Message { get; set; }
        public string SenderName { get; set; }
        public string GameTitle { get; set; }
    }
}
