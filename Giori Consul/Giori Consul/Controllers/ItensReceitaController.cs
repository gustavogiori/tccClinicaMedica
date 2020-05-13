using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Giori_Consul.Models;
using Giori_Consul.Security;

namespace Giori_Consul.Controllers
{
    [CustomAuthorize(RoleEnum.Admin, RoleEnum.Medico)]
    public class ItensReceitaController : Controller
    {
        private SqlDbContext db = new SqlDbContext();

        // GET: ItensReceita
        public ActionResult Index()
        {
            var itensReceita = db.ItensReceita.Include(i => i.Medicamentos).Include(i => i.ReceitaMedica);
            return View(itensReceita.ToList());
        }

        // GET: ItensReceita/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItensReceita itensReceita = db.ItensReceita.Find(id);
            if (itensReceita == null)
            {
                return HttpNotFound();
            }
            return View(itensReceita);
        }

        // GET: ItensReceita/Create
        public ActionResult Create()
        {
            ViewBag.IDMedicamento = new SelectList(db.Medicamentos, "Id", "NomeMedicamento");
            ViewBag.IDReceita = new SelectList(db.ReceitaMedicas, "Id", "Id");
            return View();
        }

        // POST: ItensReceita/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IDMedicamento,IDReceita")] ItensReceita itensReceita)
        {
            if (ModelState.IsValid)
            {
                db.ItensReceita.Add(itensReceita);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDMedicamento = new SelectList(db.Medicamentos, "Id", "NomeMedicamento", itensReceita.IDMedicamento);
            ViewBag.IDReceita = new SelectList(db.ReceitaMedicas, "Id", "Id", itensReceita.IDReceita);
            return View(itensReceita);
        }

        // GET: ItensReceita/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItensReceita itensReceita = db.ItensReceita.Find(id);
            if (itensReceita == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDMedicamento = new SelectList(db.Medicamentos, "Id", "NomeMedicamento", itensReceita.IDMedicamento);
            ViewBag.IDReceita = new SelectList(db.ReceitaMedicas, "Id", "Id", itensReceita.IDReceita);
            return View(itensReceita);
        }

        // POST: ItensReceita/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IDMedicamento,IDReceita")] ItensReceita itensReceita)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itensReceita).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDMedicamento = new SelectList(db.Medicamentos, "Id", "NomeMedicamento", itensReceita.IDMedicamento);
            ViewBag.IDReceita = new SelectList(db.ReceitaMedicas, "Id", "Id", itensReceita.IDReceita);
            return View(itensReceita);
        }

        // GET: ItensReceita/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItensReceita itensReceita = db.ItensReceita.Find(id);
            if (itensReceita == null)
            {
                return HttpNotFound();
            }
            return View(itensReceita);
        }

        // POST: ItensReceita/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ItensReceita itensReceita = db.ItensReceita.Find(id);
            db.ItensReceita.Remove(itensReceita);
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
