using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Shop.Utilities
{
    class SendMail
    {
        MailMessage mailMessage;
        SmtpClient smtpClient;
        private string subject, smtp, receiver, sender, senderPassword;
        private int port;


        public SendMail(string subject)
        {
            this.subject = subject;
            Init();
        }

        private void Init()
        {
            smtp = "smtp.outlook.com";
            sender = "prodipta.promit@outlook.com";
            receiver = "prodipta.promit@gmail.com";
            senderPassword = "WarriorProdipta";

            port = 587;
            mailMessage = new MailMessage();
            smtpClient = new SmtpClient(smtp);
        }

        public void Send(string message)
        {
            mailMessage.From = new MailAddress(sender);
            mailMessage.To.Add(receiver);
            mailMessage.Subject = subject;
            mailMessage.Body = message;

            smtpClient.Port = port;
            smtpClient.Credentials = new System.Net.NetworkCredential(sender, senderPassword);
            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(mailMessage);               
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
