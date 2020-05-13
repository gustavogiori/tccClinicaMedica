using Giori_Consul.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Giori_Consul.Services
{
    public class ConsultaService
    {





       
        public static List<SelectListItem> GetConsultaComboBox(List<Consulta> consultas, int idSelected)
        {
            List<SelectListItem> select = new List<SelectListItem>();

            foreach (Consulta consulta in consultas)
            {
                SetConsulta(idSelected, select, consulta);

            }
            return select;

        }

        private static void SetConsulta(int idSelected, List<SelectListItem> select, Consulta consulta)
        {
            var item = new SelectListItem();
         

            item.Value = consulta.IDConsulta.ToString();
            item.Text = consulta.Time.ToString() + " - " + consulta.Paciente.Nome;
            if (idSelected != -1)
            {
                if (idSelected == consulta.IDConsulta)
                {
                    item.Selected = true;
                }
            }
            select.Add(item);
        }
    }
}