using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SteamWeb.Models;

namespace SteamWeb.ViewModels.Users
{
    public class IndexUser
    {
        public IEnumerable<UserItem> Users { get; set; }

    }
    public class UserItem
    {
        public string Username { get; set; }
        public string Bio { get; set; }
        public decimal Wallet { get; set; }

        public int Id { get; set; }
    }
}