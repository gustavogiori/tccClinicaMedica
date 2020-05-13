using Giori_Consul.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Giori_Consul.Models
{
    [Table("HistoricosClinicos")]
    public partial class HistoricosClinicos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HistoricosClinicos()
        {
            HistoricoExames = new HashSet<HistoricoExame>();
            HistoricoExamesRealizados = new HashSet<HistoricoExamesRealizado>();
            HistoricoMedicamentos = new HashSet<HistoricoMedicamento>();
        }

        public int Id { get; set; }

        public string Observacao { get; set; }

        [Display(Name = "Paciente")]
        public int IDPaciente { get; set; }

        [IgnoreListFilter]
        [NotMapped]
        public List<int> IdsMedicamentos { get; set; }

        [IgnoreListFilter]
        [NotMapped]
        public List<int> IdsExamesRealizados { get; set; }

        [IgnoreListFilter]
        [NotMapped]
        public List<int> IdsExames { get; set; }
        [IgnoreListFilter]
        public virtual ICollection<HistoricoExame> HistoricoExames { get; set; }
        [IgnoreListFilter]
        public virtual ICollection<HistoricoExamesRealizado> HistoricoExamesRealizados { get; set; }
        [IgnoreListFilter]
        public virtual ICollection<HistoricoMedicamento> HistoricoMedicamentos { get; set; }
        [ForeignKey("IDPaciente")]
        [IgnoreListFilter]
        public Paciente Paciente { get; set; }
    }
}