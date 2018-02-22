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
            ViewData["error"] = "Error: A game with that title already exists";
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
        public ActionResult ConfirmEdit(Edit editedGame, int Id)
        {
            Game game = _session.Get<Game>(Id);
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