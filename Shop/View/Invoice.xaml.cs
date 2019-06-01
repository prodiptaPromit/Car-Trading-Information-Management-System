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
using System.Windows.Shapes;
using Shop.Utilities;
using Shop.Model;

namespace Shop.View
{
    /// <summary>
    /// Interaction logic for Invoice.xaml
    /// </summary>
    public partial class Invoice : Window
    {
        private string engineNumber;
        DatabaseConnector connector;

        public Invoice(string engineNumber)
        {
            InitializeComponent();

            this.engineNumber = engineNumber;
            FillCustomerInformation();
            FillVehicleInformation();
            FillTransectionData();
            FillOthers();
        }

        private void FillCustomerInformation()
        {
            int id = GetCustomerId();
            connector = new DatabaseConnector();
            connector.ConnectToDatabase();
            connector.command = new System.Data.SqlClient.SqlCommand("select * from Customers where Contact=" + id.ToString(),connector.connection);
            connector.reader = connector.command.ExecuteReader();
            while(connector.reader.Read())
            {
                CustomerNameText.Text = "Customer: " + connector.reader["Name"].ToString();
                CustomerProfessionText.Text = "Profession: " + connector.reader["Profession"].ToString();
                Customer_eTINText.Text = "ETIN: " + connector.reader["eTIN"].ToString();
                CustomerNIdText.Text = "NID Number: " + connector.reader["NId"].ToString();
                CustomerContactText.Text = "Contact: " + connector.reader["Contact"].ToString();
            }
            connector.reader.Close();
            connector.CloseDatabaseConnection();
        }

        private int GetCustomerId()
        {
            //int id = GetModelId();
            connector = new DatabaseConnector();
            connector.ConnectToDatabase();

            connector.command = new System.Data.SqlClient.SqlCommand("select CustomerId from SalesRecord where EngineNumber='"+engineNumber+"'", connector.connection);
            return int.Parse(connector.command.ExecuteScalar().ToString());
        }

        private void FillVehicleInformation()
        {
            int id = GetModelId();
            connector = new DatabaseConnector();
            connector.ConnectToDatabase();
            connector.command = new System.Data.SqlClient.SqlCommand("select * from Model cross join Make where Make.Id=Model.MakeId and Model.Id=" + id.ToString(), connector.connection);
            connector.reader = connector.command.ExecuteReader();
            while (connector.reader.Read())
            {
                VehicleText.Text = "Vehicle: " + connector.reader["Make"].ToString()+" "+ connector.reader["Name"].ToString() + " " + connector.reader["Year"].ToString() + " " + connector.reader["Edition"].ToString() + ".";
                
            }
            connector.reader.Close();

            EngineNumberText.Text = "Engine Number: " + engineNumber;

            connector.command = new System.Data.SqlClient.SqlCommand("select Color from SalesRecord where EngineNumber='" + engineNumber + "'", connector.connection);
            ColorText.Text= "Color: "+connector.command.ExecuteScalar().ToString()+".";

            connector.command = new System.Data.SqlClient.SqlCommand("select SalePrice from SalesRecord where EngineNumber='" + engineNumber + "'", connector.connection);
            PriceText.Text = "Price Sold: "+connector.command.ExecuteScalar().ToString()+" BDT.";

            connector.CloseDatabaseConnection();
        }

        private int GetModelId()
        {
            connector = new DatabaseConnector();
            connector.ConnectToDatabase();

            connector.command = new System.Data.SqlClient.SqlCommand("select ModelId from SalesRecord where EngineNumber='" + engineNumber + "'", connector.connection);
            return int.Parse(connector.command.ExecuteScalar().ToString());
        }

        private List<TransectionHistory> transections;

        private void FillTransectionData()
        {          
            transections = new List<TransectionHistory>();

            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();

            connector.command = new System.Data.SqlClient.SqlCommand("select * from AccountsReceivables where Account='"+engineNumber+"'", connector.connection);
            connector.reader = connector.command.ExecuteReader();

            while (connector.reader.Read())
            {
                transections.Add(new TransectionHistory { Id=int.Parse(connector.reader["Id"].ToString()), EngineNumber = connector.reader["Account"].ToString(), Amount = connector.reader["Amount"].ToString(), PaymentMethod = connector.reader["PaymentMethod"].ToString(), Date = connector.reader["Date"].ToString() });
            }
            connector.reader.Close();
            connector.CloseDatabaseConnection();

            TransectionsDataGrid.ItemsSource = transections;
        }

        private void FillOthers()
        {
            connector = new DatabaseConnector();
            connector.ConnectToDatabase();

            connector.command = new System.Data.SqlClient.SqlCommand("select Paid from SalesRecord where EngineNumber='" + engineNumber + "'", connector.connection);
            TotalPaidText.Text = "Total Paid: " + connector.command.ExecuteScalar().ToString() + " BDT.";

            connector.command = new System.Data.SqlClient.SqlCommand("select Due from SalesRecord where EngineNumber='" + engineNumber + "'", connector.connection);
            DueText.Text = "Due: " + connector.command.ExecuteScalar().ToString() + " BDT.";
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            for(int i=0;i<2;i++)
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(InvoiceGrid, "printing in progress");
                }
            }
        }

        internal DatabaseConnector DatabaseConnector
        {
            get => default(DatabaseConnector);
            set
            {
            }
        }
    }
}
