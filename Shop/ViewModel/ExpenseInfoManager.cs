using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Model;
using Shop.Utilities;

namespace Shop.ViewModel
{
    class ExpenseInfoManager
    {
        private List<ExpenseInfo> expenses;
        private DatabaseConnector connector;

        public ExpenseInfoManager()
        {
            connector = new DatabaseConnector();
        }

        internal ExpenseInfo ExpenseInfo
        {
            get => default(ExpenseInfo);
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

        private void RetriveExpenseInfo()
        {
            connector.ConnectToDatabase();
            expenses = new List<ExpenseInfo>();
            connector.command = new System.Data.SqlClient.SqlCommand("select * from Expenses", connector.connection);
            try
            {
                connector.reader = connector.command.ExecuteReader();
                while (connector.reader.Read())
                {
                    expenses.Add(new ExpenseInfo { Id = connector.reader["Id"].ToString(), Title = connector.reader["Title"].ToString(), Amount = connector.reader["Amount"].ToString(), Date = connector.reader["Date"].ToString() });
                }
                connector.reader.Close();
            }
            catch
            {

            }
            connector.CloseDatabaseConnection();
        }

        public List<ExpenseInfo> GetExpenses()
        {
            RetriveExpenseInfo();
            return expenses;
        }

        public void InsertExpenses(string title, string amount, string date)
        {
            connector.ConnectToDatabase();
            connector.command = new System.Data.SqlClient.SqlCommand("insert into Expenses values('" + title + "'," + amount + ",'" + date + "')", connector.connection);
            connector.command.ExecuteNonQuery();
            connector.CloseDatabaseConnection();
        }

        public void InsertSalaryExpense(string date, string netSalary)
        {
            connector.ConnectToDatabase();
            connector.command = new System.Data.SqlClient.SqlCommand("insert into Expenses values('Salary Expense'," + netSalary + ",'" + date + "')", connector.connection);
            connector.command.ExecuteNonQuery();
            connector.CloseDatabaseConnection();
        }

        public string CalculateMonthlySalary()
        {
            string netSalary;
            connector.ConnectToDatabase();
            connector.command = new System.Data.SqlClient.SqlCommand("SELECT SUM(Salary) FROM Stuff", connector.connection);
            netSalary = connector.command.ExecuteScalar().ToString();
            connector.CloseDatabaseConnection();
            return netSalary;
        }
    }
}
