using DataContextAline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loja_Aline.Controllers
{
    [Authorize(Users ="cidaescolastico24@gmail.com")]
    public class GerenciamentoController : Controller
    {
        private BD db = new BD();

        // GET: Gerenciamento
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Gerenciamento_vestido()
        {
            return View(db.Vestido.ToList());
        }
    }
}