namespace Giori_Consul.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Giori_Consul.DataAnnotations;

    public partial class Medico
    {
        [Key]
        [Display(Name = "C�digo")]
        public int IDMedico { get; set; }

        public int IDUser { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name ="CRM")]
        public string RG { get; set; }

        [Required]
        [StringLength(15)]
        public string Telefone { get; set; }

        [Required]
        [Display(Name = "Endere�o")]
        public string Endereco { get; set; }

        [Required]
        public string Especialidade { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "O campo Login � obrigat�rio")]
        public string Login { get; set; }
        [Required(ErrorMessage = "O campo senha � obrigat�rio")]
        [NotMapped]
        [IgnoreListFilter]
        public string Senha { get; set; }
        [IgnoreListFilter]
        public virtual Usuario Users { get; set; }
    }
}
