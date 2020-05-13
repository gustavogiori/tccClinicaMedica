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
    public class ConsultasController : BaseController
    {
        private SqlDbContext db = new SqlDbContext();


        // GET: Consultas
        [CustomAuthorize(RoleEnum.Admin, RoleEnum.Medico, RoleEnum.Secretaria)]
        public ActionResult Index(string campo = "", string valor = "", DateTime? data = null)
        {
            List<Consulta> consultas;
            DateTime dtFilter = data == null ? DateTime.Now.Date : data.Value;

            if (campo == string.Empty || valor == string.Empty)
                consultas = db.Consultas.Include(c => c.Medico).Include(c => c.Paciente).Where(x => x.Data == dtFilter && x.Cancelada == false).OrderBy(x => x.Time).ToList();
            else
                consultas = Services.QueryService<Consulta>.GetListFromFilter(campo, valor, db.Consultas.Include(c => c.Medico).Include(c => c.Paciente)).Where(x => x.Data == data && x.Cancelada == false).OrderBy(x => x.Time).ToList();
            var itens = Services.QueryService<Consulta>.GetItensFilter(typeof(Consulta));

            ViewData["AllConsultas"] = db.Consultas.ToList();
            var select = new SelectListItem { Text = "Nome Paciente", Value = "paciente.Nome" };
            itens.Add(select);
            ViewBag.itensFilter = itens;

            return View(consultas);
        }

        public JsonResult DatasDisponiveis(string IDMedico, string Horarios, DateTime data)
        {
            List<ConsultaDisponivel> disponiveis = new List<ConsultaDisponivel>();
            foreach (var item in ListaHorarios.getHorarios())
            {

                var value = item.Value.Split(':');
                if (Horarios == "M" && Convert.ToInt32(value[0]) > 12)
                {
                    continue;
                }
                else if (Horarios == "T" && Convert.ToInt32(value[0]) < 13)
                {
                    continue;
                }

                var dataConsulta = new DateTime(data.Year, data.Month, data.Day, Convert.ToInt32(value[0]), Convert.ToInt32(value[1]), 0);
                var idMedico = Convert.ToInt32(IDMedico);
                Consulta consulta = db.Consultas.FirstOrDefault(x => x.Time == dataConsulta && x.IDMedico == idMedico);

                if (consulta == null)
                {
                    var NomeMedico = db.Medicos.FirstOrDefault(x => x.IDMedico == idMedico).Nome;
                    disponiveis.Add(new ConsultaDisponivel() { Medico = NomeMedico, Data = dataConsulta.Date.ToShortDateString(), Horario = item.Value });
                }
            }
            return Json(disponiveis, JsonRequestBehavior.AllowGet);
        }

        // GET: Consultas/Details/5
        [CustomAuthorize(RoleEnum.Admin, RoleEnum.Medico, RoleEnum.Secretaria)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consulta consulta = db.Consultas.Find(id);
            if (consulta == null)
            {
                return HttpNotFound();
            }
            return View(consulta);
        }

        // GET: Consultas/Create
        [CustomAuthorize(RoleEnum.Admin, RoleEnum.Medico, RoleEnum.Secretaria)]
        public ActionResult Create()
        {
            ViewBag.IDMedico = new SelectList(db.Medicos, "IDMedico", "Nome");
            ViewBag.IDPaciente = new SelectList(db.Pacientes, "IDPaciente", "Nome");
            SetViewBagHorario();
            return View();
        }

        // POST: Consultas/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(RoleEnum.Admin, RoleEnum.Medico, RoleEnum.Secretaria)]
        public ActionResult Create(Consulta consulta)
        {
            if (ModelState.IsValid)
            {
                consulta.Time = new DateTime(consulta.Data.Year, consulta.Data.Month, consulta.Data.Day, consulta.Time.Hour, consulta.Time.Minute, 0);

                if (db.Consultas.Any(x => x.Cancelada == false && x.IDMedico == consulta.IDMedico && x.Time == consulta.Time))
                {

                    ViewBag.IDMedico = new SelectList(db.Medicos, "IDMedico", "Nome", consulta.IDMedico);
                    ViewBag.IDPaciente = new SelectList(db.Pacientes, "IDPaciente", "Nome", consulta.IDPaciente);
                    SetViewBagHorario();
                    TempData["Erro"] = $"Já existe uma consulta para o médico :{db.Medicos.FirstOrDefault(x => x.IDMedico == consulta.IDMedico).Nome} no horário: {consulta.Time}";
                    return View(consulta);
                }

                db.Consultas.Add(consulta);
                db.SaveChanges();

                if (consulta.EnviaEmail)
                {
                    Services.EmailService email = new Services.EmailService();
                    consulta.Paciente = db.Pacientes.FirstOrDefault(x => x.IDPaciente == consulta.IDPaciente);

                    email.EnviaEmailConsulta(consulta);
                }
                return RedirectToAction("Index");
            }

            ViewBag.IDMedico = new SelectList(db.Medicos, "IDMedico", "Nome", consulta.IDMedico);
            ViewBag.IDPaciente = new SelectList(db.Pacientes, "IDPaciente", "Nome", consulta.IDPaciente);
            SetViewBagHorario();
            return View(consulta);
        }

        // GET: Consultas/Edit/5
        [CustomAuthorize(RoleEnum.Admin, RoleEnum.Medico, RoleEnum.Secretaria)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consulta consulta = db.Consultas.Find(id);
            if (consulta == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDMedico = new SelectList(db.Medicos, "IDMedico", "Nome", consulta.IDMedico);
            ViewBag.IDPaciente = new SelectList(db.Pacientes, "IDPaciente", "Nome", consulta.IDPaciente);
            SetViewBagHorario();
            return View(consulta);
        }

        // POST: Consultas/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(RoleEnum.Admin, RoleEnum.Medico, RoleEnum.Secretaria)]
        public ActionResult Edit(Consulta consulta)
        {
            if (ModelState.IsValid)
            {
                consulta.Time = new DateTime(consulta.Data.Year, consulta.Data.Month, consulta.Data.Day, consulta.Time.Hour, consulta.Time.Minute, 0);

                if (db.Consultas.Any(x => x.Cancelada == false && x.IDMedico == consulta.IDMedico && x.Time == consulta.Time && x.IDConsulta != consulta.IDConsulta))
                {

                    ViewBag.IDMedico = new SelectList(db.Medicos, "IDMedico", "Nome", consulta.IDMedico);
                    ViewBag.IDPaciente = new SelectList(db.Pacientes, "IDPaciente", "Nome", consulta.IDPaciente);
                    SetViewBagHorario();

                    TempData["Erro"] = $"Já existe uma consulta para o médico :{db.Medicos.FirstOrDefault(x => x.IDMedico == consulta.IDMedico).Nome} no horário: {consulta.Time}";

                    return View(consulta);
                }

                db.Entry(consulta).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.IDMedico = new SelectList(db.Medicos, "IDMedico", "Nome", consulta.IDMedico);
            ViewBag.IDPaciente = new SelectList(db.Pacientes, "IDPaciente", "Nome", consulta.IDPaciente);
            SetViewBagHorario();

            return View(consulta);
        }

        // GET: Consultas/Delete/5
        [CustomAuthorize(RoleEnum.Admin, RoleEnum.Medico, RoleEnum.Secretaria)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consulta consulta = db.Consultas.Find(id);
            if (consulta == null)
            {
                return HttpNotFound();
            }
            return View(consulta);
        }

        // POST: Consultas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(RoleEnum.Admin, RoleEnum.Medico, RoleEnum.Secretaria)]
        public ActionResult DeleteConfirmed(int id)
        {
            Consulta consulta = db.Consultas.Find(id);
            db.Consultas.Remove(consulta);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void SetViewBagHorario()
        {
            SelectListItem manha = new SelectListItem() { Value = "M", Text = "Manhã" };
            SelectListItem tarde = new SelectListItem() { Value = "T", Text = "Tarde" };
            SelectListItem ambos = new SelectListItem() { Value = "A", Text = "Ambos" };

            List<SelectListItem> itens = new List<SelectListItem>() { manha, tarde, ambos };
            SelectList list = new SelectList(itens, "Value", "Text");

            ViewBag.Horarios = list;
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
