﻿using System;
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
    public class GamesController : Controller
    {
        private readonly NHibernate.ISession _session;

        public GamesController(NHibernate.ISession session)
        {
            _session = session;
        }



        // GET: Game
        public ActionResult Index()
        {
            
            ViewData["Title"] = "Index";
            ViewData["header"] = "Index";
            
            IEnumerable<Game> AllGames = _session.Query<Game>();

            IEnumerable<GameItem> GameItems = AllGames.Select(game =>
                new GameItem
                {
                    Title = game.Title,
                    Price = game.Price,
                    ReleaseDate = game.ReleaseDate,
                    Id = game.Id
                });

            return View(new Index
            {
                Games = GameItems
            });
        }

        public ActionResult UserGames(int Id)
        {
            User user = _session.Get<User>(Id);
            ViewData["header"] = user.Username + "'s Library";
            IEnumerable<Game> gamesOwned = user.GamesOwned;
            IEnumerable<GameItem> GameItems = gamesOwned.Select(game =>
                new GameItem
                {
                    Title = game.Title,
                    Price = game.Price,
                    ReleaseDate = game.ReleaseDate,
                    Id = game.Id
                });

            return View("Index", new Index
            {
                Games = GameItems
            });
        }

        public ActionResult Detail(int Id)
        {
            Game game = _session.Query<Game>()
                .Where(g => g.Id == Id)
                .SingleOrDefault();
            GameDetail detail = new GameDetail
            {
                Title = game.Title,
                Developer = game.Developer,
                Description = game.Description,
                Genre = game.Genre,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate,

            };
            return View(detail);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View("~/Views/Games/Add.cshtml");
        }

        [HttpPost]
        public ActionResult Add(Add add)
        {
            if (!ModelState.IsValid)
            {
                return View(add);
            }
            Game maybeGame = _session.Query<Game>()
                .Where(g => g.Title == add.Title)
                .SingleOrDefault();
            if(maybeGame == null)
            {
                Game game = new Game
                {
                    Title = add.Title,
                    Developer = add.Developer,
                    Description = add.Description,
                    Genre = add.Genre,
                    Price = add.Price,
                    ReleaseDate = add.ReleaseDate
                };
                _session.Save(game);
                ViewData["error"] = "Game successfully added!";
                return View();
            }
            ModelState.AddModelError("Title", "Error: A game with that title already exists.");
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            Game game = _session.Query<Game>()
                .Where(g => g.Id == Id)
                .SingleOrDefault();
            return View(game);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmDelete(int Id)
        {
            using (var txn = _session.BeginTransaction())
            {
                Game game = _session.Get<Game>(Id);
                _session.Delete(game);
                txn.Commit();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            Game game = _session.Get<Game>(Id);
            Edit edit = new Edit
            {
                Id = game.Id,
                Title = game.Title,
                Developer = game.Developer,
                Description = game.Description,
                Genre = game.Genre,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate,
            };
            return View(edit);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult ConfirmEdit(Edit editedGame)
        {
            if (!ModelState.IsValid)
            {
                return View(editedGame);
            }

            // [Ryan]: Here, we need to make sure that there is not already another game with the same title but different ID.
            // If we just checked for games with the same title, we would find the one that we are currently editing!!
            Game maybeGame = _session.Query<Game>()
                .Where(g => g.Title == editedGame.Title && g.Id != editedGame.Id)
                .SingleOrDefault();
            if(maybeGame != null)
            {
                //TODO: Use ModelState.AddModelError here to add a model error to the Title field!
                ViewData["error"] = "Error: A game with that title already exists";
                return View();
            }
            Game game = _session.Get<Game>(editedGame.Id);
            using (var txn = _session.BeginTransaction())
            {
                game.Title = editedGame.Title;
                game.Developer = editedGame.Developer;
                game.Description = editedGame.Description;
                game.Genre = editedGame.Genre;
                game.Price = editedGame.Price;
                game.ReleaseDate = editedGame.ReleaseDate;
                _session.SaveOrUpdate(game);
                txn.Commit();
            }
            return RedirectToAction("Index");
        }
    }
}