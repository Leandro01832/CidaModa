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
using Microsoft.AspNet.Identity;

namespace Loja_Aline.Controllers
{
    
    public class VestidoController : Controller
    {
        private BD db = new BD();

        // GET: Vestido
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Vestido.ToList());
        }

        // GET: Vestido/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vestido vestido = db.Vestido.Find(id);
            if (vestido == null)
            {
                return HttpNotFound();
            }
            return View(vestido);
        }

        // GET: Vestido/Create
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vestido/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        public ActionResult Create([Bind(Include = "IdPrduto,Comentario,Imagem,Preco,Estoque,Medida,ImagemFile")] Vestido vestido)
        {
            if (ModelState.IsValid)
            {
                db.Vestido.Add(vestido);
                db.SaveChanges();

                if (vestido.ImagemFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/Imagem";
                    var file = string.Format("{0}.jpg", vestido.IdPrduto);

                    var response = FileHelpers.UploadPhoto(vestido.ImagemFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        vestido.Imagem = pic;
                    }
                }

                db.Entry(vestido).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(vestido);
        }

        // GET: Vestido/Edit/5
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vestido vestido = db.Vestido.Find(id);
            if (vestido == null)
            {
                return HttpNotFound();
            }
            return View(vestido);
        }

        // POST: Vestido/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        public ActionResult Edit([Bind(Include = "IdPrduto,Comentario,Imagem,Preco,Estoque,Medida,ImagemFile")] Vestido vestido)
        {
            if (ModelState.IsValid)
            {
                if (vestido.ImagemFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/Imagem";
                    var file = string.Format("{0}.jpg", vestido.IdPrduto);

                    var response = FileHelpers.UploadPhoto(vestido.ImagemFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        vestido.Imagem = pic;
                    }
                }
                
                db.Entry(vestido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vestido);
        }      

        // GET: Vestido/Delete/5
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vestido vestido = db.Vestido.Find(id);
            if (vestido == null)
            {
                return HttpNotFound();
            }
            return View(vestido);
        }

        // POST: Vestido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        public ActionResult DeleteConfirmed(int id)
        {
            Vestido vestido = db.Vestido.Find(id);
            db.Vestido.Remove(vestido);
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
