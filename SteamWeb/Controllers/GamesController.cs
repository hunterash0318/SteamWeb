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
using SteamWeb.Infrastructure;
using SteamWeb.Infrastructure.Authentication;
using AutoMapper.QueryableExtensions;
using AutoMapper;


namespace SteamWeb.Controllers
{
    public class GamesController : Controller
    {
        private readonly NHibernate.ISession _session;
        private readonly ICurrentUserContext _context;

        public GamesController(NHibernate.ISession session, ICurrentUserContext context)
        {
            _session = session;
            _context = context;
        }

        // GET: Game
        public ActionResult Index()
        {
            User user = _session.Get<User>(_context.UserId);

            ViewData["Title"] = "Index";
            ViewData["header"] = "Index";
            
            IEnumerable<Game> AllGames = _session.Query<Game>();

            IEnumerable<GameItem> GameItems = AllGames.Select(game =>
                new GameItem
                {
                    Title = game.Title,
                    Price = game.Price,
                    ReleaseDate = game.ReleaseDate,
                    Id = game.Id,
                    IsOwned = user.GamesOwned.Contains<Game>(game)
                });

            return View(new Index
            {
                Games = GameItems,
                UserIsAdmin = _context.UserType,
                MyLibrary = false
            });
        }

        public ActionResult UserGames(int Id)
        {
            User user = _session.Get<User>(Id);
            User currentUser = _session.Get<User>(_context.UserId);

            ViewData["header"] = user.Username + "'s Library";
            IEnumerable<Game> gamesOwned = user.GamesOwned;
            IEnumerable<GameItem> GameItems = gamesOwned.Select(game =>
                new GameItem
                {
                    Title = game.Title,
                    Price = game.Price,
                    ReleaseDate = game.ReleaseDate,
                    Id = game.Id,
                    IsOwned = currentUser.GamesOwned.Contains<Game>(game)
                });

            return View("Index", new Index
            {
                Games = GameItems
            });
        }

        public ActionResult MyGames()
        {
            User user = _session.Get<User>(_context.UserId);
            ViewData["header"] = user.Username + "'s Library";
            IEnumerable<GameItem> gameItems = user.GamesOwned.Select(game => Mapper.Map<GameItem>(game));
            
            return View("Index", new Index
            {
                Games = gameItems,
                MyLibrary = true
            });
        }

        public ActionResult MyGifts()
        {
            User user = _session.Get<User>(_context.UserId);
            ViewData["header"] = user.Username + "'s Pending Gifts";

            IEnumerable<GiftItem> giftItems = user.GiftsOwned.Select(gift =>
            new GiftItem
            {
                Id = gift.Id,
                SenderId = gift.SenderId,
                GameId = gift.GameId,
                Returned = gift.Returned,
                Message = gift.Message,
                SenderName = _session.Get<User>(gift.SenderId).Username,
                GameTitle = _session.Get<Game>(gift.GameId).Title
            });

            return View("IndexGift", new IndexGift
            {
                Gifts = giftItems
            });
        }

        public ActionResult AcceptGift(int Id)
        {
            Gift gift = _session.Get<Gift>(Id);
            Game game = _session.Get<Game>(gift.GameId);
            User user = _session.Get<User>(_context.UserId);

            if (user.GamesOwned.Contains<Game>(game))
            {
                ViewData["error"] = "You already own this game. Please return to sender";
                return RedirectToAction("MyGifts");
            }

            List<Game> gamesList = new List<Game> { game };

            using (var txn = _session.BeginTransaction())
            {
                user.GiftsOwned = user.GiftsOwned.Where(g => g.Id != gift.Id);
                user.GamesOwned = user.GamesOwned.Concat(gamesList);
                _session.SaveOrUpdate(user);
                txn.Commit();
            }

            ViewData["error"] = game.Title + " has been added to your library!";
            return RedirectToAction("MyGifts");
        }

        public ActionResult ReturnGift(int Id)
        {
            Gift gift = _session.Get<Gift>(Id);
            Game game = _session.Get<Game>(gift.GameId);
            User user = _session.Get<User>(_context.UserId);
            User sender = _session.Get<User>(gift.SenderId);

            List<Gift> giftList = new List<Gift> { gift };

            using (var txn = _session.BeginTransaction())
            {
                gift.Returned = true;
                _session.SaveOrUpdate(gift);
                txn.Commit();
            }

            using (var txn = _session.BeginTransaction())
            {
                user.GiftsOwned = user.GiftsOwned.Where(g => g.Id != gift.Id);
                sender.GiftsOwned = sender.GiftsOwned.Concat(giftList);
                _session.SaveOrUpdate(user);
                _session.SaveOrUpdate(sender);
                txn.Commit();
            }

            return RedirectToAction("MyGifts");
        }

        public ActionResult RefundGift(int Id)
        {
            Gift gift = _session.Get<Gift>(Id);
            Game game = _session.Get<Game>(gift.GameId);
            User user = _session.Get<User>(_context.UserId);

            if (user.Wallet + game.Price > 999.99M)
            {
                ViewData["error"] = "Your wallet is too full to refund this game";
                return RedirectToAction("MyGifts");
            }

            using (var txn = _session.BeginTransaction())
            {
                user.GiftsOwned = user.GiftsOwned.Where(g => g.Id != gift.Id);
                user.Wallet = user.Wallet + game.Price;
                _session.SaveOrUpdate(user);
                txn.Commit();
            }

            return RedirectToAction("MyGifts");
        }

