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
    /// Interaction logic for LogInPage.xaml
    /// </summary>
    public partial class LogInPage : Page
    {
        DatabaseConnector connector;
        string designation;

        public LogInPage()
        {
            InitializeComponent();
            connector = new DatabaseConnector();
        }

        internal DatabaseConnector DatabaseConnector
        {
            get => default(DatabaseConnector);
            set
            {
            }
        }

        public HomePage HomePage
        {
            get => default(HomePage);
            set
            {
            }
        }

        private void LogInBtn_Click(object sender, RoutedEventArgs e)
        {
            if(ValidUser(IdTextBox.Text,PwdTextBox.Password))
            {
                //this.NavigationService.Navigate(new Uri("View/HomePage.xaml", UriKind.Relative));
                this.NavigationService.Navigate(new HomePage(designation));
            }
            else
            {
                MessageBox.Show("Invalid Credentials");
            }
        }

        private bool ValidUser(string id, string password)
        {
            connector.ConnectToDatabase();
            connector.command = new System.Data.SqlClient.SqlCommand("select * from Users", connector.connection);
            connector.reader = connector.command.ExecuteReader();
            while(connector.reader.Read())
            {
                if(connector.reader["Account"].ToString()==id && connector.reader["Password"].ToString()==password)
                {
                    designation = connector.reader["Designation"].ToString();
                    return true;
                }
            }
            connector.CloseDatabaseConnection();
            return false;
        }
    }
}
