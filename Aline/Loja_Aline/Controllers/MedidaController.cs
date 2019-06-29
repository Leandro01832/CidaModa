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
    
    public class MedidaController : Controller
    {
        private BD db = new BD();

        // GET: Medida
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        public ActionResult Index()
        {
            var medida = db.Medida.Include(m => m.Item);
            return View(medida.ToList());
        }

        // GET: Medida/Details/5
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medida medida = db.Medida.Find(id);
            if (medida == null)
            {
                return HttpNotFound();
            }
            return View(medida);
        }

        // GET: Medida/Create
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        public ActionResult Create()
        {
            ViewBag.item_ = new SelectList(db.ItemPedido, "IdItem", "IdItem");
            return View();
        }

        // POST: Medida/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        public ActionResult Create([Bind(Include = "IdMedida,encomenda,Idade,Quadril,Ombro,Torax,Altura,Cintura,Comprimento,item_")] Medida medida)
        {
            if (ModelState.IsValid)
            {
                db.Medida.Add(medida);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.item_ = new SelectList(db.ItemPedido, "IdItem", "IdItem", medida.itemPedido_);
            return View(medida);
        }

        // GET: Medida/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            var email = User.Identity.GetUserName();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medida medida = db.Medida.Find(id);
            
            if(medida.Item.pedido.Cliente.UserName != email && email != "cidaescolastico24@gmail.com")
            {
                return RedirectToAction("IndexCliente", "Cliente");
            }

            if (medida == null)
            {
                return HttpNotFound();
            }
            ViewBag.item_ = new SelectList(db.ItemPedido, "IdItem", "IdItem", medida.itemPedido_);
            return View(medida);
        }

        // POST: Medida/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "IdMedida,encomenda,Idade,Quadril,Ombro,Torax,Altura,Cintura,Comprimento,itemPedido_")] Medida medida)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medida).State = EntityState.Modified;                
                db.SaveChanges();
                return RedirectToAction("IndexCliente", "Cliente", null);
            }
            ViewBag.itemPedido_ = new SelectList(db.ItemPedido, "IdItem", "IdItem", medida.itemPedido_);
            return View(medida);
        }

        // GET: Medida/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            var email = User.Identity.GetUserName();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medida medida = db.Medida.Find(id);
            if (medida.Item.pedido.Cliente.UserName != email && email != "cidaescolastico24@gmail.com")
            {
                return RedirectToAction("IndexCliente", "Cliente");
            }

            if (medida == null)
            {
                return HttpNotFound();
            }
            return View(medida);
        }

        // POST: Medida/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Medida medida = db.Medida.Find(id);
            db.Medida.Remove(medida);
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
