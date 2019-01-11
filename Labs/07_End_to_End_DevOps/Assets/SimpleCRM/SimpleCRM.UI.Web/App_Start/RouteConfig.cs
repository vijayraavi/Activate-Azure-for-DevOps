using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SimpleCRM.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ProductList",
                url: "Product/List/{pageindex}/{pagesize}",
                defaults: new { controller = "Products", action = "Index" }
            );

            routes.MapRoute(
                name: "ProductLookup",
                url: "ProductLookup/{value}",
                defaults: new { controller = "Products", action = "ProductLookup", value = UrlParameter.Optional });


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
