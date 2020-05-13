using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Giori_Consul.Models;
using Giori_Consul.Security;

namespace Giori_Consul.Controllers
{
    [CustomAuthorize(RoleEnum.Admin, RoleEnum.Medico)]
    public class MedicamentosController : BaseController
    {
        private SqlDbContext db = new SqlDbContext();

        // GET: Medicamentos
        public ActionResult Index(string campo = "", string valor = "")
        {
            List<Medicamento> listaMedicamentos;

            if (campo == string.Empty || valor == string.Empty)
                listaMedicamentos = db.Medicamentos.ToList();
            else
                listaMedicamentos = Services.QueryService<Medicamento>.GetListFromFilter(campo, valor, db.Medicamentos);

            ViewBag.itensFilter = Services.QueryService<Medicamento>.GetItensFilter(typeof(Medicamento));

            return View(listaMedicamentos);
        }

        // GET: Medicamentos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicamento medicamento = db.Medicamentos.Find(id);
            if (medicamento == null)
            {
                return HttpNotFound();
            }
            return View(medicamento);
        }

        // GET: Medicamentos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Medicamentos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Medicamento medicamento)
        {
            if (ModelState.IsValid)
            {
                db.Medicamentos.Add(medicamento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(medicamento);
        }

        // GET: Medicamentos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicamento medicamento = db.Medicamentos.Find(id);
            if (medicamento == null)
            {
                return HttpNotFound();
            }
            return View(medicamento);
        }

        // POST: Medicamentos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Medicamento medicamento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicamento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medicamento);
        }

        // GET: Medicamentos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicamento medicamento = db.Medicamentos.Find(id);
            if (medicamento == null)
            {
                return HttpNotFound();
            }
            return View(medicamento);
        }

        // POST: Medicamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medicamento medicamento = db.Medicamentos.Find(id);
            db.Medicamentos.Remove(medicamento);
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
