using Giori_Consul.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace Giori_Consul.Services
{
    public class EmailService
    {
        SqlDbContext db = new SqlDbContext();
        public bool EnviaEmailConsulta(Consulta consulta)
        {
            EnvioEmail email = new EnvioEmail();
            string assunto = "Confirmação de consulta";
            string message = string.Format("Olá senhor(a) {0} você agora tem uma consulta agendada para o dia {1} as {2} horas. Não se esqueça.", consulta.Paciente.Nome, consulta.Data.ToShortDateString(), consulta.Time.ToShortTimeString());
            email.Texto = message;
            email.Titulo = assunto;
            email.IDConsulta = consulta.IDConsulta;
            email.EmailDestino = consulta.EmailPaciente;

            return EnviaEmail(email);
        }

        public bool EnviaEmail(EnvioEmail email)
        {
            bool enviou = EnviaEmail(email.EmailDestino, email.Titulo, email.Texto);
            email.Enviado = enviou;
            db.EnvioEmails.Add(email);
            db.SaveChanges();
            return enviou;
        }
        public bool EnviaEmail(string email, string subject, string message)
        {
            try
            {
                MailMessage objEmail = new MailMessage() { IsBodyHtml = true, Subject = subject, Body = message, Priority = MailPriority.Normal };

                objEmail.From = new MailAddress("gioriconsul@gmail.com");
                objEmail.To.Add(email);
                objEmail.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");
                objEmail.BodyEncoding = System.Text.Encoding.UTF8;

                SmtpClient objSmtp = new SmtpClient() { EnableSsl = true, Port = 587 };
               
                objSmtp.Host = "smtp.gmail.com";
                objSmtp.UseDefaultCredentials = false;
                objSmtp.Credentials = new NetworkCredential("gioriconsul@gmail.com", "tccgustavo");
                objSmtp.Send(objEmail);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
