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
using Microsoft.Win32;
using Shop.ViewModel;

namespace Shop.View
{
    /// <summary>
    /// Interaction logic for InventoryPage.xaml
    /// </summary>
    public partial class InventoryPage : Page
    {
        VehicleInforManager infoManager;        
        private void Init()
        {
            infoManager = new VehicleInforManager();
            DisplayVehicleInfo();
            InitLists();
        }

        private void DisplayVehicleInfo()
        {
            MyGrid.ItemsSource = infoManager.GetVehicles();
        }

        private void InitLists()
        {
            MakeComboBox.ItemsSource = (from z in infoManager.Models()
                                        select z.Make).Distinct().ToList();
        }

        public InventoryPage()
        {
            InitializeComponent();
            Init();
        }

        private void MakeComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ModelComboBox.ItemsSource = (from z in infoManager.Models()
                                         where z.Make == MakeComboBox.Text
                                         select z.Model).Distinct().ToList();
        }

        private void ModelComboBox_DropDownClosed(object sender, EventArgs e)
        {
            EditionComboBox.ItemsSource = (from z in infoManager.Models()
                                           where z.Model == ModelComboBox.Text
                                           select z.Edition).Distinct().ToList();
        }

        private void AddCarBtn_Click(object sender, RoutedEventArgs e)
        {
            if(IsEngineNumerExisted(EngineNumberText.Text) ==false && IsChassisNumerExisted(ChassisNumberText.Text) ==false)
            {
                infoManager.InsertIntoInventory(ModelComboBox.Text, EditionComboBox.Text, YearComboBox.Text, EngineNumberText.Text, ChassisNumberText.Text, ColorComboBox.Text, ExpenseText.Text, PriceText.Text, DateText.Text, DescriptionText.Text, MilageText.Text + " Km");
                infoManager.UpdateAccounts(ExpenseText.Text, DateText.Text);
                infoManager.InsertImage(EngineNumberText.Text, @"G:\BUBT\4th Year\Finalization\Project\Car Trading Management System V4.0\Implementation\Shop\Shop\Assets\Images\bcar.png");
                Shop.Utilities.Notify notify = new Utilities.Notify("New vehicle added to inventory");
                notify.SendNotificationViaEmail(MakeComboBox.Text + " " + ModelComboBox.Text + " " + YearComboBox.Text + " " + EditionComboBox.Text + ", Engine Number " + EngineNumberText.Text + ", has been added successfully.");
                System.Windows.MessageBox.Show("Required car added into inventory and successfully notified to owner.");
                Refresh();
            }
            else
            {
                MessageBox.Show("Duplicated Engine or Chassis Number entered. Please correct your input and proceed.");
            }                        
        }

        private void Refresh()
        {
            MakeComboBox.Text = ModelComboBox.Text = EditionComboBox.Text = YearComboBox.Text = EngineNumberText.Text = ChassisNumberText.Text = ColorComboBox.Text = ExpenseText.Text = PriceText.Text = DateText.Text = DescriptionText.Text = MilageText.Text = "";
            FeildComboBox.Text = DataText.Text = UpdateEngineNumberText.Text = "";
            Init();
        }

        private void UpdateModelBtn_Click(object sender, RoutedEventArgs e)
        {
            if(FeildComboBox.Text=="Milage")
                infoManager.UpdateVehicleInfo(FeildComboBox.Text,DataText.Text+" Km",UpdateEngineNumberText.Text);
            else
                infoManager.UpdateVehicleInfo(FeildComboBox.Text, DataText.Text, UpdateEngineNumberText.Text);
            Refresh();
        }

        OpenFileDialog fdlg;

        private void ImgBrowseBtn_Click(object sender, RoutedEventArgs e)
        {

            fdlg = new OpenFileDialog();
            fdlg.Title = "C# Corner Open File Dialog";
            fdlg.InitialDirectory = @"c:\";
            fdlg.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;

            if (fdlg.ShowDialog() == true)
            {
                MessageBox.Show(fdlg.FileName + "\n" + fdlg.SafeFileName);
            }
            ImagePathText.Text = fdlg.FileName;
        }

        private void EditionComboBox_DropDownClosed(object sender, EventArgs e)
        {
            YearComboBox.ItemsSource = infoManager.RetriveYears(ModelComboBox.Text, EditionComboBox.Text);
        }

        private void AddImageBtn_Click(object sender, RoutedEventArgs e)
        {
            infoManager.InsertImage(IEngineNumberText.Text,ImagePathText.Text);     
        }

        private void ExpenseText_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void DataText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(FeildComboBox.Text=="Purchase" || FeildComboBox.Text=="SalePrice" || FeildComboBox.Text == "Milage")
            {
                if (IsNumber(e.Text) == false)
                {
                    e.Handled = true;
                }
            }
        }

        private bool IsEngineNumerExisted(string engineNumber)
        {
            var result = false;
            foreach (var x in infoManager.GetVehicles())
            {
                if (x.EngineNumber == engineNumber)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private bool IsChassisNumerExisted(string chassisNumber)
        {
            var result = false;
            foreach (var x in infoManager.GetVehicles())
            {
                if (x.ChassisNumber == chassisNumber)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        internal Utilities.Notify Notify
        {
            get => default(Utilities.Notify);
            set
            {
            }
        }

        internal VehicleInforManager VehicleInforManager
        {
            get => default(VehicleInforManager);
            set
            {
            }
        }
    }
}
