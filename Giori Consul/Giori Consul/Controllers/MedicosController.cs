using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Giori_Consul.Models;
using Giori_Consul.Security;

namespace Giori_Consul.Controllers
{
    [CustomAuthorize(RoleEnum.Admin)]
    public class MedicosController : BaseController
    {

        private SqlDbContext db = new SqlDbContext();

        // GET: Medicos
        public ActionResult Index(string campo = "", string valor = "")
        {
            List<Medico> listaMedicos;

            if (campo == string.Empty || valor == string.Empty)
                listaMedicos = db.Medicos.Include(m => m.Users).ToList();
            else
                listaMedicos = Services.QueryService<Medico>.GetListFromFilter(campo, valor, db.Medicos.Include(m => m.Users));

            ViewBag.itensFilter = Services.QueryService<Medico>.GetItensFilter(typeof(Medico));

            return View(listaMedicos);
        }

        // GET: Medicos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medico medico = db.Medicos.Find(id);
            if (medico == null)
            {
                return HttpNotFound();
            }
            return View(medico);
        }

        // GET: Medicos/Create
        public ActionResult Create()
        {
            ViewBag.IDUser = new SelectList(db.Users, "Id", "UserId");
            return View();
        }

        // POST: Medicos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Medico medico)
        {
            if (ModelState.IsValid)
            {
                Services.RetornoServiceUser userResponse = new Services.RetornoServiceUser();
                Services.UserServies userServie = new Services.UserServies();
                userResponse = userServie.InserUsuario(Security.RoleEnum.Medico, medico.Login, medico.Senha, medico.Nome);

                if (userResponse.Sucess)
                {
                    medico.IDUser = userResponse.IdUser;
                    db.Medicos.Add(medico);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Erro"] = userResponse.MsgError;
                    return View(medico);
                }

            }

            ViewBag.IDUser = new SelectList(db.Users, "Id", "UserId", medico.IDUser);
            return View(medico);
        }

        // GET: Medicos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medico medico = db.Medicos.Find(id);
            if (medico == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDUser = new SelectList(db.Users, "Id", "UserId", medico.IDUser);
            return View(medico);
        }

        // POST: Medicos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Medico medico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDUser = new SelectList(db.Users, "Id", "UserId", medico.IDUser);
            return View(medico);
        }

        // GET: Medicos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medico medico = db.Medicos.Find(id);
            if (medico == null)
            {
                return HttpNotFound();
            }
            return View(medico);
        }

        // POST: Medicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medico medico = db.Medicos.Find(id);
            db.Medicos.Remove(medico);
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
