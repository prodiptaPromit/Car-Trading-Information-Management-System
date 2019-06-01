using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using Shop.Utilities;

namespace Shop.View
{
    /// <summary>
    /// Interaction logic for OwnerArea.xaml
    /// </summary>
    public partial class OwnerArea : Page
    {
        MakeInfoManager makeInfoManager;
        ModelInfoManager modelInfoManager;
        SalesInfoManager salesInfoManager;
        public OwnerArea()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            FillMakeData();
            FillModelData();
            FillUsersData();
            FillCustomerData();
            FillExpenseData();
            FillAccountsReceivableData();
            FillSalesData();
        }

        private void FillSalesData()
        {
            salesInfoManager = new SalesInfoManager();
            SalesData.ItemsSource = salesInfoManager.GetSalesData();
        }

        private void EditMakeButton_Click(object sender, RoutedEventArgs e)
        {
            makeInfoManager.connector.ConnectToDatabase();
            SqlCommandBuilder builder = new SqlCommandBuilder(makeInfoManager.dataAdapter);
            makeInfoManager.dataAdapter.UpdateCommand = builder.GetUpdateCommand();
            makeInfoManager.dataAdapter.Update(makeInfoManager.dataTable);
            makeInfoManager.connector.CloseDatabaseConnection();
            MessageBox.Show("Required Make edited successfully.");
            FillMakeData();
        }

        private void DeleteMakeButton_Click(object sender, RoutedEventArgs e)
        {
            var row = (DataRowView)MakeData.SelectedItem;
            var id = row.Row["id"].ToString();            
            try
            {
                makeInfoManager.RemoveMake(id);
                MessageBox.Show("Required Make deleted successfully.");
            }
            catch (Exception exception)
            {
                MessageBox.Show("Something went wrong. \n" + exception.Message);
            }
            FillMakeData();
        }

        private void FillMakeData()
        {
            makeInfoManager = new MakeInfoManager();
            makeInfoManager.RetriveMakes();
            MakeData.ItemsSource = makeInfoManager.dataTable.DefaultView;
        }

        private void ClearMakePageFields()
        {
            MakeText.Text = CountryText.Text = "";
        }

