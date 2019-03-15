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
        public ActionResult Index()
        {
            var medida = db.Medida.Include(m => m.Produto);
            return View(medida.ToList());
        }

        // GET: Medida/Details/5
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
        [Authorize]
        public ActionResult Create(int id)
        {
            ViewBag.produto_ = new SelectList(db.Produto.Where(p => p.IdPrduto == id), "IdPrduto", "IdPrduto");
            return View();
        }

        // POST: Medida/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "IdMedida,encomenda,Idade,Quadril,Ombro,Torax,Altura,Cintura,Comprimento,produto_")] Medida medida)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var email = User.Identity.GetUserName();
                    var quantidade = 0;
                    var cliente = db.Cliente.First(e => e.UserName == email);

                    db.Medida.Add(medida);
                    db.SaveChanges();

                    if (cliente.Pedidos.Count == 0 || cliente.Pedidos.Last().Status == "Finalizado")
                    {
                        cliente.Pedidos.Add(new Pedido { Datapedido = DateTime.Now, Endereco = new Endereco { }, Status = "Nao finalizado" });
                        cliente.Pedidos.Last().Produtos = new List<Produto>();
                    }                    

                    foreach(var produto in cliente.Pedidos.Last().Produtos)
                    {
                        quantidade += produto.Medida.Count(m => m.pedido_ == cliente.Pedidos.Last().IdPedido);

                        if(quantidade >= 10)
                        {
                            cliente.Pedidos.Add(new Pedido { Datapedido = DateTime.Now, Endereco = new Endereco { }, Status = "Nao finalizado" });
                            cliente.Pedidos.Last().Produtos = new List<Produto>();
                        }
                    }

                    cliente.Pedidos.Last().Produtos.Add(db.Produto.First(p => p.IdPrduto == medida.produto_));
                    if(db.Produto.First(p => p.IdPrduto == medida.produto_).Estoque < db.Produto.First(p => p.IdPrduto == medida.produto_).Medida.Count(m => m.pedido_ == cliente.Pedidos.Last().IdPedido))
                    {
                        medida.encomenda = true;
                        medida.pedido_ = cliente.Pedidos.Last().IdPedido;
                        db.Entry(medida).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        medida.pedido_ = cliente.Pedidos.Last().IdPedido;
                        db.Entry(medida).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    

                    return RedirectToAction("IndexCliente", "Cliente");
                }
                catch (Exception)
                {
                    ViewBag.error = "Erro de identidade";
                    return RedirectToAction("Create", "Cliente");
                }
            }

            ViewBag.produto_ = new SelectList(db.Produto, "IdPrduto", "IdPrduto", medida.produto_);
            return View(medida);
        }

        // GET: Medida/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            var email = User.Identity.GetUserName();
            if (email != "cidaescolastico24@gmail.com")
            {
                try
                {
                    var cliente = db.Cliente.First(e => e.UserName == email);
                    var ped = cliente.Pedidos.First(p => p.Medidas.First(m => m.IdMedida == id).IdMedida == id);
                }
                catch (Exception)
                {
                    return RedirectToAction("IndexCliente", "Cliente");
                }
            }
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medida medida = db.Medida.Find(id);
            if (medida == null)
            {
                return HttpNotFound();
            }
            ViewBag.produto_ = new SelectList(db.Produto, "IdPrduto", "IdPrduto", medida.produto_);
            return View(medida);
        }

        // POST: Medida/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "IdMedida,Idade,Quadril,Ombro,Torax,Altura,Cintura,Comprimento,produto_")] Medida medida)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medida).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexCliente", "Cliente", null);
            }
            ViewBag.produto_ = new SelectList(db.Produto, "IdPrduto", "IdPrduto", medida.produto_);
            return View(medida);
        }

        // GET: Medida/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            var email = User.Identity.GetUserName();
            if (email != "cidaescolastico24@gmail.com")
            {
                try
                {
                    var cliente = db.Cliente.First(e => e.UserName == email);
                    var ped = cliente.Pedidos.First(p => p.Medidas.First(m => m.IdMedida == id).IdMedida == id);
                }
                catch (Exception)
                {
                    return RedirectToAction("IndexCliente", "Cliente");
                }
            }
           
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
