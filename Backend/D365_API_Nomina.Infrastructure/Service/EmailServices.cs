using D365_API_Nomina.Core.Application.Common.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace D365_API_Nomina.Infrastructure.Service
{
    public class EmailServices : IEmailServices
    {
        private readonly IApplicationDbContext _dbContext;

        public EmailServices(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private static string EmailBody(string temporaryPassword, string username)
        {
            return $"Saludos {username}! <br> <br>" +
             $"Se solicitó un cambio de contraseña a travéz de los servicios de la aplicación del portal Nómina DC365. <br> <br>" +
             $"Su clave temporal es: <b>{temporaryPassword}</b> <br> <br>" +
             $"Por favor tenga en cuenta que la anterior clave suministrada tiene una vigencia de 12 horas, " +
             $"si excede el tiempo de uso deberá solicitar una nueva.<br> <br>" +
             $"Si tienes algún inconveniente, favor de comunicar con nuestro Dpto. IT: <br>" +
             $"E-mail: soporte@dynacorp365.com <br> <br>" +
             $"Que tenga muy buen dia! <br> <br>" +
             $"Dynacorp 365 Team";
        }

        public async Task<string> SendEmail(string email, string temporaryPassword, string username)
        {
            try
            {
                var generalconfig = await _dbContext.GeneralConfigs.FirstOrDefaultAsync();

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(generalconfig.SMTP);
                mail.From = new MailAddress(generalconfig.Email);
                mail.To.Add(email);
                mail.Subject = "Solicitud de cambio de contraseña";
                mail.Body = EmailBody(temporaryPassword, username);
                mail.IsBodyHtml = true;
                SmtpServer.Port = int.Parse(generalconfig.SMTPPort);
                SmtpServer.Credentials = new System.Net.NetworkCredential(generalconfig.Email, generalconfig.EmailPassword);
                SmtpServer.EnableSsl = true;

                ////Function to return send mail process
                //SmtpServer.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

                
                SmtpServer.Send(mail);

                //Limpiar el mensaje
                mail.Dispose();

                return "200 Correo enviado correctamente";
            }
            catch (SmtpException)
            {
                return "404 Error al enviar el correo con la contraseña temporal, comuniquese con los administradores o intentelo más tarde.";
            }
        }

        public async Task<string> SendEmailFile(string email, string bodyemail, DateTime reportdate)
        {
            try
            {
                var generalconfig = await _dbContext.GeneralConfigs.FirstOrDefaultAsync();

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(generalconfig.SMTP);
                mail.From = new MailAddress(generalconfig.Email);
                mail.To.Add(email);
                mail.Subject = $"Recibo de pago de nómina ${reportdate}";
                mail.Body = bodyemail;
                mail.IsBodyHtml = true;
                SmtpServer.Port = int.Parse(generalconfig.SMTPPort);
                SmtpServer.Credentials = new System.Net.NetworkCredential(generalconfig.Email, generalconfig.EmailPassword);
                SmtpServer.EnableSsl = true;

                ////Function to return send mail process
                //SmtpServer.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);


                SmtpServer.Send(mail);

                //Limpiar el mensaje
                mail.Dispose();

                return "200 Correo enviado correctamente";
            }
            catch (SmtpException)
            {
                return "404 Error al enviar el correo con la contraseña temporal, comuniquese con los administradores o intentelo más tarde.";
            }
        }



        //private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        //{
        //    // Get the unique identifier for this asynchronous operation.
        //    String token = (string)e.UserState;

        //    if (e.Cancelled)
        //    {
        //        ResponseEmailSent = "Envio cancelado";
        //    }
        //    if (e.Error != null)
        //    {
        //        ResponseEmailSent = $"Error en el envío del correo - {e.Error}";
        //    }
        //    else
        //    {
        //        ResponseEmailSent = "Correo enviado éxitosamente";
        //    }
        //}
    }
}
