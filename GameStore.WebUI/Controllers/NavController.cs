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
            Category cat = repository.Categories.FirstOrDefault(x => x.CategoryName == category);
            IEnumerable<CategoryInfo> categories = repository.Games
                .Select(game => new CategoryInfo()
                {
                    Category = game.Category,
                    IsSelected = category != null ? game.Category.Id == cat.Id : false
                })
                .GroupBy(p => p.Category.Id)
                .Select(g => g.First())
                .OrderBy(x => x.Category.Id);
            return PartialView(categories);
        }
    }
}