using DataContextAline;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loja_Aline.Controllers
{
    public class HomeController : Controller
    {
       // private BD db = new BD();

        public ActionResult Index()
        {
            using (var db = new BD())
            {
                foreach (var dados in db.Dados.ToList())
                {
                    if (DateTime.Now > dados.data.AddDays(5) && dados.Status != "Pagamento realizado")
                    {
                        // Atualizar estoque
                        var m = 0;
                        foreach (var produto in db.Pedido.Include(p => p.Produtos).FirstOrDefault(p => p.IdPedido == dados.NumeroPedido).Produtos.ToList())
                        {
                            foreach (var medida in produto.Medida)
                            {
                                if (!medida.encomenda && medida.pedido_ == dados.NumeroPedido)
                                {
                                    m++;
                                    if (m == produto.Medida.Count(med => med.pedido_ == dados.NumeroPedido && !med.encomenda))
                                    {
                                        medida.Produto.Estoque = medida.Produto.Estoque + m;
                                        m = 0;
                                        db.Entry(medida.Produto).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }

                        //Atualizar status do produto atraves da atualização do estoque

                        foreach (var produto in db.Pedido.FirstOrDefault(p => p.IdPedido == dados.NumeroPedido).Produtos.ToList())
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

                    }
                } 
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}