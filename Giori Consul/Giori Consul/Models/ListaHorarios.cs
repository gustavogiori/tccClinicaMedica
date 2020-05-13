using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Giori_Consul.Models
{
    public class ListaHorarios
    {
        private static IList<SelectListItem> GetItens(string HorarioSelecionado)
        {
            IList<SelectListItem> time = new List<SelectListItem>();
            DateTime dtInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
            DateTime dtFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 30, 0);

            while (dtInicio < dtFim)
            {
                SelectListItem item;
                if (dtInicio.ToShortTimeString().Equals(HorarioSelecionado))
                {
                    item = new SelectListItem { Text = dtInicio.ToShortTimeString(), Value = dtInicio.ToShortTimeString(), Selected = true };
                }
                else
                {
                    item = new SelectListItem { Text = dtInicio.ToShortTimeString(), Value = dtInicio.ToShortTimeString() };
                }
                time.Add(item);
                dtInicio = dtInicio.AddMinutes(30);
            }

            return time;
        }
        public static IEnumerable<SelectListItem> getHorarios(string HorarioSelecionado = "")
        {
            return new SelectList(GetItens(HorarioSelecionado), "Text", "Value", HorarioSelecionado);

        }
    }
}