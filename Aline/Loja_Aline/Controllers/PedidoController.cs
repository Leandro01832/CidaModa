using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Loja_Aline.Models;
using business;
using Microsoft.AspNet.Identity;
using DataContextAline;
using System.Threading.Tasks;

namespace Loja_Aline.Controllers
{
    public class PedidoController : Controller
    {
        private BD db = new BD();

        // GET: Pedido
        public ActionResult Index()
        {
            var pedidoes = db.Pedido.Include(p => p.Endereco);
            return View(pedidoes.ToList());
        }

        // GET: Pedido/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // GET: Pedido/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.IdPedido = new SelectList(db.Endereco, "IdEndereco", "Estado");
            return View();
        }

        // POST: Pedido/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                var email = User.Identity.GetUserName();
                var cli = await db.Cliente.FirstAsync(e => e.UserName == email);
                pedido.Cliente = cli;
                db.Pedido.Add(pedido);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdPedido = new SelectList(db.Endereco, "IdEndereco", "Estado", pedido.IdPedido);
            return View(pedido);
        }

        // GET: Pedido/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdPedido = new SelectList(db.Endereco, "IdEndereco", "Estado", pedido.IdPedido);
            return View(pedido);
        }

        // POST: Pedido/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdPedido = new SelectList(db.Endereco, "IdEndereco", "Estado", pedido.IdPedido);
            return View(pedido);
        }

        // GET: Pedido/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: Pedido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pedido pedido = db.Pedido.Find(id);
            db.Pedido.Remove(pedido);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
