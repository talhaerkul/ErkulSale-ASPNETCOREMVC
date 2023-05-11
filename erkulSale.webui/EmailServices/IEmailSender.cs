using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace erkulSale.webui.EmailServices
{
    public interface IEmailSender
    {
        // stmp gmail,hotmail
        // api sendgrid (günlük 100 mail)
    
        Task SendEmailAsync(string email,string subject, string htmlMessage);
    
    
    
    }
}