        public ActionResult Detail(int Id)
        {
            Game game = _session.Get<Game>(Id);
            GameDetail detail = Mapper.Map<GameDetail>(game);
            
            return View(detail);
        }

        [HttpGet]
        public ActionResult Buy(int Id)
        {
            Game game = _session.Get<Game>(Id);
            Buy buy = Mapper.Map<Buy>(game);
            return View(buy);
        }

        [HttpPost]
        public ActionResult Buy(Buy buy)
        {
            User maybeUser = _session.Get<User>(_context.UserId);

            if (maybeUser == null)
            {
                return RedirectToAction("Index");
            }

            Game game = _session.Get<Game>(buy.Id);
            Buy buy2 = Mapper.Map<Buy>(game);
            Game owned = maybeUser.GamesOwned.FirstOrDefault(g => g.Title == game.Title);
            if (owned != null)
            {
                ModelState.AddModelError(string.Empty, "You already own this game");
                return View(buy2);
            }

            if (maybeUser.Wallet < game.Price)
            {
                ModelState.AddModelError(string.Empty, "Insufficient funds to purchase game");
                return View(buy2);
            }

            List<Game> gameList = new List<Game> { game };

            using (var txn = _session.BeginTransaction())
            {
                maybeUser.GamesOwned = maybeUser.GamesOwned.Concat(gameList);
                maybeUser.Wallet = maybeUser.Wallet - game.Price;
                _session.SaveOrUpdate(maybeUser);
                txn.Commit();
            }

            ModelState.AddModelError(string.Empty, "Game successfully purchased!");
            return View(buy2);
        }

        [HttpGet]
        public ActionResult SendGift(int Id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendGift(SendGift sendGift)
        {
            if (!ModelState.IsValid)
            {
                return View(sendGift);
            }

            Game gameExists = _session.Query<Game>()
                .Where(g => g.Title == sendGift.GameTitle)
                .SingleOrDefault();

            if (gameExists == null)
            {
                ModelState.AddModelError("GameTitle", "No game with that title exists.");
                return View();
            }

            User userExists = _session.Query<User>()
                .Where(u => u.Username == sendGift.ReceiverName)
                .SingleOrDefault();

            if (userExists == null)
            {
                ModelState.AddModelError("ReceiverName", "No user with that name exists.");
                return View();
            }

            User sender = _session.Get<User>(_context.UserId);

            if (sender.Wallet < gameExists.Price)
            {
                ModelState.AddModelError(string.Empty, "Insufficient funds to purchase game");
                return View();
            }

            if (userExists.GiftsOwned == null)
            {
                userExists.GiftsOwned = Enumerable.Empty<Gift>();
            }

            Gift giftExists = userExists.GiftsOwned.FirstOrDefault(g => g.GameId == gameExists.Id);

            if (userExists.GamesOwned.Contains<Game>(gameExists) || (giftExists != null && !giftExists.Returned))
            {
                ModelState.AddModelError(string.Empty, "The user already owns or has been gifted this game");
                return View();
            }
            
            Gift gift = new Gift
            {
                ReceiverId = userExists.Id,
                SenderId = sender.Id,
                GameId = gameExists.Id,
                Returned = false,
                Message = sendGift.Message
            };

            List<Gift> giftList = new List<Gift> { gift };

            using (var txn = _session.BeginTransaction())
            {
                _session.SaveOrUpdate(gift);
                txn.Commit();
            }

            using (var txn = _session.BeginTransaction())
            {
                userExists.GiftsOwned = userExists.GiftsOwned.Concat(giftList);
                sender.Wallet = sender.Wallet - gameExists.Price;
                _session.SaveOrUpdate(userExists);
                _session.SaveOrUpdate(sender);
                txn.Commit();
            }

            ModelState.AddModelError(string.Empty, "Gift successfully sent!");
            return View(sendGift);
        }



        [HttpGet]
        public ActionResult Add()
        {
            if (!_context.UserType)
            {
                ViewData["error"] = "Error: you do not have that permission";
                return RedirectToAction("Index");
            }
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
                Game game = Mapper.Map<Game>(add);
                _session.Save(game);
                ModelState.AddModelError(string.Empty, "Game successfully added!");
                return View();
            }
            ModelState.AddModelError("Title", "Error: A game with that title already exists.");
            return View();
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            if (!_context.UserType)
            {
                ViewData["error"] = "Error: you do not have that permission";
                return RedirectToAction("Index");
            }
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
            if (!_context.UserType)
            {
                ViewData["error"] = "Error: you do not have that permission";
                return RedirectToAction("Index");
            }
            Game game = _session.Get<Game>(Id);
            Edit edit = Mapper.Map<Edit>(game);
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
                // This will make the error show up right next to the field that has the error, making it clearer to users of your system which part of their form they need to fix.
                // ModelState.AddModelError(nameof(editedGame.Title), "A game with that title already exists.");
                ModelState.AddModelError(string.Empty, "Error: A game with that title already exists");
                return View();
            }
            Game game = Mapper.Map<Game>(editedGame);
            using (var txn = _session.BeginTransaction())
            {
                _session.SaveOrUpdate(game);
                txn.Commit();
            }
            return RedirectToAction("Index");
        }
    }
}