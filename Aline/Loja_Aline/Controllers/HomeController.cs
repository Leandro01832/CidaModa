using DataContextAline;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loja_Aline.Controllers
{
    public class HomeController : Controller
    {
       // private BD db = new BD();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Descrição.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "COntatos.";

            return View();
        }
    }
}