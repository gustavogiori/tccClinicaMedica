using Giori_Consul.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Giori_Consul.Models
{
    public class EnvioEmail
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Display(Name = "Consulta")]
        public int IDConsulta { get; set; }
        public string Titulo { get; set; }
        [Display(Name = "Email")]
        public string EmailDestino { get; set; }

        [Display(Name = "Conteúdo")]
        public string Texto { get; set; }

        [Display(Name = "Enviado?")]
        public bool Enviado { get; set; }

        [IgnoreListFilter]
        [ForeignKey("IDConsulta")]
        public Consulta Consulta { get; set; }
    }
}