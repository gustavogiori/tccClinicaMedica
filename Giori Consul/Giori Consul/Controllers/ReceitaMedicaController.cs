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
    public class ReceitaMedicaController : BaseController
    {
        private SqlDbContext db = new SqlDbContext();

        // GET: ReceitaMedica
        public ActionResult Index(string campo = "", string valor = "")
        {
            List<ReceitaMedica> listaReceitaMedica;

            if (campo == string.Empty || valor == string.Empty)
                listaReceitaMedica = db.ReceitaMedicas.Include(r => r.ItensReceita).Include(r => r.Consulta).ToList();
            else
                listaReceitaMedica = Services.QueryService<ReceitaMedica>.GetListFromFilter(campo, valor, db.ReceitaMedicas.Include(m => m.ItensReceita).Include(r => r.Consulta));

            ViewBag.itensFilter = Services.QueryService<ReceitaMedica>.GetItensFilter(typeof(ReceitaMedica));

            return View(listaReceitaMedica);
        }

        // GET: ReceitaMedica/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceitaMedica receitaMedica = db.ReceitaMedicas.Find(id);
            if (receitaMedica == null)
            {
                return HttpNotFound();
            }
            return View(receitaMedica);
        }

        private void SetConsulta(int idSelected = -1)
        {
            ViewBag.ItensReceita = db.ItensReceita.ToList<ItensReceita>();
            ViewBag.IDReceita = new SelectList(db.ReceitaMedicas, "Id", "Id");
            ViewBag.IDConsulta = Services.ConsultaService.GetConsultaComboBox(db, idSelected);
        }

        private void SetIdMedicamento(int[] idsSeleted = null)
        {
            List<MedicamentoDto> medicamentos = new List<MedicamentoDto>();
            foreach (var item in db.Medicamentos)
            {
                MedicamentoDto dto = new MedicamentoDto();
                dto.Id = item.Id;
                dto.Descricao = item.NomeMedicamento + " - " + item.NomeFabricante;
                medicamentos.Add(dto);
            }
            ViewBag.IDMedicamento = new MultiSelectList(medicamentos, "Id", "Descricao", idsSeleted);
        }
        // GET: ReceitaMedica/Create
        public ActionResult Create()
        {

            SetIdMedicamento();
            //var itensReceita = db.ItensReceita.Include(i => i.Medicamentos).Include(i => i.ReceitaMedica);
            SetConsulta();
            return View();
        }

        // POST: ReceitaMedica/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReceitaMedica receitaMedica)
        {
            if (ModelState.IsValid)
            {
                db.ReceitaMedicas.Add(receitaMedica);
                db.SaveChanges();
                AdicionarItensReceita(receitaMedica);
                SetConsulta(receitaMedica.IdConsulta);
                return RedirectToAction("Index");
            }

            SetConsulta();
            return View(receitaMedica);
        }

        private void AdicionarItensReceita(ReceitaMedica receitaMedica)
        {
            foreach (var medicamentoId in receitaMedica.IdMedicamentos)
            {
                ItensReceita item = new ItensReceita();
                item.IDMedicamento = medicamentoId;
                item.IDReceita = receitaMedica.Id;
                db.ItensReceita.Add(item);
                db.SaveChanges();
            }
        }

        // GET: ReceitaMedica/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceitaMedica receitaMedica = db.ReceitaMedicas.Find(id);
            receitaMedica.IdMedicamentos = db.ItensReceita.Where(x => x.IDReceita == receitaMedica.Id).Select(c => c.IDMedicamento).ToList();
            if (receitaMedica == null)
            {
                return HttpNotFound();
            }
            SetConsulta(receitaMedica.IdConsulta);
            SetIdMedicamento(receitaMedica.IdMedicamentos.ToArray());
            return View(receitaMedica);
        }

        // POST: ReceitaMedica/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReceitaMedica receitaMedica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(receitaMedica).State = EntityState.Modified;
                RemoveItensReceita(receitaMedica);
                AdicionarItensReceita(receitaMedica);

                return RedirectToAction("Index");
            }
            SetConsulta(receitaMedica.IdConsulta);
            return View(receitaMedica);
        }

        private void RemoveItensReceita(ReceitaMedica receitaMedica)
        {
            var all = db.ItensReceita.Where(x => x.IDReceita == receitaMedica.Id);
            db.ItensReceita.RemoveRange(all);
            db.SaveChanges();
        }

        // GET: ReceitaMedica/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReceitaMedica receitaMedica = db.ReceitaMedicas.Find(id);
            if (receitaMedica == null)
            {
                return HttpNotFound();
            }
            return View(receitaMedica);
        }

        // POST: ReceitaMedica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReceitaMedica receitaMedica = db.ReceitaMedicas.Find(id);
            RemoveItensReceita(receitaMedica);
            db.ReceitaMedicas.Remove(receitaMedica);
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
    public class MedicamentoDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
    }
    public class ExameDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
    }
}
