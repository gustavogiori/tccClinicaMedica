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
    [CustomAuthorize(RoleEnum.Secretaria)]
    public class EnvioEmailsController : BaseController
    {
        private SqlDbContext db = new SqlDbContext();

        // GET: EnvioEmails
        public ActionResult Index(string campo = "", string valor = "")
        {
            List<EnvioEmail> listaEmails;

            if (campo == string.Empty || valor == string.Empty)
                listaEmails = db.EnvioEmails.Include(m => m.Consulta).ToList();
            else
                listaEmails = Services.QueryService<EnvioEmail>.GetListFromFilter(campo, valor, db.EnvioEmails.Include(m => m.Consulta)).ToList();

            ViewBag.itensFilter = Services.QueryService<EnvioEmail>.GetItensFilter(typeof(EnvioEmail));

            return View(listaEmails);
        }

        // GET: EnvioEmails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnvioEmail envioEmail = db.EnvioEmails.Find(id);
            if (envioEmail == null)
            {
                return HttpNotFound();
            }
            return View(envioEmail);
        }

        // GET: EnvioEmails/Create
        public ActionResult Create()
        {
            SetConsulta();
            return View();
        }

        private void SetConsulta(int idSelected = -1)
        {
            ViewBag.IDConsulta = Services.ConsultaService.GetConsultaComboBox(db.Consultas.ToList(), idSelected);
        }

        // POST: EnvioEmails/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EnvioEmail envioEmail)
        {
            if (ModelState.IsValid)
            {
                Services.EmailService service = new Services.EmailService();
                service.EnviaEmail(envioEmail);
                return RedirectToAction("Index");
            }

            SetConsulta(envioEmail.IDConsulta);
            return View(envioEmail);
        }

        // GET: EnvioEmails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnvioEmail envioEmail = db.EnvioEmails.Find(id);
            if (envioEmail == null)
            {
                return HttpNotFound();
            }
            return View(envioEmail);
        }

        // POST: EnvioEmails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EnvioEmail envioEmail = db.EnvioEmails.Find(id);
            db.EnvioEmails.Remove(envioEmail);
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
