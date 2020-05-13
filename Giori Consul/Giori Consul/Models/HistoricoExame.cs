namespace Giori_Consul.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HistoricoExames")]
    public partial class HistoricoExame
    {
        public int Id { get; set; }

        public int IDHistoricoClinico { get; set; }

        public int IDExames { get; set; }

        public virtual Exames Exames { get; set; }

        public virtual HistoricosClinicos HistoricosClinicos { get; set; }
    }
}