using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using SteamWeb.Models;
using NHibernate;
using NHibernate.Linq;
using SteamWeb.ViewModels.Games;
using SteamWeb.ViewModels.Users;
using SteamWeb.ViewModels.Account;
using SteamWeb.Infrastructure.Authentication;
using SteamWeb.Infrastructure;
using System.Net;

namespace SteamWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly NHibernate.ISession _session;
        private readonly ICurrentUserContext _context;
        private readonly IAuthenticationManager _authManager;
        public AccountController(ICurrentUserContext context, NHibernate.ISession session, IAuthenticationManager authManager)
        {
            _context = context;
            _session = session;
            _authManager = authManager;
        }

        [HttpGet]
        public IActionResult Login()
        {

            return View("~/Views/Account/Login.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
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

            var user = _session.Get<User>(maybeUser.Id);

            var userInformations = new List<UserInfo>
            {
                new UserNameInfo(user.Username),
                new UserIdInfo(user.Id),
                new UserTypeInfo(user.IsAdmin),
            };

            await _authManager.SignInAsync(userInformations, new Infrastructure.Authentication.CookieOptions()
            {
                AllowRefresh = true,
                ExpirationDateTime = DateTime.Now.AddHours(1),
                IsPersistent = false
            });
            

            return RedirectToAction("Index", "Games");
        }
    }
}