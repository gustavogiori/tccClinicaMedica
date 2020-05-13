namespace Giori_Consul.Models
{
    using Giori_Consul.DataAnnotations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public partial class ExamesRealizados
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Display(Name = "Código Exame")]
        [Required]
        public int IDExame { get; set; }
        [Display(Name = "Resultados")]
        [Required]
        public string Resultados { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data de Realização")]
        [Required(ErrorMessage = "O campo data é obrigatório")]
        [DataType(DataType.Date, ErrorMessage = "Insira a data no formato dd/mm/aaaa")]
        public DateTime DataRealizacao { get; set; }

        [NotMapped]
        [IgnoreListFilter]
        public string Display { get; set; }

        [IgnoreListFilter]
        public virtual Exames Exames { get; set; }
        [IgnoreListFilter]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoricoExamesRealizado> HistoricoExamesRealizados { get; set; }
    }
}
