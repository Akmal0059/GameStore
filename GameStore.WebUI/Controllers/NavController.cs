using GameStore.Domain.Abstract;
using GameStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IGameRepository repository;

        public NavController(IGameRepository repo)
        {
            repository = repo;
        }

        // GET: Nav
        public PartialViewResult Menu(string category = null)
        {
            //ViewBag.SelectedCategory = category;
            IEnumerable<CategoryInfo> categories = repository.Games
                .Select(game => new CategoryInfo() 
                { 
                    CategoryName =  game.Category,
                    IsSelected = game.Category == category
                })
                .GroupBy(p => p.CategoryName)
                .Select(g => g.First())
                .OrderBy(x => x.CategoryName);
            return PartialView(categories);
        }
    }
}