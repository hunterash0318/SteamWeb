using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SteamWeb.Models;

namespace SteamWeb.ViewModels.Users
{
    public class EditUser
    {
        public virtual int Id { get; set; }

        public virtual string Username { get; set; }

        public virtual string Bio { get; set; }

        public virtual decimal Wallet { get; set; }

        public virtual string Location { get; set; }

        public virtual string Password { get; set; }

        public virtual bool IsAdmin { get; set; }

        public virtual bool UserIsAdmin { get; set; }

        public virtual IEnumerable<User> Friends { get; set; }
    }
}
