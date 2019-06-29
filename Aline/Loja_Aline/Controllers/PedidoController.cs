using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DataContextAline;
using business;
using Microsoft.AspNet.Identity;
using PagSeguro;
using System.Collections.Generic;

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
        [Authorize]
        public ActionResult Create(int id)
        {
            var email = User.Identity.GetUserName();
            Cliente cliente = null;

            try
            {
                cliente = db.Cliente.First(e => e.UserName == email);
            }
            catch (Exception)
            {
                return RedirectToAction("Create", "Cliente");
            }

            if (cliente.Pedidos.Count == 0 || cliente.Pedidos.Last().Status == "Finalizado")
            {
                cliente.Pedidos.Add(new Pedido { Datapedido = DateTime.Now, Endereco = new Endereco { }, Status = "Nao finalizado" });
                cliente.Pedidos.Last().Itens = new List<ItemPedido>();
            }

            db.SaveChanges();

            ViewBag.produto_ = db.Produto.Find(id).IdPrduto;
            ViewBag.estoque = db.Produto.Find(id).Estoque;
            ViewBag.pedido_ = new SelectList(db.Pedido, "IdPedido", cliente.Pedidos.Last().IdPedido.ToString());

            return View(cliente.Pedidos.Last());
        }

        // POST: Pedido/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "IdPedido,Datapedido")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                var Itens = db.Pedido.First(m => m.IdPedido == pedido.IdPedido).Itens;

                int id = int.Parse(Request["roupa"]);
                ItemPedido item = new ItemPedido();
                item.produto_ = id;
                item.pedido_ = pedido.IdPedido;
                item.Quantidade = int.Parse(Request["quantidade"]);

                foreach(var it in Itens)
                {
                    if (it.produto.IdPrduto == id)
                    {
                        return RedirectToAction("Create", "Pedido", new { error2 = "Este produto ja foi adicionado ao seu carrinho. Se quiser remove-lo acesse o link area do cliente e altere seu pedido!!!" });
                    }
                }

                for (int i = 1; i <= item.Quantidade; i++)
                {
                    try
                    {
                        var Idade = int.Parse(Request["idade" + i.ToString()]);
                        var Altura = int.Parse(Request["altura" + i.ToString()]);
                        var Ombro = int.Parse(Request["ombro" + i.ToString()]);
                        var Quadril = int.Parse(Request["quadril" + i.ToString()]);
                        var Torax = int.Parse(Request["torax" + i.ToString()]);
                        var Cintura = int.Parse(Request["cintura" + i.ToString()]);
                        var Comprimento = int.Parse(Request["comprimento" + i.ToString()]);
                    }
                    catch (Exception)
                    {
                        return RedirectToAction("Create", "Pedido", new { error = "Informe todas as medidas!!!" });
                    }
                }
                    db.ItemPedido.Add(item);
                //  item.pedido_ = ped.IdPedido;
                if (pedido.Itens == null)
                {
                    pedido.Itens = new List<ItemPedido>();
                }
                if (item.Quantidade > 99 || item.Quantidade < 1)
                {
                    return RedirectToAction("Create", "Pedido", new { error = "A quantidade deve estar entre 1 e 99!!!" });
                }

                Itens.Add(item);
                pedido.Itens.AddRange(Itens);
                db.SaveChanges();

                item.Medida = new List<Medida>();

                for (int i = 1; i <= item.Quantidade; i++)
                {                    
                    Medida med = new Medida();

                    var textoEncomenda = Request["encomenda" + i.ToString()];
                    if (textoEncomenda == "sim")
                    {
                        med.encomenda = true;
                    }
                    else
                    {
                        med.encomenda = false;
                    }


                    med.itemPedido_ = item.IdItem;
                    med.Idade = int.Parse(Request["idade" + i.ToString()]);
                    med.Altura = int.Parse(Request["altura" + i.ToString()]);
                    med.Ombro = int.Parse(Request["ombro" + i.ToString()]);
                    med.Quadril = int.Parse(Request["quadril" + i.ToString()]);
                    med.Torax = int.Parse(Request["torax" + i.ToString()]);
                    med.Cintura = int.Parse(Request["cintura" + i.ToString()]);
                    med.Comprimento = int.Parse(Request["comprimento" + i.ToString()]);

                    item.Medida.Add(med);
                    db.SaveChanges();
                }

                                
                
                return RedirectToAction("IndexCliente", "Cliente");
            }

            return View(pedido);
        }

        // GET: Pedido/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            var email = User.Identity.GetUserName();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.First(p => p.IdPedido == id);

            if (pedido.Cliente.UserName != email && email != "cidaescolastico24@gmail.com")
            {
                return RedirectToAction("IndexCliente", "Cliente");
            }

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
                var texto = db.Destino.First(d => d.Ativacao == true).Estados;

                if(!texto.Contains(Request["Estado"]))
                {
                    return RedirectToAction("Edit", "Pedido", new { id = pedido.IdPedido, error1 = "Desculpe. Nós não ainda estamos atendendo sua região" });
                } 


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
                    return RedirectToAction("Edit", "Pedido", new { id = pedido.IdPedido, error2 = "Informe o numero da sua casa" });
                }
                pedido.Endereco.Rua = Request["Rua"];
                pedido.Endereco.ValorFrete = Request["ValorFrete"];

                db.Entry(pedido).State = EntityState.Modified;
                db.Entry(pedido.Endereco).State = EntityState.Modified;
                db.SaveChanges();

                Dados dados = new Dados();
                var email = User.Identity.GetUserName();
                dados.Email = email;
                dados.data = DateTime.Now;
                dados.Nome = db.Cliente.First(c => c.UserName == email).FirstName + " " + db.Cliente.First(c => c.UserName == email).LastName;
                dados.DDD = db.Cliente.Include(c => c.Telefone).First(c => c.IdCliente == pedido.Cliente.IdCliente).Telefone.DDD_Celular;
                dados.NumeroTelefone = db.Cliente.Include(c => c.Telefone).First(c => c.IdCliente == pedido.Cliente.IdCliente).Telefone.Celular;
                dados.NumeroPedido = pedido.IdPedido;                
                
                dados.Valor = pedido.ValorPedido;

                dados.MeuEmail = "leandro91luis@gmail.com";
                dados.MeuToken = "ffd81aee-ddac-42c9-b6ff-d1cc90f8eab42ee5fca7432694c690d3819c29f774714bd9-609f-4b91-9704-6df2d8c0e2e4";
                dados.TituloPagamento = "Pagamento";
                dados.Referencia = "Roupas";

                dados = sPagSeguro.GerarPagamento(dados);
                
                db.Dados.Add(dados);
                db.SaveChanges();

                //Atualizar estoque

                var m = 0;
                foreach (var item in db.Pedido.Include(p => p.Itens).First(p => p.IdPedido == pedido.IdPedido).Itens)
                {                    
                    foreach (var medida in item.Medida)
                    {                        
                        if (!medida.encomenda && medida.Item.pedido.IdPedido == pedido.IdPedido)
                        {
                            m++;
                            if (m == item.Medida.Count(med => med.Item.pedido.IdPedido == pedido.IdPedido && !med.encomenda))
                            {
                                medida.Item.produto.Estoque = medida.Item.produto.Estoque - m;
                                m = 0;
                                db.Entry(medida.Item).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }                    
                }

                //Atualizar status das medidas dos itens de acordo com a atualizacao do estoque
                
                    foreach (var OutrosPedidos in db.Pedido.ToList())
                    {
                        if (OutrosPedidos.Status == "Nao finalizado")
                        {
                            foreach (var item in OutrosPedidos.Itens)
                            {
                                // variavel que guarda quantidade de medidas que tem status compra
                                var quantidade_compra = item.Medida.Count(q => q.encomenda == false);
                                //variavel atualizada
                                var quantidade_Atualizada = quantidade_compra - item.produto.Estoque;
                                var i = 0;

                                foreach (var medida in item.Medida)
                                {
                                        i++;
                                        medida.encomenda = true;
                                        db.Entry(medida).State = EntityState.Modified;
                                        db.SaveChanges();

                                        if (i == item.Medida.Count)
                                        {
                                            for (int a = 0; a <= quantidade_Atualizada; a++)
                                            {
                                                medida.encomenda = false;
                                                db.Entry(medida).State = EntityState.Modified;
                                                db.SaveChanges();
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
        [Authorize]
        public ActionResult Delete(int? id)
        {
            var email = User.Identity.GetUserName();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido.Cliente.UserName != email && email != "cidaescolastico24@gmail.com")
            {
                return RedirectToAction("IndexCliente", "Cliente");
            }

            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: Pedido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
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
