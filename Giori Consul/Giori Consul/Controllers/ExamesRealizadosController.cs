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
    public class ExamesRealizadosController : Controller
    {
        private SqlDbContext db = new SqlDbContext();

        // GET: ExamesRealizados
        public ActionResult Index(string campo = "", string valor = "")
        {
            List<ExamesRealizados> listaExamesRealizados;

            if (campo == string.Empty || valor == string.Empty)
                listaExamesRealizados = db.ExamesRealizados.Include(m => m.Exames).ToList();
            else
                listaExamesRealizados = Services.QueryService<ExamesRealizados>.GetListFromFilter(campo, valor, db.ExamesRealizados.Include(m => m.Exames));

            ViewBag.itensFilter = Services.QueryService<ExamesRealizados>.GetItensFilter(typeof(ExamesRealizados));

            return View(listaExamesRealizados);
        }

        // GET: ExamesRealizados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ExamesRealizados examesRealizados = db.ExamesRealizados.Find(id);

            if (examesRealizados == null)
            {
                return HttpNotFound();
            }
            return View(examesRealizados);
        }

        // GET: ExamesRealizados/Create
        public ActionResult Create()
        {
            SetExame();

            return View();
        }

        // POST: ExamesRealizados/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IDExame,Resultados,DataRealizacao")] ExamesRealizados examesRealizados)
        {
            if (ModelState.IsValid)
            {
                db.ExamesRealizados.Add(examesRealizados);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(examesRealizados);
        }

        private void SetExame()
        {
            List<Exames> exames = new List<Exames>();

            foreach (var item in db.Exames)
            {
                var exame = new Exames();
                exame.IDExame = item.IDExame;
                exame.Display = item.IDExame + " - " + item.DataPedido.Value.Date.ToShortDateString();
                exames.Add(exame);
            }

            ViewBag.IDExame = new SelectList(exames, "IDExame", "Display");
        }
        // GET: ExamesRealizados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ExamesRealizados examesRealizados = db.ExamesRealizados.Find(id);

            if (examesRealizados == null)
            {
                return HttpNotFound();
            }

            SetExame();
            return View(examesRealizados);
        }

        // POST: ExamesRealizados/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IDExame,Resultados,DataRealizacao")] ExamesRealizados examesRealizados)
        {
            if (ModelState.IsValid)
            {
                db.Entry(examesRealizados).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SetExame();
            return View(examesRealizados);
        }

        // GET: ExamesRealizados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamesRealizados examesRealizados = db.ExamesRealizados.Find(id);
            if (examesRealizados == null)
            {
                return HttpNotFound();
            }
            return View(examesRealizados);
        }

        // POST: ExamesRealizados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExamesRealizados examesRealizados = db.ExamesRealizados.Find(id);
            db.ExamesRealizados.Remove(examesRealizados);
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
