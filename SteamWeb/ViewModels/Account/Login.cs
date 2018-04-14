using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using NHibernate;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using SteamWeb.Models;
using SteamWeb.Infrastructure.Authentication;
using System.Threading;

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
