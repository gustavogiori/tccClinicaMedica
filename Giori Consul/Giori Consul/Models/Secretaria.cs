namespace Giori_Consul.Models
{
    using Giori_Consul.DataAnnotations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Secretaria
    {
        [Display(Name = "C�digo")]
        [Key]
        public int IDSecretaria { get; set; }
        [IgnoreListFilter]
        public int IDUser { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [StringLength(10)]
        public string RG { get; set; }

        [Required]
        [StringLength(15)]
        public string Telefone { get; set; }

        [Required]
        public string Endereco { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "O campo Login � obrigat�rio")]
        [IgnoreListFilter]
        public string Login { get; set; }
        [Required(ErrorMessage = "O campo senha � obrigat�rio")]
        [NotMapped]
        [DataType(DataType.Password)]
        [IgnoreListFilter]
        public string Senha { get; set; }
        [IgnoreListFilter]
        public virtual Usuario Users { get; set; }
    }
}
