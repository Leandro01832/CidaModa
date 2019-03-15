using business;
using DataContextAline;
using Loja_Aline.ServiceReferenceCorreios;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loja_Aline.Controllers
{
    public class SalesController : Controller
    {
        private BD db = new BD();

        // GET: Sales
        [Authorize]
        public ActionResult Index(int id)
        {
            try
            {
                var email = User.Identity.GetUserName();
                var cliente = db.Cliente.First(e => e.UserName == email);
                var ped = cliente.Pedidos.First(p => p.IdPedido == id);
            }
            catch (Exception)
            {
                return RedirectToAction("IndexCliente", "Cliente");
            }
            Pedido pedido = db.Pedido.Find(id);
            return View(pedido);
        }

        public JsonResult CorreiosCalc(string cep)
        {

            string nCdEmpresa = string.Empty;
            string sDsSenha = string.Empty;
            string nCdServico = "41106";
            string sCepOrigem = "36774016";
            string sCepDestino = cep;
            string nVlPeso = 2.ToString();
            int nCdFormato = 1;
            decimal nVlComprimento = 40;
            decimal nVlAltura = 40;
            decimal nVlLargura = 40;
            decimal nVlDiametro = 0;
            string sCdMaoPropria = "N";
            decimal nVlValorDeclarado = 0;
            string sCdAvisoRecebimento = "N";
            string data = DateTime.Now.ToString("dd/MM/yyyy");

            CalcPrecoPrazoWSSoapClient wsCorreios = new CalcPrecoPrazoWSSoapClient();

            cResultado retornoCorreios = wsCorreios.CalcPrecoPrazoData(nCdEmpresa, sDsSenha,
                nCdServico, sCepOrigem, sCepDestino, nVlPeso, nCdFormato, nVlComprimento, nVlAltura,
                nVlLargura, nVlDiametro, sCdMaoPropria, nVlValorDeclarado, sCdAvisoRecebimento,
              data );

             
                string[] result = new string[3];
            
            result[0] = retornoCorreios.Servicos[0].Valor;
            result[1] = retornoCorreios.Servicos[0].PrazoEntrega;
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}