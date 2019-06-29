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

namespace Loja_Aline.Controllers
{
    
    public class EnderecoController : Controller
    {
        private BD db = new BD();

        // GET: Endereco
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        public ActionResult Index()
        {
            var endereco = db.Endereco.Include(e => e.Pedido);
            return View(endereco.ToList());
        }

        // GET: Endereco/Details/5
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Endereco endereco = db.Endereco.Find(id);
            if (endereco == null)
            {
                return HttpNotFound();
            }
            return View(endereco);
        }

        // GET: Endereco/Create
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        public ActionResult Create()
        {
            ViewBag.IdEndereco = new SelectList(db.Pedido, "IdPedido", "IdPedido");
            return View();
        }

        // POST: Endereco/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        public ActionResult Create([Bind(Include = "IdEndereco,Estado,Cidade,Bairro,Rua,Numero,Cep")] Endereco endereco)
        {
            if (ModelState.IsValid)
            {
                db.Endereco.Add(endereco);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdEndereco = new SelectList(db.Pedido, "IdPedido", "IdPedido", endereco.IdEndereco);
            return View(endereco);
        }

        // GET: Endereco/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Endereco endereco = db.Endereco.Find(id);
            if (endereco == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdEndereco = new SelectList(db.Pedido, "IdPedido", "IdPedido", endereco.IdEndereco);
            return View(endereco);
        }

        // POST: Endereco/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "IdEndereco,Estado,Cidade,Bairro,Rua,Numero,Cep")] Endereco endereco)
        {
            if (ModelState.IsValid)
            {
                endereco.Estado = Request["Estado"];
                endereco.Bairro = Request["Bairro"];
                endereco.Cep = long.Parse(Request["Cep"].Replace("-", ""));
                endereco.Cidade = Request["Cidade"];
                try
                {
                    endereco.Numero = long.Parse(Request["Numero"]);
                }
                catch (Exception)
                {

                    throw;
                }
                endereco.Rua = Request["Rua"];

                db.Entry(endereco).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdEndereco = new SelectList(db.Pedido, "IdPedido", "IdPedido", endereco.IdEndereco);
            return View(endereco);
        }

        // GET: Endereco/Delete/5
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Endereco endereco = db.Endereco.Find(id);
            if (endereco == null)
            {
                return HttpNotFound();
            }
            return View(endereco);
        }

        // POST: Endereco/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        public ActionResult DeleteConfirmed(int id)
        {
            Endereco endereco = db.Endereco.Find(id);
            db.Endereco.Remove(endereco);
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
