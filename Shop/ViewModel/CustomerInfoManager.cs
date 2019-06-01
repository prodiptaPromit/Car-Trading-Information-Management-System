using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Utilities;
using Shop.Model;

namespace Shop.ViewModel
{
    class CustomerInfoManager
    {
        private List<CustomerInfo> customers;

        private void RetriveCustomerInformation()
        {
            customers = new List<CustomerInfo>();

            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();

            connector.command = new System.Data.SqlClient.SqlCommand("select * from Customers Order by Id desc", connector.connection);
            connector.reader = connector.command.ExecuteReader();

            while(connector.reader.Read())
            {
                customers.Add(new CustomerInfo { Name = connector.reader["Name"].ToString(), Profession = connector.reader["Profession"].ToString(), NId = connector.reader["NId"].ToString(), eTIN = connector.reader["eTIN"].ToString(), Contact = connector.reader["Contact"].ToString() });
            }
            connector.reader.Close();
            connector.CloseDatabaseConnection();
        }

        public List<CustomerInfo> GetCustomers()
        {
            RetriveCustomerInformation();
            return customers;
        }
    }
}
