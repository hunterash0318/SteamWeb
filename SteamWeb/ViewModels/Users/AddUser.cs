using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SteamWeb.Models;

namespace SteamWeb.ViewModels.Users
{
    public class AddUser
    {
        [Required]
        public virtual string Username { get; set; }

        [Required]
        public virtual string Bio { get; set; }

        [Required]
        public virtual decimal Wallet { get; set; }

        [Required]
        public virtual string Location { get; set; }

        [Required]
        public virtual string Password { get; set; }

        public virtual bool IsAdmin { get; set; }
    }
}
