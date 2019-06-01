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

namespace Shop.View
{
    /// <summary>
    /// Interaction logic for IncomeStatement.xaml
    /// </summary>
    public partial class IncomeStatement : Window
    {
        DatabaseConnector connector;
        string year;
        int netRevenue, netExpense;

        public IncomeStatement(string year)
        {
            InitializeComponent();
            this.year = year;
            GenerateIncomeStatement();
        }

        internal DatabaseConnector DatabaseConnector
        {
            get => default(DatabaseConnector);
            set
            {
            }
        }

        private void RetriveData()
        {
            connector = new DatabaseConnector();
            connector.ConnectToDatabase();

            connector.command = new System.Data.SqlClient.SqlCommand("select SUM(Expense) from Accounts where Year="+year, connector.connection);
            netExpense = int.Parse(connector.command.ExecuteScalar().ToString());

            connector.command = new System.Data.SqlClient.SqlCommand("select SUM(Revenue) from Accounts where Year=" + year, connector.connection);
            netRevenue = int.Parse(connector.command.ExecuteScalar().ToString());

            connector.CloseDatabaseConnection();
        }

        private void FillData()
        {
            DateText.Text = DateTime.Now.ToString();
            NetExpenseText.Text += " " + year + " stands \t" + netExpense.ToString() + " BDT";
            NetRevenueText.Text += " " + year + " stands \t" + netRevenue.ToString() + " BDT";
            NetIncomeText.Text += " \t" + (netRevenue - netExpense).ToString() + " BDT";
            if ((netRevenue - netExpense) > 0)
                NetIncomeStatementText.Text = "Net profit stands " + (netRevenue - netExpense).ToString() + " BDT";
            else
                NetIncomeStatementText.Text = "Net loss stands " + (netRevenue - netExpense).ToString() + " BDT";
        }

        private void GenerateIncomeStatement()
        {
            RetriveData();
            FillData();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(IncomeStatementGrid, "printing in progress");
            }
        }
    }
}
