using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Loja_Aline
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Gerenciamento_vestido_adicionar",
                url: "Gerenciamento/Gereciamento_vestido_adicionar/{id}",
                defaults: new { controller = "Produto", action = "AdicionarEstoque", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Gerenciamento_vestido_criar",
                url: "Gerenciamento/Gereciamento_vestido_criar",
                defaults: new { controller = "Vestido", action = "Create" }
            );

            routes.MapRoute(
                name: "Gerenciamento_vestido_delete",
                url: "Gerenciamento/Gereciamento_vestido_deletar/{id}",
                defaults: new { controller = "Vestido", action = "Delete", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Gerenciamento_vestido_details",
                url: "Gerenciamento/Gereciamento_vestido_detalhes/{id}",
                defaults: new { controller = "Vestido", action = "Details", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Gerenciamento_vestido_edit",
                url: "Gerenciamento/Gereciamento_vestido_editar/{id}",
                defaults: new { controller = "Vestido", action = "Edit", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
