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
        private IOrderProcessor orderProcessor;
        public CartController(IGameRepository repo, IOrderProcessor orderProc)
        {
            repository = repo;
            orderProcessor = orderProc;
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

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if(cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Ваша корзина пуста!");
            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            return View(shippingDetails);
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