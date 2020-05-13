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
    public class ExamesController : BaseController
    {
        private SqlDbContext db = new SqlDbContext();

        // GET: Exames
        [CustomAuthorize(RoleEnum.Medico)]
        public ActionResult Index(string campo = "", string valor = "")
        {
            List<Exames> listaExames;

            if (campo == string.Empty || valor == string.Empty)
                listaExames = db.Exames.Include(m => m.Pacientes).Include(x => x.TipoExames).ToList();
            else
                listaExames = Services.QueryService<Exames>.GetListFromFilter(campo, valor, db.Exames.Include(m => m.Pacientes).Include(x => x.TipoExames));

            ViewBag.itensFilter = Services.QueryService<Exames>.GetItensFilter(typeof(Exames));

            return View(listaExames);
        }

        // GET: Exames/Details/5
        [CustomAuthorize(RoleEnum.Medico)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Exames exames = db.Exames.Find(id);

            if (exames == null)
            {
                return HttpNotFound();
            }

            return View(exames);
        }

        // GET: Exames/Create
        [CustomAuthorize(RoleEnum.Medico)]
        public ActionResult Create()
        {
            ViewBag.IDPaciente = new SelectList(db.Pacientes, "IDPaciente", "Nome");
            ViewBag.IDTipoExame = new SelectList(db.TipoExames, "Id", "Descricao");

            return View();
        }

        // POST: Exames/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(RoleEnum.Medico)]
        public ActionResult Create(Exames exames)
        {
            if (ModelState.IsValid)
            {
                var idUserMedico = Session["UserId"].ToString();
                var id = db.Users.FirstOrDefault(x => x.UserId == idUserMedico).Id;
                var medico = db.Medicos.FirstOrDefault(x => x.IDUser == id);
                exames.IDMedico = medico.IDMedico;
                db.Exames.Add(exames);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDTipoExame = new SelectList(db.TipoExames, "Id", "Descricao", exames.IDTipoExame);
            ViewBag.IDPaciente = new SelectList(db.Pacientes, "IDPaciente", "Nome", exames.IDPaciente);

            return View(exames);
        }

        // GET: Exames/Edit/5
        [CustomAuthorize(RoleEnum.Medico)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Exames exames = db.Exames.Find(id);

            if (exames == null)
            {
                return HttpNotFound();
            }

            ViewBag.IDPaciente = new SelectList(db.Pacientes, "IDPaciente", "Nome", exames.IDPaciente);
            ViewBag.IDTipoExame = new SelectList(db.TipoExames, "Id", "Descricao", exames.IDTipoExame);

            return View(exames);
        }

        // POST: Exames/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(RoleEnum.Medico)]
        public ActionResult Edit(Exames exames)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exames).State = EntityState.Modified;
                var idUserMedico = Session["UserId"].ToString();
                var id = db.Users.FirstOrDefault(x => x.UserId == idUserMedico).Id;
                var medico = db.Medicos.FirstOrDefault(x => x.IDUser == id);
                exames.IDMedico = medico.IDMedico;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDPaciente = new SelectList(db.Pacientes, "IDPaciente", "Nome", exames.IDPaciente);
            ViewBag.IDTipoExame = new SelectList(db.TipoExames, "Id", "Descricao", exames.IDTipoExame);

            return View(exames);
        }

        // GET: Exames/Delete/5
        [CustomAuthorize(RoleEnum.Medico)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Exames exames = db.Exames.Find(id);

            if (exames == null)
            {
                return HttpNotFound();
            }

            return View(exames);
        }

        // POST: Exames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(RoleEnum.Medico)]
        public ActionResult DeleteConfirmed(int id)
        {
            Exames exames = db.Exames.Find(id);
            db.Exames.Remove(exames);
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
