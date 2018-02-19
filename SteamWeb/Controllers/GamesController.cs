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
                    game = game
                });

            return View(new Index
            {
                Games = GameItems
            });
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View("~/Views/Games/Add.cshtml");
        }

        [HttpPost]
        public ActionResult Add(Game game)
        {
            Game maybeGame = _session.Query<Game>()
                .Where(g => g.Title == game.Title)
                .SingleOrDefault();
            if(maybeGame == null)
            {
                _session.Save(game);
                ViewData["error"] = "Game successfully added!";
                return View();
            }
            ViewData["error"] = "Error: A game with that title already exists";
            return View();
        }


        //Problems with delete
        [HttpGet]
        public ActionResult Delete(Game g)
        {

            return View(g);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmDelete(Game game)
        {
            string title = game.Title;
            _session.Delete(game);
            ViewData["error"] = $"Successfully deleted  {title}";

            return View();
        }
        
    }
}