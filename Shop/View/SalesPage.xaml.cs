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
using Shop.ViewModel;

namespace Shop.View
{
    /// <summary>
    /// Interaction logic for SalesPage.xaml
    /// </summary>
    public partial class SalesPage : Page
    {
        private VehicleInforManager vehicileInfo;
        private CustomerInfoManager customerInfo;
        private SalesInfoManager salesInfo;

        public SalesPage()
        {
            InitializeComponent();
            Init();
        }

        internal Model.VehicleInfo VehicleInfo
        {
            get => default(Model.VehicleInfo);
            set
            {
            }
        }

        internal Model.CustomerInfo CustomerInfo
        {
            get => default(Model.CustomerInfo);
            set
            {
            }
        }

        internal SalesInfoManager SalesInfoManager
        {
            get => default(SalesInfoManager);
            set
            {
            }
        }

        internal Utilities.Notify Notify1
        {
            get => default(Utilities.Notify);
            set
            {
            }
        }

        public Invoice Invoice
        {
            get => default(Invoice);
            set
            {
            }
        }

        private void Init()
        {
            vehicileInfo = new VehicleInforManager();
            InventoryData.ItemsSource = vehicileInfo.GetVehicles();

            customerInfo = new CustomerInfoManager();
            CustomerData.ItemsSource = customerInfo.GetCustomers();

            salesInfo = new SalesInfoManager();
            SalesData.ItemsSource = salesInfo.GetSalesData();
        }

        private void Refresh()
        {
            ClearFields();
            Init();
        }

        private void ClearFields()
        {
            NameText.Text = ProfessionText.Text = NIdText.Text = eTINText.Text = ContactText.Text = "";
            SalePriceText.Text = PaidText.Text = EngineNumberText.Text  = CustomerIdText.Text = DatePick.Text = PaymentMethodInitialText.Text = "";
            AccountNumberText.Text = AmountPaidText.Text = ARDatePick.Text = PaymentMethodText.Text = "";
            DEngineNumberText.Text = "";
        }

        private void AddCustomerBtn_Click(object sender, RoutedEventArgs e)
        {   
            if(IsNIDExisted(NIdText.Text) == false && Is_eTINExisted(eTINText.Text) == false && IsContactNumberExisted(ContactText.Text) == false)
            {
                salesInfo.InsertCustomer(NameText.Text, ProfessionText.Text, NIdText.Text, eTINText.Text, ContactText.Text);
                Refresh();
            }
            else
            {
                MessageBox.Show("Customer information seems existed. Please insert valid information.");
            }
        }

        private void SaleCarBtn_Click(object sender, RoutedEventArgs e)
        {
            if(IsEngineNumerExisted(EngineNumberText.Text))
            {
                salesInfo.UpdateSalesRecord(SalePriceText.Text, PaidText.Text, EngineNumberText.Text, CustomerIdText.Text, DatePick.Text);
                salesInfo.AccountsRecievableInitials(EngineNumberText.Text, PaidText.Text, DatePick.Text, PaymentMethodInitialText.Text);
                Notify(PaidText.Text, PaymentMethodInitialText.Text, EngineNumberText.Text, "Vehicle sold. ");
                PrintInvoice(EngineNumberText.Text);
                Refresh();
            }
            else
            {
                MessageBox.Show("Invalid engine number entered.");
            }
        }

        private void DeliverBtn_Click(object sender, RoutedEventArgs e)
        {
            salesInfo.DeliverUpdate(DEngineNumberText.Text);
            Refresh();
        }

        private void AccRcvdBtn_Click(object sender, RoutedEventArgs e)
        {
            salesInfo.AccountsReceivableUpdate(AccountNumberText.Text, AmountPaidText.Text, ARDatePick.Text, PaymentMethodText.Text);
            PrintInvoice(AccountNumberText.Text);
            Notify(AmountPaidText.Text, PaymentMethodText.Text, AccountNumberText.Text);
            Refresh();
        }

        private void AddCarBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void PrintInvoice(string engineNumber)
        {
            Invoice invoice = new Invoice(engineNumber);
            invoice.Show();
        }

        private void Notify(string amount, string paymentMethod, string account, string sales="")
        {
            Shop.Utilities.Notify notify = new Utilities.Notify("Sales Update");
            notify.SendNotificationViaEmail(sales+amount + " taka paid in " + paymentMethod + ", for the account "+account+".");
        }

        private void eTINText_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private bool IsEngineNumerExisted(string engineNumber)
        {
            var result = false;
            foreach (var x in vehicileInfo.GetVehicles())
            {
                if (x.EngineNumber == engineNumber)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private bool IsNIDExisted(string nid)
        {
            var result = false;
            foreach (var x in customerInfo.GetCustomers())
            {
                if (x.NId==nid)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private bool Is_eTINExisted(string etin)
        {
            var result = false;
            foreach (var x in customerInfo.GetCustomers())
            {
                if (x.eTIN == etin)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private bool IsContactNumberExisted(string contact)
        {
            var result = false;
            foreach (var x in customerInfo.GetCustomers())
            {
                if (x.Contact == contact)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private void InvoiceButton_Click(object sender, RoutedEventArgs e)
        {
            PrintInvoice(IEngineNumberText.Text);
        }
    }
}
