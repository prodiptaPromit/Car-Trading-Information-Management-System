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

namespace Shop.View
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        string designation;
        public HomePage(string designation)
        {           
            InitializeComponent();
            Init();
            this.designation = designation;
        }

        private void Init()
        {
            ActivityFrame.Navigate(new Uri("View/HomeIntro.xaml", UriKind.Relative));
        }

        private void InventoryBtn_Click(object sender, RoutedEventArgs e)
        {
            ActivityFrame.Navigate(new Uri("View/InventoryPage.xaml", UriKind.Relative));
        }

        private void SaleCarBtn_Click(object sender, RoutedEventArgs e)
        {
            ActivityFrame.Navigate(new Uri("View/SalesPage.xaml", UriKind.Relative));
        }

        private void ManageStuffBtn_Click(object sender, RoutedEventArgs e)
        {
            ActivityFrame.Navigate(new Uri("View/ManageStuffPage.xaml", UriKind.Relative));
        }

        private void ExpendituresButton_Click(object sender, RoutedEventArgs e)
        {
            ActivityFrame.Navigate(new Uri("View/ExpendituresPage.xaml", UriKind.Relative));
        }

        private void OwnerAreaBtn_Click(object sender, RoutedEventArgs e)
        {
            if (designation == "Owner")
                ActivityFrame.Navigate(new Uri("View/OwnerArea.xaml", UriKind.Relative));
            else
                MessageBox.Show("You are not authorized to access this section.");
        }

        private void DashBtn_Click(object sender, RoutedEventArgs e)
        {
            if (designation == "Owner")
                ActivityFrame.Navigate(new Uri("View/DashBoard.xaml", UriKind.Relative));
            else
                MessageBox.Show("You are not authorized to access this section.");
        }

        private void SiteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (designation == "Owner")
                ActivityFrame.Navigate(new Uri("View/Support.xaml", UriKind.Relative));
            else
                MessageBox.Show("You are not authorized to access this section.");
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("View/LogInPage.xaml", UriKind.Relative));
        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e)
        {
            ActivityFrame.Navigate(new Uri("View/HomeIntro.xaml", UriKind.Relative));
        }
    }
}
