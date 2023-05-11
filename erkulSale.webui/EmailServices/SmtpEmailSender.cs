using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace erkulSale.webui.EmailServices
{
    public class SmtpEmailSender : IEmailSender
    {
        private string Host;
        private int Port;
        private bool EnableSSL;
        private string Username;
        private string Password;

        public SmtpEmailSender(string host,int port,bool enableSSL,string username,string password)
        {
            Host = host;
            Port = port;
            EnableSSL = enableSSL;
            Username = username;
            Password = password;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(this.Host,this.Port){
                Credentials = new NetworkCredential(Username,Password),
                EnableSsl = this.EnableSSL
            };
            return client.SendMailAsync(
                new MailMessage(this.Username,email,subject,htmlMessage){IsBodyHtml = true}
            );
        }
    }
}