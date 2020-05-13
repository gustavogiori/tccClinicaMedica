using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Giori_Consul.Models
{
    [Table("Medicamentos")]
    public partial class Medicamento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Medicamento()
        {
            ItensReceita = new HashSet<ItensReceita>();
        }

        [Display(Name = "Código")]
        public int Id { get; set; }

        [StringLength(100)]
        [Display(Name = "Nome do Medicamento")]
        public string NomeMedicamento { get; set; }

        [StringLength(100)]
        [Display(Name = "Nome Genérico")]
        public string NomeGenerico { get; set; }

        [StringLength(100)]
        [Display(Name = "Nome do Fabricante")]
        public string NomeFabricante { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItensReceita> ItensReceita { get; set; }
    }
}