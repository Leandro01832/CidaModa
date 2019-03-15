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

namespace Loja_Aline.Controllers
{
    
    public class ClienteController : Controller
    {
        private BD db = new BD();

        // GET: Cliente
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        public ActionResult Index()
        {
            var cliente = db.Cliente.Include(c => c.Telefone).Include(c => c.Pedidos);
            
            return View(cliente.ToList());
        }

        [Authorize]
        public ActionResult IndexCliente()
        {
            Cliente cliente = null;
            var email = User.Identity.GetUserName();
            try
            {
                cliente = db.Cliente.First(c => c.UserName == email);
            }
            catch (Exception)
            {
                return RedirectToAction("Create", "Cliente");
            }
            Cliente cli = cliente;
            return View(cli);
        }

        // GET: Cliente/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Cliente/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.email = User.Identity.GetUserName();
            ViewBag.IdCliente = new SelectList(db.Telefone, "IdTelefone", "Celular");
            return View();
        }

        // POST: Cliente/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "IdCliente,FirstName,LastName,UserName,Cpf,Telefone")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Cliente.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("IndexCliente", "Cliente");
            }

            ViewBag.IdCliente = new SelectList(db.Endereco, "IdEndereco", "Estado", cliente.IdCliente);
            ViewBag.IdCliente = new SelectList(db.Telefone, "IdTelefone", "Celular", cliente.IdCliente);
            return View(cliente);
        }

        // GET: Cliente/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCliente = new SelectList(db.Endereco, "IdEndereco", "Estado", cliente.IdCliente);
            ViewBag.IdCliente = new SelectList(db.Telefone, "IdTelefone", "Celular", cliente.IdCliente);
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "IdCliente,Nome,FirstName,LastName,UserName,Cpf,Telefone")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCliente = new SelectList(db.Endereco, "IdEndereco", "Estado", cliente.IdCliente);
            ViewBag.IdCliente = new SelectList(db.Telefone, "IdTelefone", "Celular", cliente.IdCliente);
            return View(cliente);
        }

        public string Link(int id)
        {
            Dados dados = db.Dados.First(d => d.NumeroPedido == id);
            var link = dados.stringConexao;

            return link;
        }

        // GET: Cliente/Delete/5
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "cidaescolastico24@gmail.com")]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Cliente.Find(id);
            db.Cliente.Remove(cliente);
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
