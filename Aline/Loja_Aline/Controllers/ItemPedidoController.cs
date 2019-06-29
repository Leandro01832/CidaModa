using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataContextAline;
using business;
using Microsoft.AspNet.Identity;

namespace Loja_Aline.Controllers
{
    
    public class ItemPedidoController : Controller
    {
        private BD db = new BD();

        // GET: ItemPedido
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        public ActionResult Index()
        {
            var itemPedido = db.ItemPedido.Include(i => i.pedido).Include(i => i.produto);
            return View(itemPedido.ToList());
        }

        // GET: ItemPedido/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            var email = User.Identity.GetUserName();
            Cliente cliente = null;

            try
            {
                cliente = db.Cliente.First(e => e.UserName == email);
            }
            catch (Exception)
            {
                return RedirectToAction("Create", "Cliente");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemPedido itemPedido = db.ItemPedido.Find(id);

            if(itemPedido.pedido.Cliente.UserName != email && email != "cidaescolastico24@gmail.com")
            {
                return RedirectToAction("IndexCliente", "Cliente");
            }

            if (itemPedido == null)
            {
                return HttpNotFound();
            }
            return View(itemPedido);
        }

        // GET: ItemPedido/Create        
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        public ActionResult Create()
        {          

            ViewBag.produto_ = new SelectList(db.Pedido, "IdPrduto", "IdPrduto");
            ViewBag.pedido_ = new SelectList(db.Pedido, "IdPedido", "IdPedido");

            return View();
        }

        // POST: ItemPedido/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdItem,Quantidade,pedido_,produto_")] ItemPedido itemPedido)
        {
            if (ModelState.IsValid)
            {
                db.ItemPedido.Add(itemPedido);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.pedido_ = new SelectList(db.Pedido, "IdPedido", "ValorPedido", itemPedido.pedido_);
            ViewBag.produto_ = new SelectList(db.Produto, "IdPrduto", "Imagem", itemPedido.produto_);
            return View(itemPedido);
        }

        // GET: ItemPedido/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            var email = User.Identity.GetUserName();
            Cliente cliente = null;

            try
            {
                cliente = db.Cliente.First(e => e.UserName == email);
            }
            catch (Exception)
            {
                return RedirectToAction("Create", "Cliente");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemPedido itemPedido = db.ItemPedido.Find(id);

            if (itemPedido.pedido.Cliente.UserName != email && email != "cidaescolastico24@gmail.com")
            {
                return RedirectToAction("IndexCliente", "Cliente");
            }

            if (itemPedido == null)
            {
                return HttpNotFound();
            }

            ViewBag.pedido_ = db.ItemPedido.Find(id).pedido.IdPedido;
            ViewBag.produto_ = db.ItemPedido.Find(id).produto.IdPrduto;
            ViewBag.estoque = db.Produto.Find(itemPedido.produto.IdPrduto).Estoque;
            return View(itemPedido);
        }

        // POST: ItemPedido/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "IdItem,Quantidade,pedido_,produto_")] ItemPedido itemPedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itemPedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexCliente", "Cliente");
            }
            ViewBag.pedido_ = new SelectList(db.Pedido, "IdPedido", "ValorPedido", itemPedido.pedido_);
            ViewBag.produto_ = new SelectList(db.Produto, "IdPrduto", "Imagem", itemPedido.produto_);
            return View(itemPedido);
        }

        // GET: ItemPedido/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            var email = User.Identity.GetUserName();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemPedido itemPedido = db.ItemPedido.Find(id);
            if (itemPedido.pedido.Cliente.UserName != email && email != "cidaescolastico24@gmail.com")
            {
                return RedirectToAction("IndexCliente", "Cliente");
            }

            if (itemPedido == null)
            {
                return HttpNotFound();
            }
            return View(itemPedido);
        }

        // POST: ItemPedido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemPedido itemPedido = db.ItemPedido.Include(m => m.Medida).First(i => i.IdItem == id);
            foreach(var m in itemPedido.Medida.ToList())
            {
                db.Medida.Remove(m);
                db.SaveChanges();
            }
            db.ItemPedido.Remove(itemPedido);
            db.SaveChanges();
            return RedirectToAction("IndexCliente", "Cliente", null);
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
