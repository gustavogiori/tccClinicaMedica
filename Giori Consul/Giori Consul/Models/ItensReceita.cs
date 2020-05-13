using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Giori_Consul.Models
{
    [Table("ItensReceita")]
    public partial class ItensReceita
    {
        public int Id { get; set; }

        public int IDMedicamento { get; set; }

        public int IDReceita { get; set; }

        public virtual Medicamento Medicamentos { get; set; }

        public virtual ReceitaMedica ReceitaMedica { get; set; }
    }
}