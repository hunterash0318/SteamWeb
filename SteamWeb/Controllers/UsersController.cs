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
using SteamWeb.Infrastructure;
using SteamWeb.Infrastructure.Authentication;
using AutoMapper;

namespace SteamWeb.Controllers
{
    public class UsersController : Controller
    {
        private readonly NHibernate.ISession _session;
        private readonly ICurrentUserContext _context;

        public UsersController(NHibernate.ISession session, ICurrentUserContext context)
        {
            _session = session;
            _context = context;
        }

        [HttpGet]
        public ActionResult AddFunds()
        {
            User user = _session.Get<User>(_context.UserId);
            AddFunds funds = Mapper.Map<AddFunds>(user);
            return View(funds);
        }

        [HttpPost, ActionName("AddFunds")]
        public ActionResult ConfirmAddFunds(AddFunds funds)
        {
            if (!ModelState.IsValid)
            {
                return View(funds);
            }
            if (funds.Wallet >= (decimal)1000.0)
            {
                ModelState.AddModelError(string.Empty, "Error: Wallet amount cannot exceed $999.99");
                return View(funds);
            }
            User user = _session.Get<User>(_context.UserId);
            using (var txn = _session.BeginTransaction())
            {
                user.Wallet = funds.Wallet;
                _session.SaveOrUpdate(user);
                txn.Commit();
            }
            ModelState.AddModelError(string.Empty, "Funds successfully added!");
            return View(funds);
        }

        [HttpGet]
        public ActionResult AddUser()
        {
            if (!_context.UserType)
            {
                ViewData["error"] = "Error: you do not have that permission";
                return RedirectToAction("Index", "Games");
            }
            return View("~/Views/Users/AddUser.cshtml");
        }

        [HttpPost, ActionName("AddUser")]
        public ActionResult ConfirmAddUser(AddUser add)
        {
            if (!ModelState.IsValid)
            {
                return View(add);
            }
            User maybeUser = _session.Query<User>()
                .Where(u => u.Username == add.Username)
                .SingleOrDefault();
            if (add.Wallet >= (decimal)1000.0)
            {
                ModelState.AddModelError(string.Empty, "Error: Wallet amount cannot exceed $999.99");
                return View(add);
            }
            if (maybeUser == null)
            {
                User user = Mapper.Map<User>(add);
                user.GamesOwned = Enumerable.Empty<Game>();
                user.Friends = Enumerable.Empty<User>();

                _session.Save(user);
                ModelState.AddModelError(string.Empty, "User successfully added!");
                return View(add);
            }
            ModelState.AddModelError("Username", "Error: A user with that name already exists.");
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
                Users = UserItems,
                UserIsAdmin = _context.UserType,
                UserId = _context.UserId
            });
        }

        public ActionResult UserDetail(int Id)
        {
            User user = _session.Query<User>()
                .Where(u => u.Id == Id)
                .SingleOrDefault();
            UserDetail detail = Mapper.Map<UserDetail>(user);
            detail.UserIsAdmin = _context.UserType;
            detail.CurrentUserId = _context.UserId;
            return View(detail);
        }

        [HttpGet]
        public ActionResult DeleteUser(int Id)
        {
            if (!_context.UserType)
            {
                ViewData["error"] = "Error: you do not have that permission";
                return RedirectToAction("Index", "Games");
            }
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

        [HttpGet]
        public ActionResult EditUser(int Id)
        {
            if (!_context.UserType && !(_context.UserId == Id))
            {
                ModelState.AddModelError(string.Empty, "Error: You do not have that permission");
                return View("~/Views/Games/Index.cshtml");
            }
            User user = _session.Get<User>(Id);
            EditUser edit = Mapper.Map<EditUser>(user);
            edit.UserIsAdmin = _context.UserType;
            return View(edit);
        }

        [HttpPost, ActionName("EditUser")]
        public ActionResult ConfirmEditUser(EditUser editedUser)
        {
            if (!ModelState.IsValid)
            {
                return View(editedUser);
            }

            User maybeUser = _session.Query<User>()
                .Where(u => u.Username == editedUser.Username && u.Id != editedUser.Id)
                .SingleOrDefault();
            if (maybeUser != null)
            {
                ModelState.AddModelError(string.Empty, "Error: A user with that name already exists");
                return View();
            }
            if (editedUser.Wallet >= (decimal) 1000.0)
            {
                ModelState.AddModelError(string.Empty, "Error: Wallet amount cannot exceed $999.99");
                return View(editedUser);
            }
            User user = Mapper.Map<User>(editedUser);
            using (var txn = _session.BeginTransaction())
            {
                _session.SaveOrUpdate(user);
                txn.Commit();
            }
            return RedirectToAction("IndexUser");
        }
    }
}