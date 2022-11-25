using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SistemaGSG.Email
{
    internal class email
    {

        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public DateTime Created { get; set; }
        public string body {get;set;}

        private void EnviaEmail()
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(FromEmail);
            mail.To.Add(ToEmail);
            mail.Subject = Subject;
            mail.Body = body;
            mail.Attachments.Add(new Attachment(@"C:\ArquivosSAP\xmlDownload\25221110144451000156550010000012111004640320-procNfe.xml"));
        }
    }
}
