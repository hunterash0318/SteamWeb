using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SteamWeb.Models;

namespace SteamWeb.ViewModels.Users
{
    public class SelectReceiver
    {
        public IEnumerable<User> Receivers{ get; set; }
        public int UserId { get; set; }
        public Game GameToSend { get; set; }
    }

    public class ReceiverItem
    {
        public int Id { get; set; }

        public string Username { get; set; }
    }
}
