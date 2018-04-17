using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SteamWeb.Models;

namespace SteamWeb.ViewModels.Users
{
    public class AddFunds
    {
        public virtual int Id { get; set; }

        public virtual decimal Wallet { get; set; }
    }
}
