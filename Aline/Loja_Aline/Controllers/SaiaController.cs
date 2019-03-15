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
    public class SaiaController : Controller
    {
        private BD db = new BD();

        // GET: Saia
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Saia.ToList());
        }

        // GET: Saia/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saia saia = db.Saia.Find(id);
            if (saia == null)
            {
                return HttpNotFound();
            }
            return View(saia);
        }

        // GET: Saia/Create        
        public ActionResult Create()
        {
            return View();
        }

        // POST: Saia/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public ActionResult Create([Bind(Include = "IdPrduto,Comentario,Imagem,Preco,Medida,ImagemFile")] Saia saia)
        {
            if (ModelState.IsValid)
            {
                db.Saia.Add(saia);
                db.SaveChanges();

                if (saia.ImagemFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/Imagem";
                    var file = string.Format("{0}.jpg", saia.IdPrduto);

                    var response = FileHelpers.UploadPhoto(saia.ImagemFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        saia.Imagem = pic;
                    }
                }

                db.Entry(saia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(saia);
        }

        // GET: Saia/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saia saia = db.Saia.Find(id);
            if (saia == null)
            {
                return HttpNotFound();
            }
            return View(saia);
        }

        // POST: Saia/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPrduto,Comentario,Imagem,Preco,Medida,ImagemFile")] Saia saia)
        {
            if (ModelState.IsValid)
            {
                if (saia.ImagemFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/Imagem";
                    var file = string.Format("{0}.jpg", saia.IdPrduto);

                    var response = FileHelpers.UploadPhoto(saia.ImagemFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        saia.Imagem = pic;
                    }
                }

                db.Entry(saia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(saia);
        }

        // GET: Saia/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saia saia = db.Saia.Find(id);
            if (saia == null)
            {
                return HttpNotFound();
            }
            return View(saia);
        }

        // POST: Saia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Saia saia = db.Saia.Find(id);
            db.Saia.Remove(saia);
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
