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
    [Authorize(Users ="cidaescolastico24@gmail.com")]
    public class ImagemController : Controller
    {
        private BD db = new BD();

        // GET: Imagem
        public ActionResult Index()
        {
            var imagem = db.Imagem.Include(i => i.Produto);
            return View(imagem.ToList());
        }

        // GET: Imagem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Imagem imagem = db.Imagem.Find(id);
            if (imagem == null)
            {
                return HttpNotFound();
            }
            return View(imagem);
        }

        // GET: Imagem/Create
        public ActionResult Create()
        {
            ViewBag.produtoImg_ = new SelectList(db.Produto, "IdPrduto", "IdPrduto");
            return View();
        }

        // POST: Imagem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdImagem,ImagemProduto,produtoImg_,ImagemProdutoFile")] Imagem imagem)
        {
            if (ModelState.IsValid)
            {
                db.Imagem.Add(imagem);
                db.SaveChanges();

                if (imagem.ImagemProdutoFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/ListaImagens";
                    var file = string.Format("{0}.jpg", imagem.IdImagem);

                    var response = FileHelpers.UploadPhoto(imagem.ImagemProdutoFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        imagem.ImagemProduto = pic;
                    }
                }

                db.Entry(imagem).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.produtoImg_ = new SelectList(db.Produto, "IdPrduto", "IdPrduto", imagem.produtoImg_);
            return View(imagem);
        }

        // GET: Imagem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Imagem imagem = db.Imagem.Find(id);
            if (imagem == null)
            {
                return HttpNotFound();
            }
            ViewBag.produtoImg_ = new SelectList(db.Produto, "IdPrduto", "IdPrduto", imagem.produtoImg_);
            return View(imagem);
        }

        // POST: Imagem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdImagem,ImagemProduto,produtoImg_,ImagemProdutoFile")] Imagem imagem)
        {
            if (ModelState.IsValid)
            {
                if (imagem.ImagemProdutoFile != null)
                {
                    var pic = string.Empty;
                    var folder = "~/Content/ListaImagens";
                    var file = string.Format("{0}.jpg", imagem.IdImagem);

                    var response = FileHelpers.UploadPhoto(imagem.ImagemProdutoFile, folder, file);
                    if (response)
                    {
                        pic = string.Format("{0}/{1}", folder, file);
                        imagem.ImagemProduto = pic;
                    }
                }

                db.Entry(imagem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.produtoImg_ = new SelectList(db.Produto, "IdPrduto", "IdPrduto", imagem.produtoImg_);
            return View(imagem);
        }

        // GET: Imagem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Imagem imagem = db.Imagem.Find(id);
            if (imagem == null)
            {
                return HttpNotFound();
            }
            return View(imagem);
        }

        // POST: Imagem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Imagem imagem = db.Imagem.Find(id);
            db.Imagem.Remove(imagem);
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
