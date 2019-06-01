using System;
using System.Collections.Generic;
using System.Data;
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
using Microsoft.Win32;
using Shop.ViewModel;

namespace Shop.View
{
    /// <summary>
    /// Interaction logic for ManageStuffPage.xaml
    /// </summary>
    public partial class ManageStuffPage : Page
    {
        StuffInfoManager stuffInfoManager;

        public ManageStuffPage()
        {
            InitializeComponent();
            Init();
        }

        internal StuffInfoManager StuffInfoManager
        {
            get => default(StuffInfoManager);
            set
            {
            }
        }

        private void Init()
        {
            stuffInfoManager = new StuffInfoManager();
            StuffGrid.ItemsSource = stuffInfoManager.Stuffs();
        }

        private void ClearFields()
        {
            IdText.Text = NameText.Text = DesignationComboBox.Text = SalaryText.Text = NIdText.Text = AddressText.Text = ContactText.Text = "";
            FieldComboBox.Text = NewDataText.Text = SIdText.Text = "";
            RStuffId.Text = "";
        }

        private void Refresh()
        {
            ClearFields();
            Init();
        }

        private void AddStuffBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //StuffImage.Source = new BitmapImage(new Uri(@"G:\BUBT\4th Year\Finalization\Project\Car Trading Management System V4.0\Implementation\Shop\Shop\Assets\Images\Stuff.png"));                
                stuffInfoManager.InsertStuff(IdText.Text, NameText.Text, DesignationComboBox.Text, SalaryText.Text, NIdText.Text, AddressText.Text, ContactText.Text);
                Refresh();
            }
            catch(Exception exception)
            {
                MessageBox.Show("Something went wrong. \n" + exception.Message);
            }
        }        

        private void UpdateStuffButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                stuffInfoManager.UpdateStuffInfo(FieldComboBox.Text, NewDataText.Text, SIdText.Text);
                MessageBox.Show("Required information updated successfully.");
                Refresh();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Something went wrong. \n" + exception.Message);
            }
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                stuffInfoManager.RemoveStuff(RStuffId.Text);
                MessageBox.Show("Required stuff deleted successfully.");
                Refresh();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Something went wrong. \n" + exception.Message);
            }
        }

        private void EmployeePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsNumber(e.Text) == false)
            {
                e.Handled = true;
            }
        }

        private bool IsNumber(string Text)
        {
            int output;
            return int.TryParse(Text, out output);
        }

        private void NewDataText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (FieldComboBox.Text == "Salary" || FieldComboBox.Text == "NID" || FieldComboBox.Text == "Contact")
            {
                if (IsNumber(e.Text) == false)
                {
                    e.Handled = true;
                }
            }
        }
    }
}
