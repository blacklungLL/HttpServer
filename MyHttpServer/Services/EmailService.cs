using System.Net;
using System.Net.Mail;
using HttpServerLibrary.Configurations;

namespace MyHTTPServer.services
{
    internal class EmailService : IEmailService
    {
        public void SendEmail(string email, string subject, string message, AppConfig config)
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress(config.NetworkCredential!.UserName);
            // кому отправляем
            MailAddress to = new MailAddress(email);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = subject;
            // текст письма
            m.Body = message;
            // письмо представляет код html
            m.IsBodyHtml = true;
            m.Attachments.Add(new Attachment(@"/Users/overmann/Desktop/MyHttpServer_19_11_24/MyHttpServer/public/images/hand.png"));
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = config.SmtpClient;
            // логин и пароль
            smtp.Credentials = config.NetworkCredential;
            smtp.EnableSsl = true;
            smtp.Send(m);
        }
    }
}