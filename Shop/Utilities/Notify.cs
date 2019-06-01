using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Utilities
{
    class Notify
    {
        private string subject;
        public Notify(string subject)
        {
            this.subject = subject;
        }

        internal SendMail SendMail
        {
            get => default(SendMail);
            set
            {
            }
        }

        internal SendMail SendMail1
        {
            get => default(SendMail);
            set
            {
            }
        }

        internal SendMail SendMail2
        {
            get => default(SendMail);
            set
            {
            }
        }

        internal SendMail SendMail3
        {
            get => default(SendMail);
            set
            {
            }
        }

        public void SendNotificationViaEmail(string message)
        {
            SendMail sendMail = new SendMail(subject);
            sendMail.Send(message);   
        }
    }
}
