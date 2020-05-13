using Giori_Consul.Models;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Rotativa;

namespace Giori_Consul.Controllers
{
    public class ImpressaoExamesController : Controller
    {
        private SqlDbContext db = new SqlDbContext();
        public ActionResult ExamesRealizados(int IdExame, bool? pdf)
        {
            var exame = db.ExamesRealizados.Where(x => x.Id == IdExame).OrderBy(c => c.DataRealizacao).ToList().FirstOrDefault();

            if (pdf != true)
            {
                return View(exame);
            }
            else
            {
                var relatorioPDF = new ViewAsPdf
                {
                    ViewName = "ImpressaoResultados",
                    IsGrayScale = false,
                    FileName = "Exame_" + exame.DataRealizacao.ToShortDateString() + ".pdf",
                    Model = exame
                };
                return relatorioPDF;
            }
        }
        public ActionResult Index(int IdExame, bool? pdf)
        {
            var exame = db.Exames.Where(x => x.IDExame == IdExame).OrderBy(c => c.DataPedido).ToList().FirstOrDefault();

            if (pdf != true)
            {
                return View(exame);
            }
            else
            {
                var relatorioPDF = new ViewAsPdf
                {
                    ViewName = "Impressao",
                    IsGrayScale = false,
                    FileName = "Exame_" + exame.DataPedido.Value.ToShortDateString() + ".pdf",
                    Model = exame
                };
                return relatorioPDF;
            }
        }
    }
}