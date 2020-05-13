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
    public class HistoricosClinicosController : Controller
    {
        private SqlDbContext db = new SqlDbContext();

        // GET: HistoricosClinicos
        public ActionResult Index(string campo = "", string valor = "")
        {
            List<HistoricosClinicos> listaHistoricosClinicos;

            if (campo == string.Empty || valor == string.Empty)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                listaHistoricosClinicos = Services.QueryService<HistoricosClinicos>.GetListFromFilter(campo, valor, db.HistoricosClinicos.Include(x => x.Paciente));
                PreencheItensHistorico(listaHistoricosClinicos);

                ViewData["IdPaciente"] = valor;
                ViewBag.itensFilter = Services.QueryService<HistoricosClinicos>.GetItensFilter(typeof(HistoricosClinicos));

                return View(listaHistoricosClinicos);
            }
        }

        private void PreencheItensHistorico(List<HistoricosClinicos> listaHistoricosClinicos)
        {
            foreach (var item in listaHistoricosClinicos)
            {
                item.HistoricoExames = db.HistoricoExames.Where(x => x.IDHistoricoClinico == item.Id).ToList();
                item.HistoricoExamesRealizados = db.HistoricoExamesRealizados.Where(x => x.IDHistoricoClinico == item.Id).ToList();
                item.HistoricoMedicamentos = db.HistoricoMedicamentos.Where(x => x.IDMedicamentos == item.Id).ToList();
            }
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
        private void SetIdExames(int[] idsSeleted = null)
        {
            List<ExameDto> medicamentos = new List<ExameDto>();
            foreach (var item in db.Exames)
            {
                ExameDto dto = new ExameDto();
                dto.Id = item.IDExame;
                dto.Descricao = item.IDExame + " - " + item.DataPedido.Value.ToShortDateString();
                medicamentos.Add(dto);
            }
            ViewBag.IDExames = new MultiSelectList(medicamentos, "Id", "Descricao", idsSeleted);
        }
        private void SetIdExamesRealizados(int[] idsSeleted = null)
        {
            List<ExamesRealizados> medicamentos = new List<ExamesRealizados>();
            foreach (var item in db.ExamesRealizados.Include(x => x.Exames))
            {
                ExamesRealizados dto = new ExamesRealizados();
                dto.Id = item.Id;
                dto.Display = item.Exames.IDExame + " - " + item.Exames.DataPedido.Value.ToShortDateString();
                medicamentos.Add(dto);
            }
            ViewBag.IDExamesRealizados = new MultiSelectList(medicamentos, "Id", "Display", idsSeleted);
        }
        // GET: HistoricosClinicos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistoricosClinicos historicosClinicos = db.HistoricosClinicos.Include(x => x.Paciente).FirstOrDefault(x => x.Id == id);
            if (historicosClinicos == null)
            {
                return HttpNotFound();
            }

            var idsMedicamento = db.HistoricoMedicamentos.Select(x => x.IDMedicamentos).ToList();
            ViewBag.Medicamentos = db.Medicamentos.Where(x => idsMedicamento.Contains(x.Id)).ToList();

            var idsExames = db.HistoricoExames.Select(x => x.IDExames).ToList();
            ViewBag.Exames = db.Exames.Include(x => x.Pacientes).Where(x => idsExames.Contains(x.IDExame)).ToList();


            var idsExamesRealizados = db.HistoricoExamesRealizados.Select(x => x.IDExamesRealizados).ToList();
            ViewBag.ExamesRealizados = db.ExamesRealizados.Include(x => x.Exames).Where(x => idsExamesRealizados.Contains(x.Id)).ToList();

            return View(historicosClinicos);
        }

        // GET: HistoricosClinicos/Create
        public ActionResult Create(int idPaciente)
        {
            SetIdExames();
            SetIdExamesRealizados();
            SetIdMedicamento();

            return View(new HistoricosClinicos() { IDPaciente = idPaciente });
        }
        private void RemoveItensMedicamentos(HistoricosClinicos historicoClinico)
        {
            var all = db.HistoricoMedicamentos.Where(x => x.IDHistoricoClinico == historicoClinico.Id);
            db.HistoricoMedicamentos.RemoveRange(all);
            db.SaveChanges();
        }
        private void RemoveItensExames(HistoricosClinicos historicoClinico)
        {
            var all = db.HistoricoExames.Where(x => x.IDHistoricoClinico == historicoClinico.Id);
            db.HistoricoExames.RemoveRange(all);
            db.SaveChanges();
        }
        private void RemoveItensExamesRealizados(HistoricosClinicos historicoClinico)
        {
            var all = db.HistoricoExamesRealizados.Where(x => x.IDHistoricoClinico == historicoClinico.Id);
            db.HistoricoExamesRealizados.RemoveRange(all);
            db.SaveChanges();
        }
        private void AdicionarItensMedicamentos(HistoricosClinicos historicoClinico)
        {
            foreach (var medicamentoId in historicoClinico.IdsMedicamentos)
            {
                HistoricoMedicamento item = new HistoricoMedicamento();
                item.IDMedicamentos = medicamentoId;
                item.IDHistoricoClinico = historicoClinico.Id;
                db.HistoricoMedicamentos.Add(item);
                db.SaveChanges();
            }
        }
        private void AdicionarItensHistoricoExames(HistoricosClinicos historicoClinico)
        {
            if (historicoClinico.IdsExames != null)
            {
                foreach (var exameId in historicoClinico.IdsExames)
                {
                    HistoricoExame item = new HistoricoExame();
                    item.IDExames = exameId;
                    item.IDHistoricoClinico = historicoClinico.Id;
                    db.HistoricoExames.Add(item);
                    db.SaveChanges();
                }
            }
        }
        private void AdicionarItensHistoricoExamesRealizados(HistoricosClinicos historicoClinico)
        {
            if (historicoClinico.IdsExamesRealizados != null)
            {
                foreach (var exameId in historicoClinico.IdsExamesRealizados)
                {
                    HistoricoExamesRealizado item = new HistoricoExamesRealizado();
                    item.IDExamesRealizados = exameId;
                    item.IDHistoricoClinico = historicoClinico.Id;
                    db.HistoricoExamesRealizados.Add(item);
                    db.SaveChanges();
                }
            }
        }
        // POST: HistoricosClinicos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HistoricosClinicos historicosClinicos)
        {
            if (ModelState.IsValid)
            {
                db.HistoricosClinicos.Add(historicosClinicos);
                db.SaveChanges();
                AdicionarItens(historicosClinicos);

                return RedirectToAction("Index", new { campo = "IDPaciente", valor = historicosClinicos.IDPaciente });
            }

            return View(historicosClinicos);
        }

        private void AdicionarItens(HistoricosClinicos historicosClinicos)
        {
            AdicionarItensMedicamentos(historicosClinicos);
            AdicionarItensHistoricoExames(historicosClinicos);
            AdicionarItensHistoricoExamesRealizados(historicosClinicos);
        }

        // GET: HistoricosClinicos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            HistoricosClinicos historicosClinicos = db.HistoricosClinicos.Find(id);
            List<HistoricosClinicos> lista = new List<HistoricosClinicos>() { historicosClinicos };
            PreencheItensHistorico(lista);
            historicosClinicos = lista.FirstOrDefault();

            historicosClinicos.IdsExames = db.HistoricoExames.Where(x => x.HistoricosClinicos.Id == historicosClinicos.Id).Select(c => c.IDExames).ToList();
            historicosClinicos.IdsExamesRealizados = historicosClinicos.HistoricoExamesRealizados.Select(x => x.IDExamesRealizados).ToList();
            historicosClinicos.IdsMedicamentos = db.HistoricoMedicamentos.Where(x => x.HistoricosClinicos.Id == historicosClinicos.Id).Select(c => c.IDMedicamentos).ToList();

            SetIdExames(historicosClinicos.IdsExames.ToArray());
            SetIdExamesRealizados(historicosClinicos.IdsExamesRealizados.ToArray());
            SetIdMedicamento(historicosClinicos.IdsMedicamentos.ToArray());

            if (historicosClinicos == null)
            {
                return HttpNotFound();
            }
            return View(historicosClinicos);
        }

        // POST: HistoricosClinicos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HistoricosClinicos historicosClinicos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(historicosClinicos).State = EntityState.Modified;

                RemoveItens(historicosClinicos);
                AdicionarItens(historicosClinicos);

                db.SaveChanges();
                
                return RedirectToAction("Index", new { campo = "IDPaciente", valor = historicosClinicos.IDPaciente });
            }
            return View(historicosClinicos);
        }

        private void RemoveItens(HistoricosClinicos historicosClinicos)
        {
            RemoveItensExames(historicosClinicos);
            RemoveItensExamesRealizados(historicosClinicos);
            RemoveItensMedicamentos(historicosClinicos);
        }

        // GET: HistoricosClinicos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistoricosClinicos historicosClinicos = db.HistoricosClinicos.Find(id);
            if (historicosClinicos == null)
            {
                return HttpNotFound();
            }
            return View(historicosClinicos);
        }

        // POST: HistoricosClinicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HistoricosClinicos historicosClinicos = db.HistoricosClinicos.Find(id);
            int idPaciente = historicosClinicos.IDPaciente;
            RemoveItens(historicosClinicos);
            db.HistoricosClinicos.Remove(historicosClinicos);
            db.SaveChanges();

            return RedirectToAction("Index", new { campo = "IDPaciente", valor = idPaciente });
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
