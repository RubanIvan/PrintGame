using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PrintGame
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Page",
               url: "page/{PageID}",
               defaults: new { controller = "Page", action = "Index", PageID = UrlParameter.Optional },
               constraints: new { PageID = @"\d+" }
           );



            routes.MapRoute(
            name: "Strategic",
            url: "strategic/{PageID}",
            defaults: new { controller = "Strategic", action = "Index", PageID = UrlParameter.Optional },
            constraints: new { PageID = @"\d+" }
        );

            routes.MapRoute(
            name: "Economic",
            url: "economic/{PageID}",
            defaults: new { controller = "Economic", action = "Index", PageID = UrlParameter.Optional },
            constraints: new { PageID = @"\d+" }
            );

            routes.MapRoute(
            name: "Tail",
            url: "tail/{PageID}",
            defaults: new { controller = "Tail", action = "Index", PageID = UrlParameter.Optional },
            constraints: new { PageID = @"\d+" }
            );

            routes.MapRoute(
            name: "Card",
            url: "card/{PageID}",
            defaults: new { controller = "Card", action = "Index", PageID = UrlParameter.Optional },
            constraints: new { PageID = @"\d+" }
        );



            routes.MapRoute(
               name: "Game",
               url: "game/{GameIdAndSlug}",
               defaults: new { controller = "Game", action = "Index" }
               //constraints: new { GameID = @"\d+" }
            );

            routes.MapRoute(
               name: "Search",
               url: "Search/",
               defaults: new { controller = "Search", action = "Index" }
               //constraints: new { GameID = @"\d+" }
            );

            routes.MapRoute(
               name: "Licensing",
               url: "licence",
               defaults: new { controller = "Licence", action = "Index" }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Page", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
