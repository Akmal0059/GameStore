﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GameStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: null,
                url: "",
                defaults: new { controller = "Game", action = "List", category = (string)null, page = 1 }
                );

            routes.MapRoute(
                name: null,
                url: "Page{page}",
                defaults: new { controller = "Game", action = "List", category = (string)null},
                constraints: new { page = @"\d+" }
                );

            routes.MapRoute(
                name: null,
                url: "{category}",
                defaults: new { controller = "Game", action = "List", page = 1 }
                );

            routes.MapRoute(
                name: null,
                url: "{category}/Page{page}",
                defaults: new { controller = "Game", action = "List" },
                constraints: new { page = @"\d+" }
                );

            routes.MapRoute(
                name: null,
                url: "{controller}/{action}"
                );
        }
    }
}
