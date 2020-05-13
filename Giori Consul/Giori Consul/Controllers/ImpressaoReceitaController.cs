using Giori_Consul.Models;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Giori_Consul.Controllers
{
    public class ImpressaoReceitaController : BaseController
    {
        private SqlDbContext db = new SqlDbContext();
        public ActionResult Index(int IdReceita, bool? pdf)
        {
            ViewBag.ItensReceita = db.ItensReceita.Where(x => x.IDReceita == IdReceita).ToList();
            var receita = db.ReceitaMedicas.Where(x => x.Id == IdReceita).ToList().FirstOrDefault();

            if (pdf != true)
            {
                return View(receita);
            }
            else
            {
                var relatorioPDF = new ViewAsPdf
                {
                    ViewName = "ImpressaoReceita",
                    IsGrayScale = false,
                    FileName = "Receita_" + receita.Id + ".pdf",
                    Model = receita
                };
                return relatorioPDF;
            }
        }
        public ActionResult ImpressaoReceita(int IdReceita, bool? pdf)
        {
            ViewBag.ItensReceita = db.ItensReceita.Where(x => x.IDReceita == IdReceita).ToList();
            var receita = db.ReceitaMedicas.Where(x => x.Id == IdReceita).ToList().FirstOrDefault();

            if (pdf != true)
            {
                return View(receita);
            }
            else
            {
                var relatorioPDF = new ViewAsPdf
                {
                    ViewName = "Impressao",
                    IsGrayScale = false,
                    FileName = "Receita_" + receita.Id + ".pdf",
                    Model = receita
                };
                return relatorioPDF;
            }
        }
    }
}