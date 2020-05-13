namespace Giori_Consul.Models
{
    using Giori_Consul.DataAnnotations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuario()
        {
            Medicos = new HashSet<Medico>();
            Secretarias = new HashSet<Secretaria>();
        }

        [IgnoreListFilter]
        public int Id { get; set; }

        [Display(Name = "Login")]
        public string UserId { get; set; }

        [Display(Name = "Nome")]
        public string UserName { get; set; }
        [IgnoreListFilter]
        [Required(ErrorMessage = "O campo senha é obrigatório")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Tipo usuário")]
        public int RoleId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [IgnoreListFilter]
        public virtual ICollection<Medico> Medicos { get; set; }
        [IgnoreListFilter]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Secretaria> Secretarias { get; set; }
    }
}
