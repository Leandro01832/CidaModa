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
    [Authorize(Users = "cidaescolastico24@gmail.com")]
    public class ProdutoController : Controller
    {
        private BD db = new BD();

        // GET: Produto
        public ActionResult Index()
        {
            return View(db.Produto.ToList());
        }

        // GET: Produto/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // GET: Produto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Produto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPrduto,Estoque")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                db.Produto.Add(produto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(produto);
        }

        // GET: Produto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // POST: Produto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPrduto")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(produto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(produto);
        }

        public ActionResult AdicionarEstoque(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // POST: Produto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdicionarEstoque([Bind(Include = "IdPrduto,Estoque,Imagem,Preco")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                produto.Estoque += int.Parse(Request["adicionar"]);

                //atualizar status dos produtos

                foreach(var pedido in db.Pedido.ToList())
                {
                    if(pedido.Status == "Nao finalizado")
                    {
                        foreach(var medida in db.Medida.ToList())
                        {
                            if(medida.Item.produto_ == produto.IdPrduto)
                            {
                                if(medida.Item.produto.Estoque < medida.Item.Medida.Count(m => m.Item.pedido_ == pedido.IdPedido))
                                {
                                    for(int i = 0; i < medida.Item.produto.Estoque; i++)
                                    {
                                        try
                                        {
                                            var medid = db.Medida.First(m => m.Item.produto_ == produto.IdPrduto && m.encomenda == true && m.Item.produto.Estoque < medida.Item.Medida.Count(medi => medi.Item.pedido_ == pedido.IdPedido));
                                            medid.encomenda = false;
                                            db.Entry(medid).State = EntityState.Modified;
                                            db.SaveChanges();
                                        }
                                        catch (Exception)
                                        {
                                            throw;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                db.Entry(produto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(produto);
        }



        // GET: Produto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = db.Produto.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // POST: Produto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Produto produto = db.Produto.Find(id);
            db.Produto.Remove(produto);
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
