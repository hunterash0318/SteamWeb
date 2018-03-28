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

namespace SteamWeb.Controllers
{
    public class UsersController : Controller
    {
        private readonly NHibernate.ISession _session;

        public UsersController(NHibernate.ISession session)
        {
            _session = session;
        }

        [HttpGet]
        public ActionResult AddUser()
        {
            return View("~/Views/Users/AddUser.cshtml");
        }

        [HttpPost]
        public ActionResult AddUser(AddUser add)
        {
            if (!ModelState.IsValid)
            {
                return View(add);
            }
            User maybeUser = _session.Query<User>()
                .Where(u => u.Username == add.Username)
                .SingleOrDefault();
            if (maybeUser == null)
            {
                User user = new User
                {
                    Username = add.Username,
                    Bio = add.Bio,
                    Wallet = add.Wallet,
                    Location = add.Location,
                    Password = add.Password,
                    GamesOwned = Enumerable.Empty<Game>(),
                    Friends = Enumerable.Empty<User>()
                };
                _session.Save(user);
                ViewData["error"] = "User successfully added!";
                return View();
            }
            ModelState.AddModelError("User", "Error: A user with that name already exists.");
            return View();
        }

        public IActionResult IndexUser()
        {
            IEnumerable<User> AllUsers = _session.Query<User>();

            IEnumerable<UserItem> UserItems = AllUsers.Select(user =>
                new UserItem
                {
                    Username = user.Username,
                    Wallet = user.Wallet,
                    Bio = user.Bio,
                    Id = user.Id
                });

            return View(new IndexUser
            {
                Users = UserItems
            });
        }

        public ActionResult UserDetail(int Id)
        {
            User user = _session.Query<User>()
                .Where(u => u.Id == Id)
                .SingleOrDefault();
            UserDetail detail = new UserDetail
            {
                Id = user.Id,
                Username = user.Username,
                Wallet = user.Wallet,
                Bio = user.Bio,
                Location = user.Location,
                Password = user.Password,

            };
            return View(detail);
        }

        [HttpGet]
        public ActionResult DeleteUser(int Id)
        {
            User user = _session.Query<User>()
                .Where(u => u.Id == Id)
                .SingleOrDefault();
            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        public ActionResult ConfirmDelete(int Id)
        {
            using (var txn = _session.BeginTransaction())
            {
                User user = _session.Get<User>(Id);
                _session.Delete(user);
                txn.Commit();
            }
            return RedirectToAction("IndexUser");
        }
    }
}