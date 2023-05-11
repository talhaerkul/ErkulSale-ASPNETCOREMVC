using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace erkulSale.webui.Models
{
    public class AlertMessage
    {
        public AlertMessage(string title, string message, string alertType)
        {
            Title = title;
            Message = message;
            AlertType = alertType;
        }

        public string Title { get; set; }
        public string Message { get; set; }
        public string AlertType { get; set; }
    }
}