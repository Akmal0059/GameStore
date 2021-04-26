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

        public ViewResult Index(Cart cart, string returnUrl)
        {
            var model = new CartIndexViewModel() 
            { 
                Cart = cart,
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public RedirectResult AddToCart(Cart cart, int Id, string returnUrl)
        {
            Game game = repository.Games
                .FirstOrDefault(g => g.Id == Id);

            if (game != null)
            {
                cart.AddItem(game, 1);
            }
            return Redirect(returnUrl);
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int Id, string returnUrl)
        {
            Game game = repository.Games
                .FirstOrDefault(g => g.Id == Id);

            if (game != null)
            {
                cart.RemoveLine(game);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
    }
}