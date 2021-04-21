using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;
using GameStore.WebUI.HTMLHelpers;
using GameStore.WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GameStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { Id = 1, Name = "Игра1"},
                new Game { Id = 2, Name = "Игра2"},
                new Game { Id = 3, Name = "Игра3"},
                new Game { Id = 4, Name = "Игра4"},
                new Game { Id = 5, Name = "Игра5"}
            });
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            GamesListViewModel result = (GamesListViewModel)controller.List(null, 2).Model;

            // Утверждение (assert)
            List<Game> games = result.Games.ToList();
            Assert.IsTrue(games.Count == 2);
            Assert.AreEqual(games[0].Name, "Игра4");
            Assert.AreEqual(games[1].Name, "Игра5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                          + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                          + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { Id = 1, Name = "Игра1"},
                new Game { Id = 2, Name = "Игра2"},
                new Game { Id = 3, Name = "Игра3"},
                new Game { Id = 4, Name = "Игра4"},
                new Game { Id = 5, Name = "Игра5"}
            });
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            // Act
            GamesListViewModel result = (GamesListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Games()
        {
            // Организация (arrange)
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            Category cat1 = new Category() { Id = 1, CategoryName = "Cat1" };
            Category cat2 = new Category() { Id = 2, CategoryName = "Cat2" };
            Category cat3 = new Category() { Id = 3, CategoryName = "Cat3" };
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { Id = 1, Name = "Игра1", Category = cat1},
                new Game { Id = 2, Name = "Игра2", Category = cat2},
                new Game { Id = 3, Name = "Игра3", Category = cat1},
                new Game { Id = 4, Name = "Игра4", Category = cat2},
                new Game { Id = 5, Name = "Игра5", Category = cat3}
            });
            mock.Setup(m => m.Categories).Returns(new List<Category>
            {
                 cat1,
                 cat2,
                 cat3
            });
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            // Action
            List<Game> result = ((GamesListViewModel)controller.List("Cat2", 1).Model).Games.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Игра2" && result[0].Category.CategoryName == "Cat2");
            Assert.IsTrue(result[1].Name == "Игра4" && result[1].Category.CategoryName == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Организация - создание имитированного хранилища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game> 
            {
                new Game { Id = 1, Name = "Игра1", Category = new Category(){Id = 2, CategoryName = "Симулятор"}},
                new Game { Id = 2, Name = "Игра2", Category = new Category(){Id = 2, CategoryName = "Симулятор"}},
                new Game { Id = 3, Name = "Игра3", Category = new Category(){Id = 3, CategoryName = "Шутер"}},
                new Game { Id = 4, Name = "Игра4", Category = new Category(){Id = 1, CategoryName = "RPG"}},
            });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Действие - получение набора категорий
            List<CategoryInfo> results = ((IEnumerable<CategoryInfo>)target.Menu().Model).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0].Category.Id, 1);
            Assert.AreEqual(results[1].Category.Id, 2);
            Assert.AreEqual(results[2].Category.Id, 3);
        }

        [TestMethod]
        public void Indicates_Selected_Category() 
        {
            // Организация - создание имитированного хранилища
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            Category cat1 = new Category() { Id = 1, CategoryName = "Симулятор" };
            Category cat2 = new Category() { Id = 2, CategoryName = "Шутер" };

            mock.Setup(m => m.Games).Returns(new Game[] 
            {
                new Game { Id = 1, Name = "Игра1", Category = cat1},
                new Game { Id = 2, Name = "Игра2", Category = cat2},
            });
            mock.Setup(m => m.Categories).Returns(new List<Category>
            {
                 cat1,
                 cat2
            });
            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Организация - определение выбранной категории
            string categoryToSelect = "Шутер";

            // Действие
            var result = ((IEnumerable<CategoryInfo>)target.Menu("Шутер").Model).First(x=>x.IsSelected);

            // Утверждение
            Assert.AreEqual(categoryToSelect, result.Category.CategoryName);
        }
        
        [TestMethod]
        public void Generate_Category_Specific_Game_Count()
        {
            /// Организация (arrange)
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            Category cat1 = new Category() { Id = 1, CategoryName = "Cat1" };
            Category cat2 = new Category() { Id = 2, CategoryName = "Cat2" };
            Category cat3 = new Category() { Id = 3, CategoryName = "Cat3" };

            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { Id = 1, Name = "Игра1", Category = cat1},
                new Game { Id = 2, Name = "Игра2", Category = cat2},
                new Game { Id = 3, Name = "Игра3", Category = cat1},
                new Game { Id = 4, Name = "Игра4", Category = cat2},
                new Game { Id = 5, Name = "Игра5", Category = cat3},
            });
            mock.Setup(m => m.Categories).Returns(new List<Category>
            {
                 cat1,
                 cat2,
                 cat3
            });
            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((GamesListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((GamesListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((GamesListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((GamesListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            // Утверждение
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
