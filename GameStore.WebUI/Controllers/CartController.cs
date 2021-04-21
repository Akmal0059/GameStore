using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IGameRepository repository;
        public CartController(IGameRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index(string returnUrl)
        {
            var model = new CartIndexViewModel() 
            { 
                Cart = GetCart(),
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        public RedirectToRouteResult AddToCart(int Id, string returnUrl)
        {
            Game game = repository.Games
                .FirstOrDefault(g => g.Id == Id);

            if (game != null)
            {
                GetCart().AddItem(game, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(int gameId, string returnUrl)
        {
            Game game = repository.Games
                .FirstOrDefault(g => g.Id == gameId);

            if (game != null)
            {
                GetCart().RemoveLine(game);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
    }
}