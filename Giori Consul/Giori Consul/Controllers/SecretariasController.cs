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
    [CustomAuthorize(RoleEnum.Admin)]
    public class SecretariasController : BaseController
    {
        private SqlDbContext db = new SqlDbContext();

        // GET: Secretarias
        public ActionResult Index(string campo = "", string valor = "")
        {
            List<Secretaria> listaSecretarias;

            if (campo == string.Empty || valor == string.Empty)
                listaSecretarias = db.Secretarias.Include(m => m.Users).ToList();
            else
                listaSecretarias = Services.QueryService<Secretaria>.GetListFromFilter(campo, valor, db.Secretarias.Include(m => m.Users));

            ViewBag.itensFilter = Services.QueryService<Secretaria>.GetItensFilter(typeof(Secretaria));

            return View(listaSecretarias);
        }

        // GET: Secretarias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Secretaria secretaria = db.Secretarias.Find(id);
            if (secretaria == null)
            {
                return HttpNotFound();
            }
            return View(secretaria);
        }

        // GET: Secretarias/Create
        public ActionResult Create()
        {
            ViewBag.IDUser = new SelectList(db.Users, "Id", "UserId");
            return View();
        }

        // POST: Secretarias/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Secretaria secretaria)
        {
            if (ModelState.IsValid)
            {
                Services.RetornoServiceUser userResponse = new Services.RetornoServiceUser();
                Services.UserServies userServie = new Services.UserServies();
                userResponse = userServie.InserUsuario(Security.RoleEnum.Medico, secretaria.Login, secretaria.Senha, secretaria.Nome);

                if (userResponse.Sucess)
                {
                    secretaria.IDUser = userResponse.IdUser;
                    db.Secretarias.Add(secretaria);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Erro"] = userResponse.MsgError;
                    return View(secretaria);
                }

            }

            ViewBag.IDUser = new SelectList(db.Secretarias, "Id", "UserId", secretaria.IDUser);
            return View(secretaria);
        }

        // GET: Secretarias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Secretaria secretaria = db.Secretarias.Find(id);
            if (secretaria == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDUser = new SelectList(db.Users, "Id", "UserId", secretaria.IDUser);
            return View(secretaria);
        }

        // POST: Secretarias/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Secretaria secretaria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(secretaria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDUser = new SelectList(db.Users, "Id", "UserId", secretaria.IDUser);
            return View(secretaria);
        }

        // GET: Secretarias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Secretaria secretaria = db.Secretarias.Find(id);
            if (secretaria == null)
            {
                return HttpNotFound();
            }
            return View(secretaria);
        }

        // POST: Secretarias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Secretaria secretaria = db.Secretarias.Find(id);
            db.Secretarias.Remove(secretaria);
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
