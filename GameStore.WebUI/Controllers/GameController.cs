using GameStore.Domain.Abstract;
using GameStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.WebUI.Controllers
{
    public class GameController : Controller
    {
        private IGameRepository repos;
        public int pageSize = 4;
        public GameController(IGameRepository repo)
        {
            repos = repo;
        }
        public ViewResult List(string category, int page = 1)
        {
            GamesListViewModel model = new GamesListViewModel()
            {
                Games = repos.Games
                        .Where(x=> category == (string)null || x.Category == category)
                        .OrderBy(x => x.Id)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                        repos.Games.Count() :
                        repos.Games.Where(game => game.Category == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }
        //public ActionResult Index()
        //{
        //    return View();
        //}
    }
}