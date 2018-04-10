using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SteamWeb.Models;

namespace SteamWeb.ViewModels.Account
{
    public class Login
    {
        [Required]
        public virtual string Username { get; set; }

        [Required]
        public virtual string Password { get; set; }

        public virtual bool Admin { get; set; }
    }
}
