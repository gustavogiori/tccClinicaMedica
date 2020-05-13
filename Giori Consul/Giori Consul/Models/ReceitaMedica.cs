using Giori_Consul.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Giori_Consul.Models
{
    [Table("ReceitaMedica")]
    public class ReceitaMedica
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ReceitaMedica()
        {
            ItensReceita = new HashSet<ItensReceita>();
        }
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Display(Name = "Código Consulta")]
        public int IdConsulta { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "O campo data é obrigatório")]
        [DataType(DataType.Date, ErrorMessage = "Insira a data no formato dd/mm/aaaa")]
        [Display(Name = "Data")]
        public DateTime Data { get; set; }

        [Display(Name = "Observação")]
        public string Observacao { get; set; }
        [IgnoreListFilter]
        public List<int> IdMedicamentos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [IgnoreListFilter]
        public virtual ICollection<ItensReceita> ItensReceita { get; set; }
        [IgnoreListFilter]
        public virtual Consulta Consulta { get; set; }
    }
}