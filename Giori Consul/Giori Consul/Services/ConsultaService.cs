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
        public static List<SelectListItem> GetConsultaComboBox(SqlDbContext db, int idSelected)
        {
            List<SelectListItem> select = new List<SelectListItem>();

            foreach (Consulta consulta in db.Consultas)
            {

                var item = new SelectListItem();

                if (idSelected == consulta.IDConsulta)
                {
                    item.Selected = true;
                }

                item.Value = consulta.IDConsulta.ToString();
                item.Text = consulta.Time.ToString() + " - " + consulta.Paciente.Nome;

                select.Add(item);

            }
            return select;

        }
    }
}