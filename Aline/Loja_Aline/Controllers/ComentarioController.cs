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
    [Authorize(Users = "cidaescolastico24@gmail.com")]
    public class ComentarioController : Controller
    {
        private BD db = new BD();

        // GET: Comentario
        public ActionResult Index()
        {
            var comentario = db.Comentario.Include(c => c.Produto);
            return View(comentario.ToList());
        }

        // GET: Comentario/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comentario comentario = db.Comentario.Find(id);
            if (comentario == null)
            {
                return HttpNotFound();
            }
            return View(comentario);
        }

        // GET: Comentario/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            ViewBag.produto_2 = new SelectList(db.Produto.Where(p => p.IdPrduto == id), "IdPrduto", "IdPrduto");
            return View();
        }

        // POST: Comentario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "IdComentario,Comentar,Email,produto_2")] Comentario comentario)
        {
            if (ModelState.IsValid)
            {
                var email = User.Identity.GetUserName();
                comentario.Email = email;
                db.Comentario.Add(comentario);
                db.SaveChanges();
                return RedirectToAction("Details", "Produto", new { id = comentario.produto_2 });
            }

            ViewBag.produto_2 = new SelectList(db.Produto, "IdPrduto", "IdPrduto", comentario.produto_2);
            return View(comentario);
        }

        // GET: Comentario/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comentario comentario = db.Comentario.Find(id);
            if (comentario == null)
            {
                return HttpNotFound();
            }
            ViewBag.produto_2 = new SelectList(db.Produto, "IdPrduto", "IdPrduto", comentario.produto_2);
            return View(comentario);
        }

        // POST: Comentario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdComentario,Comentar,produto_2")] Comentario comentario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comentario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.produto_2 = new SelectList(db.Produto, "IdPrduto", "IdPrduto", comentario.produto_2);
            return View(comentario);
        }

        public string UsuarioComentario(string email)
        {
            var nome = "";
            try
            {
                 nome = db.Cliente.First(c => c.UserName == email).FirstName + " " + db.Cliente.First(c => c.UserName == email).LastName;
            }            

            catch (Exception)
            {
                return email;
            }

            return nome;


        }

        // GET: Comentario/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comentario comentario = db.Comentario.Find(id);
            if (comentario == null)
            {
                return HttpNotFound();
            }
            return View(comentario);
        }

        // POST: Comentario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comentario comentario = db.Comentario.Find(id);
            db.Comentario.Remove(comentario);
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
