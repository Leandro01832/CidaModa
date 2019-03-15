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
using PagSeguro;
using System.Diagnostics;

namespace Loja_Aline.Controllers
{
    public class PedidoController : Controller
    {
        private BD db = new BD();

        // GET: Pedido
        public ActionResult Index()
        {
            return View(db.Pedido.ToList());
        }     

        // GET: Pedido/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            var email = User.Identity.GetUserName();
            if (email != "cidaescolastico24@gmail.com")
            {
                try
                {
                    var cliente = db.Cliente.First(e => e.UserName == email);
                    var ped = cliente.Pedidos.First(p => p.IdPedido == id);
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
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // GET: Pedido/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pedido/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPedido,Datapedido")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Pedido.Add(pedido);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pedido);
        }

        // GET: Pedido/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.First(p => p.IdPedido == id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: Pedido/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "IdPedido,Datapedido,Endereco,Status,ValorPedido,Cliente")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                pedido.Status = "Finalizado";
                pedido.ValorPedido = Request["precototal"].Replace(".", ",");
                pedido.Endereco.Estado = Request["Estado"];
                pedido.Endereco.Bairro = Request["Bairro"];
                pedido.Endereco.Cep = long.Parse(Request["Cep"].ToString().Replace("-", ""));
                pedido.Endereco.Cidade = Request["Cidade"];
                try
                {
                    pedido.Endereco.Numero = long.Parse(Request["Numero"]);
                }
                catch (Exception)
                {
                    return View(pedido);
                }
                pedido.Endereco.Rua = Request["Rua"];
                pedido.Endereco.ValorFrete = Request["ValorFrete"];

                db.Entry(pedido).State = EntityState.Modified;
                db.Entry(pedido.Endereco).State = EntityState.Modified;
                
                Dados dados = new Dados();
                var email = User.Identity.GetUserName();
                dados.Email = email;
                dados.Nome = db.Cliente.First(c => c.UserName == email).FirstName + " " + db.Cliente.First(c => c.UserName == email).LastName;
                dados.DDD = db.Cliente.Include(c => c.Telefone).First(c => c.IdCliente == pedido.Cliente.IdCliente).Telefone.DDD_Celular;
                dados.NumeroTelefone = db.Cliente.Include(c => c.Telefone).First(c => c.IdCliente == pedido.Cliente.IdCliente).Telefone.Celular;
                dados.NumeroPedido = pedido.IdPedido;
                
                dados.Valor = pedido.ValorPedido;

                dados.MeuEmail = "leandroleanleo@gmail.com";
                dados.MeuToken = "A37C25015C8446EE938DBB58D92F6BCF";
                dados.TituloPagamento = "Pagamento";

                dados = sPagSeguro.GerarPagamento(dados);
                
                db.Dados.Add(dados);
                db.SaveChanges();

                //Atualizar estoque

                var m = 0;
                foreach (var produto in db.Pedido.Include(p => p.Produtos).First(p => p.IdPedido == pedido.IdPedido).Produtos)
                {                    
                    foreach (var medida in produto.Medida)
                    {                        
                        if (!medida.encomenda && medida.pedido_ == pedido.IdPedido)
                        {
                            m++;
                            if (m == produto.Medida.Count(med => med.pedido_ == pedido.IdPedido))
                            {
                                medida.Produto.Estoque = medida.Produto.Estoque - m;
                                m = 0;
                                db.Entry(medida.Produto).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }                    
                }

                //Atualizar status do produto atraves da atualização do estoque

                foreach (var produto in pedido.Produtos)
                {
                    foreach (var OutrosPedidos in produto.Pedido)
                    {
                        if (OutrosPedidos.Status == "Nao finalizado")
                        {
                            foreach (var med in OutrosPedidos.Medidas)
                            {
                                if (med.Produto.Estoque < med.Produto.Medida.Count(medi => medi.pedido_ == OutrosPedidos.IdPedido))
                                {
                                    for (int i = 0; i < med.Produto.Estoque; i++)
                                    {
                                        try
                                        {
                                            var medid = db.Medida.First(me => me.produto_ == produto.IdPrduto && me.encomenda == true);
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

                return RedirectToAction("IndexCliente", "Cliente");
            }
            return View(pedido);
        }

        // GET: Pedido/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: Pedido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pedido pedido = db.Pedido.Find(id);
            db.Pedido.Remove(pedido);
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
