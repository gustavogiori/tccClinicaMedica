using Giori_Consul.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Giori_Consul.Models
{
    public partial class Exames
    {
        [Key]
        [Display(Name = "Código")]
        public int IDExame { get; set; }

        [Display(Name = "Código Paciente")]
        public int? IDPaciente { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data de Solicitação")]
        [Required(ErrorMessage = "O campo data é obrigatório")]
        [DataType(DataType.Date, ErrorMessage = "Insira a data no formato dd/mm/aaaa")]
        public DateTime? DataPedido { get; set; }

        [Display(Name = "Código Tipo Exame")]
        public int IDTipoExame { get; set; }

        public int IDMedico { get; set; }

        [Display(Name = "Exames Solicitados")]
        public string ExamesSolicitados { get; set; }

        [NotMapped]
        [IgnoreListFilter]
        public string Display { get; set; }

        [IgnoreListFilter]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExamesRealizados> ExamesRealizados { get; set; }
        [IgnoreListFilter]
        public virtual Paciente Pacientes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoricoExame> HistoricoExames { get; set; }

        [ForeignKey("IDTipoExame")]
        public virtual TipoExame TipoExames { get; set; }

        [ForeignKey("IDMedico")]
        public virtual Medico Medico { get; set; }
    }
}