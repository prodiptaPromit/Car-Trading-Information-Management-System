using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Shop.Model;
using Shop.Utilities;

namespace Shop.View
{
    /// <summary>
    /// Interaction logic for DashBoard.xaml
    /// </summary>
    public partial class DashBoard : Page
    {
        DatabaseConnector databaseConnector;
        int countMonths=0;

        public DashBoard()
        {
            InitializeComponent();
            MonthlyIncomeStatement();
            CurrentYearProfitGrowth();
            CustomerDemographicChartByProfession();
            VehicleSalesDataVisualizationByBodyType();
        }

        internal DatabaseConnector DatabaseConnector
        {
            get => default(DatabaseConnector);
            set
            {
            }
        }

        public IncomeStatement IncomeStatement
        {
            get => default(IncomeStatement);
            set
            {
            }
        }

        private void CurrentYearProfitGrowth()
        {
            string year = DateTime.Now.Year.ToString();
            List<KeyValuePair<string, int>> accounts = new List<KeyValuePair<string, int>>();

            databaseConnector = new DatabaseConnector();
            databaseConnector.ConnectToDatabase();

            databaseConnector.command = new System.Data.SqlClient.SqlCommand("select * from Accounts Order by Id Asc", databaseConnector.connection);
            databaseConnector.reader = databaseConnector.command.ExecuteReader();
            var item = new KeyValuePair<string, int>[] { };
            while (databaseConnector.reader.Read())
            {
                accounts.Add(new KeyValuePair<string, int>(databaseConnector.reader["Month"].ToString()+ databaseConnector.reader["Year"].ToString(), int.Parse(databaseConnector.reader["Profit"].ToString()) ));
                countMonths++;
            }
            databaseConnector.CloseDatabaseConnection();
            ((LineSeries)mcChart.Series[0]).ItemsSource = accounts;
        }

        private void CustomerDemographicChartByProfession()
        {
            ((PieSeries)mccChart.Series[0]).ItemsSource = new KeyValuePair<string, int>[] {
                new KeyValuePair<string, int>("Politician", GetCustomerProfessionQuantity("Politician")),  
            new KeyValuePair<string, int>("Businessman", GetCustomerProfessionQuantity("Businessman")),  
            new KeyValuePair<string, int>("Govt. Service", GetCustomerProfessionQuantity("Govt. Service")),  
            new KeyValuePair<string, int>("Service Holder", GetCustomerProfessionQuantity("Service Holder")),  
            new KeyValuePair<string, int>("Engineer", GetCustomerProfessionQuantity("Engineer")),  
            new KeyValuePair<string, int>("Doctor", GetCustomerProfessionQuantity("Doctor"))
            };
        }

        private void VehicleSalesDataVisualizationByBodyType()
        {
            ((PieSeries)mcc1Chart.Series[0]).ItemsSource = new KeyValuePair<string, int>[] {
                new KeyValuePair<string, int>("SUV", GetVehicleBodyTypeQuantity("SUV")),
            new KeyValuePair<string, int>("Sedan", GetVehicleBodyTypeQuantity("Sedan")),
            new KeyValuePair<string, int>("Hatch Back", GetVehicleBodyTypeQuantity("Hatch Back")),
            new KeyValuePair<string, int>("Mini Van", GetVehicleBodyTypeQuantity("Mini Van")),
            new KeyValuePair<string, int>("Hybrid", GetVehicleBodyTypeQuantity("Hybrid")),
            new KeyValuePair<string, int>("MPV", GetVehicleBodyTypeQuantity("MPV")),
            new KeyValuePair<string, int>("LUV", GetVehicleBodyTypeQuantity("LUV")),
            new KeyValuePair<string, int>("Others", GetVehicleBodyTypeQuantity("Others"))
            };
        }

        private int GetCustomerProfessionQuantity(string profession)
        {
            databaseConnector = new DatabaseConnector();
            databaseConnector.ConnectToDatabase();

            databaseConnector.command = new System.Data.SqlClient.SqlCommand("SELECT COUNT(Id) FROM Customers WHERE Profession = '"+profession+"'", databaseConnector.connection);
            int count = int.Parse(databaseConnector.command.ExecuteScalar().ToString());
            databaseConnector.CloseDatabaseConnection();
            return count;
        }

        private int GetVehicleBodyTypeQuantity(string bodyType)
        {
            databaseConnector = new DatabaseConnector();
            databaseConnector.ConnectToDatabase();

            databaseConnector.command = new System.Data.SqlClient.SqlCommand("select count(Model.BodyType) from Model cross join SalesRecord where Model.Id=SalesRecord.ModelId and Model.BodyType='"+bodyType+"'", databaseConnector.connection);
            int count = int.Parse(databaseConnector.command.ExecuteScalar().ToString());
            databaseConnector.CloseDatabaseConnection();
            return count;
        }

        private void IncomeStatementButton_Click(object sender, RoutedEventArgs e)
        {
            IncomeStatement incomeStatement = new IncomeStatement(DateTime.Now.Year.ToString());
            incomeStatement.Show();
        }

        private void MonthlyIncomeStatement()
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();
            databaseConnector.ConnectToDatabase();

            databaseConnector.command = new System.Data.SqlClient.SqlCommand("select * from Accounts", databaseConnector.connection);
            databaseConnector.reader = databaseConnector.command.ExecuteReader();
            while(databaseConnector.reader.Read())
            {
                var month = databaseConnector.reader["Month"].ToString();
                var year = databaseConnector.reader["Year"].ToString();
                NetRevenueText.Text = "Net Revenue of " + month + " " + year + " is " + databaseConnector.reader["Revenue"].ToString() + " BDT";
                NetExpenseText.Text = "Net Expense of " + month + " " + year + " is " + databaseConnector.reader["Expense"].ToString() + " BDT";
                NetProfitText.Text = "Net Profit of " + month + " " + year + " is " + databaseConnector.reader["Profit"].ToString() + " BDT";
                break;
            }
            databaseConnector.CloseDatabaseConnection();
        }
    }
}
