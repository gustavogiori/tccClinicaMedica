using Giori_Consul.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Giori_Consul.Models
{
    public class Consulta
    {
        [Key]
        [Display(Name = "Código")]
        public int IDConsulta { get; set; }

        [IgnoreListFilter]
        [Display(Name = "Paciente")]
        public int IDPaciente { get; set; }
        [IgnoreListFilter]
        [Display(Name = "Médico")]
        public int IDMedico { get; set; }

        [Required(ErrorMessage = "O campo data é obrigatório")]
        [DataType(DataType.Date, ErrorMessage = "Insira a data no formato dd/mm/aaaa")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Data { get; set; }
        [Required(ErrorMessage = "O campo horário é obrigatório")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm tt}")]
        [DataType(DataType.Time)]
        [Display(Name = "Horário")]
        [IgnoreListFilter]
        public DateTime Time { get; set; }

        [NotMapped]
        [IgnoreListFilter]
        public bool EnviaEmail { get; set; }
        [NotMapped]
        [IgnoreListFilter]
        public string EmailPaciente { get; set; }
        public bool Cancelada { get; set; }
        [IgnoreListFilter]
        public virtual Paciente Paciente { get; set; }
        [IgnoreListFilter]
        public virtual Medico Medico { get; set; }


    }
}