        private void AddMakeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                makeInfoManager.InsertMake(MakeText.Text, CountryText.Text);
                MessageBox.Show("Required data inserted successfully");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            FillMakeData();
            ClearMakePageFields();
        }

        private void FillModelData()
        {
            modelInfoManager = new ModelInfoManager();
            modelInfoManager.RetriveModels();
            ModelData.ItemsSource = modelInfoManager.dataTable.DefaultView;
        }

        private void AddModelBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                modelInfoManager.InsertModel(MakeIDText.Text, ModelText.Text, YearText.Text, EditionText.Text, EngineText.Text, BodyComboBox.Text, TransmissionComboBox.Text);
                MessageBox.Show("Required data inserted successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            FillModelData();
        }

        private void EditModelButton_Click(object sender, RoutedEventArgs e)
        {
            modelInfoManager.connector.ConnectToDatabase();
            SqlCommandBuilder builder = new SqlCommandBuilder(modelInfoManager.dataAdapter);
            modelInfoManager.dataAdapter.UpdateCommand = builder.GetUpdateCommand();
            modelInfoManager.dataAdapter.Update(modelInfoManager.dataTable);
            modelInfoManager.connector.CloseDatabaseConnection();
            MessageBox.Show("Required model edited successfully.");
            FillModelData();
        }

        private void DeleteModelButton_Click(object sender, RoutedEventArgs e)
        {
            var row = (DataRowView)ModelData.SelectedItem;
            var id = row.Row["id"].ToString();
            try
            {
                modelInfoManager.RemoveModel (id);
                MessageBox.Show("Required Make deleted successfully.");
            }
            catch (Exception exception)
            {
                MessageBox.Show("Something went wrong. \n" + exception.Message);
            }
            FillModelData();
        }

        SqlDataAdapter userDataAdapter;
        DataTable userDataTable;

        private void FillUsersData()
        {
            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();
            connector.command = new SqlCommand("select * from Users",connector.connection);
            userDataAdapter = new SqlDataAdapter(connector.command);
            userDataTable = new DataTable();
            userDataAdapter.Fill(userDataTable);
            UserData.ItemsSource = userDataTable.DefaultView;
            connector.CloseDatabaseConnection();
        }

        private void ClearUserPageFields()
        {
            AccountIdText.Text = DesignationText.Text = PasswordText.Password = "";
        }

        private void AddUserBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InsertUsers(AccountIdText.Text, DesignationText.Text, PasswordText.Password);
                MessageBox.Show("Required data inserted successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            FillUsersData();
            ClearUserPageFields();
        }

        private void InsertUsers(string account, string designation, string password)
        {
            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();
            connector.command = new SqlCommand("insert into Users values('" + account + "','" + designation + "','"+password+"')", connector.connection);
            connector.command.ExecuteNonQuery();
            connector.CloseDatabaseConnection();
        }

        private void EditUsersButton_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();
            SqlCommandBuilder builder = new SqlCommandBuilder(userDataAdapter);
            userDataAdapter.UpdateCommand = builder.GetUpdateCommand();
            userDataAdapter.Update(userDataTable);
            connector.CloseDatabaseConnection();
            MessageBox.Show("Required user edited successfully.");
            FillUsersData();
        }

        private void DeleteUsersButton_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();
            var row = (DataRowView)UserData.SelectedItem;
            var id = row.Row["id"].ToString();
            connector.command = new System.Data.SqlClient.SqlCommand("delete from Users where Id=" + id, connector.connection);
            connector.command.ExecuteNonQuery();
            connector.CloseDatabaseConnection();
            MessageBox.Show("Required user edited successfully.");
            FillUsersData();
        }

        SqlDataAdapter customerDataAdapter;
        DataTable customerDataTable;

        private void FillCustomerData()
        {
            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();
            connector.command = new SqlCommand("select * from Customers Order by Id desc", connector.connection);
            customerDataAdapter = new SqlDataAdapter(connector.command);
            customerDataTable = new DataTable();
            customerDataAdapter.Fill(customerDataTable);
            CustomerData.ItemsSource = customerDataTable.DefaultView;
            connector.CloseDatabaseConnection();
        }

        private void EditCustomersButton_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();
            SqlCommandBuilder builder = new SqlCommandBuilder(customerDataAdapter);
            customerDataAdapter.UpdateCommand = builder.GetUpdateCommand();
            customerDataAdapter.Update(customerDataTable);
            connector.CloseDatabaseConnection();
            MessageBox.Show("Required customer edited successfully.");
            FillCustomerData();
        }

        SqlDataAdapter expenseDataAdapter;
        DataTable expenseDataTable;

        private void FillExpenseData()
        {
            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();
            connector.command = new SqlCommand("select * from Expenses Order by Date desc", connector.connection);
            expenseDataAdapter = new SqlDataAdapter(connector.command);
            expenseDataTable = new DataTable();
            expenseDataAdapter.Fill(expenseDataTable);
            ExpenseData.ItemsSource = expenseDataTable.DefaultView;
            connector.CloseDatabaseConnection();
        }

        private void EditExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();
            SqlCommandBuilder builder = new SqlCommandBuilder(expenseDataAdapter);
            expenseDataAdapter.UpdateCommand = builder.GetUpdateCommand();
            expenseDataAdapter.Update(expenseDataTable);
            connector.CloseDatabaseConnection();
            MessageBox.Show("Required expense information edited successfully.");
            FillExpenseData();
        }

        SqlDataAdapter arDataAdapter;
        DataTable arDataTable;

        private void FillAccountsReceivableData()
        {
            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();
            connector.command = new SqlCommand("select * from AccountsReceivables Order by Date desc", connector.connection);
            arDataAdapter = new SqlDataAdapter(connector.command);
            arDataTable = new DataTable();
            arDataAdapter.Fill(arDataTable);
            AccountsRecievablesData.ItemsSource = arDataTable.DefaultView;
            connector.CloseDatabaseConnection();
        }

        private void EditARButton_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();
            SqlCommandBuilder builder = new SqlCommandBuilder(arDataAdapter);
            arDataAdapter.UpdateCommand = builder.GetUpdateCommand();
            arDataAdapter.Update(arDataTable);
            connector.CloseDatabaseConnection();
            MessageBox.Show("Required expense information edited successfully.");
            FillAccountsReceivableData();
        }

        private void UpdateAccountsBtn_Click(object sender, RoutedEventArgs e)
        {
            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();
            string[] dpText = dp.ToString().Split(' ');
            string[] date = dpText[0].Split('/');
            int day = int.Parse(date[1]);
            string month = date[0];
            string year = date[2];

            Int32 revenue = 0;
            Int32 expense = 0;
            int days = 0;

            if (month == "1" || month == "3" || month == "5" || month == "7" || month == "8" || month == "10" || month == "12")
                days = -30;
            else if (month == "2")
                days = -27;
            else
                days = -29;
            string date1 = month + "/" + (day + days).ToString() + "/" + year;
            string date2 = month + "/" + day.ToString() + "/" + year;

            connector.command = new System.Data.SqlClient.SqlCommand("select SUM(SalePrice) from SalesRecord where Date between '" + date1 + "' and '" + date2 + "'", connector.connection);
            revenue = Convert.ToInt32(connector.command.ExecuteScalar().ToString());
            MessageBox.Show(revenue.ToString());

            connector.command = new System.Data.SqlClient.SqlCommand("select SUM(Amount) from Expenses where Date between '" + date1 + "' and '" + date2 + "'", connector.connection);
            expense = Convert.ToInt32(connector.command.ExecuteScalar().ToString());
            MessageBox.Show(expense.ToString());

            connector.command = new System.Data.SqlClient.SqlCommand("insert into Accounts values('" + GetMonthName(month) + "'," + year + "," + revenue.ToString() + "," + expense.ToString() + "," + (revenue-expense).ToString() + ")", connector.connection);
            try
            {
                connector.command.ExecuteNonQuery();
                MessageBox.Show("Accounts updated successfully. Your net profit of this month is " + (revenue-expense).ToString());
            }
            catch
            {

            }
            connector.CloseDatabaseConnection();

        }

        private string GetMonthName(string nm)
        {
            switch(nm)
            {
                case "1":
                    {
                        return "January";
                    }
                case "2":
                    {
                        return "February";
                    }
                case "3":
                    {
                        return "March";
                    }
                case "4":
                    {
                        return "April";
                    }
                case "5":
                    {
                        return "May";
                    }
                case "6":
                    {
                        return "June";
                    }
                case "7":
                    {
                        return "July";
                    }
                case "8":
                    {
                        return "August";
                    }
                case "9":
                    {
                        return "September";
                    }
                case "10":
                    {
                        return "October";
                    }
                case "11":
                    {
                        return "November";
                    }
                case "12":
                    {
                        return "December";
                    }
            }
            return "";
        }

        private void PrintIncomeStatementButton_Click(object sender, RoutedEventArgs e)
        {
            IncomeStatement incomeStatement = new IncomeStatement(IAccYearText.Text);
            incomeStatement.Show();
        }

        internal MakeInfoManager MakeInfoManager
        {
            get => default(MakeInfoManager);
            set
            {
            }
        }

        internal ModelInfoManager ModelInfoManager
        {
            get => default(ModelInfoManager);
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
    }
}
