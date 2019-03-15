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
using Ecommerce.Classes;

namespace Loja_Aline.Controllers
{
    [Authorize(Users = "cidaescolastico24@gmail.com")]
    public class DaminhaController : Controller
    {
        private BD db = new BD();

        // GET: Daminha
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Daminha.ToList());
        }

        // GET: Daminha/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Daminha daminha = db.Daminha.Find(id);
            if (daminha == null)
            {
                return HttpNotFound();
            }
            return View(daminha);
        }

        // GET: Daminha/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Daminha/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPrduto,Comentario,Imagem,Preco,Medida,ImagemFile")] Daminha daminha)
        {
            if (ModelState.IsValid)
            {
                db.Daminha.Add(daminha);
                db.SaveChanges();

                if (daminha.ImagemFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/Imagem";
                    var file = string.Format("{0}.jpg", daminha.IdPrduto);

                    var response = FileHelpers.UploadPhoto(daminha.ImagemFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        daminha.Imagem = pic;
                    }
                }

                db.Entry(daminha).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(daminha);
        }

        // GET: Daminha/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Daminha daminha = db.Daminha.Find(id);
            if (daminha == null)
            {
                return HttpNotFound();
            }
            return View(daminha);
        }

        // POST: Daminha/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPrduto,Comentario,Imagem,Preco,Medida,ImagemFile")] Daminha daminha)
        {
            if (ModelState.IsValid)
            {
                if (daminha.ImagemFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/Imagem";
                    var file = string.Format("{0}.jpg", daminha.IdPrduto);

                    var response = FileHelpers.UploadPhoto(daminha.ImagemFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        daminha.Imagem = pic;
                    }
                }

                db.Entry(daminha).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(daminha);
        }

        // GET: Daminha/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Daminha daminha = db.Daminha.Find(id);
            if (daminha == null)
            {
                return HttpNotFound();
            }
            return View(daminha);
        }

        // POST: Daminha/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Daminha daminha = db.Daminha.Find(id);
            db.Daminha.Remove(daminha);
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
