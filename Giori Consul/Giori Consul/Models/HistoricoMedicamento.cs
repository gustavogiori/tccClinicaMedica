using System.ComponentModel.DataAnnotations.Schema;

namespace Giori_Consul.Models
{
    [Table("HistoricoMedicamentos")]
    public partial class HistoricoMedicamento
    {
        public int Id { get; set; }

        public int IDMedicamentos { get; set; }

        public int IDHistoricoClinico { get; set; }

        [ForeignKey("HistoricosClinicos")]
        public virtual HistoricosClinicos HistoricosClinicos { get; set; }

        [ForeignKey("IDMedicamentos")]
        public virtual Medicamento Medicamentos { get; set; }
    }
}