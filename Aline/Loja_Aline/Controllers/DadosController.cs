﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataContextAline;
using PagSeguro;
using business;
using System.Diagnostics;
using PagSeguro.Properties;

namespace Loja_Aline.Controllers
{
    [Authorize(Users = "cidaescolastico24@gmail.com")]
    public class DadosController : Controller
    {
        private BD db = new BD();

        // GET: Dados
        public ActionResult Index()
        {
            return View(db.Dados.ToList());
        }

        // GET: Dados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dados dados = db.Dados.Find(id);
            if (dados == null)
            {
                return HttpNotFound();
            }
            return View(dados);
        }

        // GET: Dados/Create
        public ActionResult Create(int id)
        {
            Pedido pedido = db.Pedido.Find(id);
            
            return View(pedido);
        }

        // POST: Dados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DadosId,Nome,Email,DDD,NumeroTelefone,Valor")] Dados dados)
        {
            if (ModelState.IsValid)
            {
                db.Dados.Add(dados);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dados);
        }

        // GET: Dados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dados dados = db.Dados.Find(id);
            if (dados == null)
            {
                return HttpNotFound();
            }
            return View(dados);
        }

        // POST: Dados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DadosId,Nome,Email,DDD,NumeroTelefone,Valor")] Dados dados)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dados).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dados);
        }

        // GET: Dados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dados dados = db.Dados.Find(id);
            if (dados == null)
            {
                return HttpNotFound();
            }
            return View(dados);
        }

        // POST: Dados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dados dados = db.Dados.Find(id);
            db.Dados.Remove(dados);
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
