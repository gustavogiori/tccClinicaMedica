using System.ComponentModel.DataAnnotations.Schema;

namespace Giori_Consul.Models
{
    [Table("HistoricoExamesRealizados")]
    public partial class HistoricoExamesRealizado
    {
        public int Id { get; set; }

        public int IDHistoricoClinico { get; set; }

        public int IDExamesRealizados { get; set; }

        public virtual ExamesRealizados ExamesRealizados { get; set; }

        public virtual HistoricosClinicos HistoricosClinicos { get; set; }
    }
}