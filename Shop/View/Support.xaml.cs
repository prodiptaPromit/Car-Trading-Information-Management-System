using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Shop.Utilities;

namespace Shop.View
{
    /// <summary>
    /// Interaction logic for Support.xaml
    /// </summary>
    public partial class Support : Page
    {
        SendMail sendMail;
        public Support()
        {
            InitializeComponent();           
        }

        internal Notify Notify
        {
            get => default(Notify);
            set
            {
            }
        }

        private void AskSupportButton_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(IssueText.Document.ContentStart, IssueText.Document.ContentEnd);
            string message = textRange.Text;

            sendMail = new SendMail("Seeking for support.");
            sendMail.Send(message);
        }
    }
}
