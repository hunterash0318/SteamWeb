using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SteamWeb.Models;
using NHibernate;
using NHibernate.Linq;
using SteamWeb.ViewModels.Games;
using SteamWeb.ViewModels.Users;
using SteamWeb.ViewModels.Account;

namespace SteamWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly NHibernate.ISession _session;

        public AccountController(NHibernate.ISession session)
        {
            _session = session;
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View("~/Views/Account/Login.cshtml");
        }

        [HttpPost]
        public IActionResult Login(Login login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            User maybeUser = _session.Query<User>()
                .Where(u => u.Username == login.Username)
                .SingleOrDefault();

            if ( maybeUser == null )
            {
                ModelState.AddModelError(string.Empty, "Invalid Username");
                login.Username = string.Empty;
                login.Password = string.Empty;
                return View(login);
            }

            if (maybeUser.Password != login.Password)
            {
                ModelState.AddModelError(string.Empty, "Invalid Password");
                login.Password = string.Empty;
                return View(login);
            }

            if (maybeUser.IsAdmin != login.Admin)
            {
                ModelState.AddModelError(string.Empty, "Invalid Admin Status");
                return View(login);
            }

            return View("~/Views/Games/Index.cshtml");
        }
    }
}