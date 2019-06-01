using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Utilities;
using Shop.Model;
using System.Windows;
using System.Data.SqlClient;

namespace Shop.ViewModel
{
    class SalesInfoManager
    {
        private List<SalesInfo> salesInfos;

        internal SalesInfo SalesInfo
        {
            get => default(SalesInfo);
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

        internal SalesInfo SalesInfo1
        {
            get => default(SalesInfo);
            set
            {
            }
        }

        private void RetriveSalesInformation()
        {
            salesInfos = new List<SalesInfo>();

            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();

            connector.command = new System.Data.SqlClient.SqlCommand("select * from SalesRecord Order by Date desc", connector.connection);
            connector.reader = connector.command.ExecuteReader();

            while (connector.reader.Read())
            {
                salesInfos.Add(new SalesInfo { Id = int.Parse(connector.reader["Id"].ToString()), EngineNumber = connector.reader["EngineNumber"].ToString(), ChassisNumber = connector.reader["ChassisNumber"].ToString(), Color = connector.reader["Color"].ToString(), Price = connector.reader["SalePrice"].ToString(), Paid = connector.reader["Paid"].ToString(), Due = connector.reader["Due"].ToString(), Status = connector.reader["Status"].ToString(), CustomerId = connector.reader["CustomerId"].ToString(), Date = connector.reader["Date"].ToString() });
            }
            connector.reader.Close();
            connector.CloseDatabaseConnection();
        }

        public List<SalesInfo> GetSalesData()
        {
            RetriveSalesInformation();
            return salesInfos;
        }

        public void InsertCustomer(string name, string profession, string nid, string etin, string contact)
        {
            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();

            connector.command = new System.Data.SqlClient.SqlCommand("insert into Customers values('" + name + "','" + profession + "','" + nid + "','" + etin + "','" + contact + "')", connector.connection);
            try
            {
                connector.command.ExecuteNonQuery();
                MessageBox.Show("New customer added successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connector.CloseDatabaseConnection();
        }

        public void UpdateSalesRecord(string salePrice, string paidAmount, string engineNumber, string customerId, string date)
        {
            int total, paid, due;
            string status;
            total = int.Parse(salePrice);
            paid = int.Parse(paidAmount);
            due = total - paid;
            if (due == 0)
                status = "Paid";
            else
                status = "Paying";

            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();

            connector.command = new SqlCommand("select ModelId from Vehicle where EngineNumber='" + engineNumber + "'", connector.connection);
            string modelId = connector.command.ExecuteScalar().ToString();

            connector.command = new SqlCommand("select Color from Vehicle where EngineNumber='"+engineNumber+"'", connector.connection);
            string color = connector.command.ExecuteScalar().ToString();

            connector.command = new SqlCommand("select ChassisNumber from Vehicle where EngineNumber='" + engineNumber + "'", connector.connection);
            string chassisNumber = connector.command.ExecuteScalar().ToString();

            connector.command = new SqlCommand("insert into SalesRecord values('" + engineNumber + "','" + chassisNumber + "','" + color + "'," + total.ToString() + "," + paid.ToString() + "," + due.ToString() + ",'" + status + "'," + customerId + ",'" + date + "','"+modelId+"')", connector.connection);
            try
            {
                connector.command.ExecuteNonQuery();
                MessageBox.Show("Required car sales information updated into Sales Record");
                connector.command = new SqlCommand("delete from Vehicle where EngineNumber='" + engineNumber + "'", connector.connection);
                connector.command.ExecuteNonQuery();

                connector.command = new SqlCommand("delete from Images where EngineNumber='" + engineNumber + "'", connector.connection);
                connector.command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            connector.CloseDatabaseConnection();
        }

        public void DeliverUpdate(string customerId)
        {
            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();

            connector.command = new SqlCommand("update SalesRecord set Status='Delivered' where CustomerId='" + customerId + "'", connector.connection);
            try
            {
                connector.command.ExecuteNonQuery();
                MessageBox.Show("Required car delivary information updated into Sales Record");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AccountsReceivableUpdate(string accountNumber, string amountPaid, string date, string paymentMethod)
        {
            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();

            connector.command = new System.Data.SqlClient.SqlCommand("insert into AccountsReceivables values('" + accountNumber + "'," + amountPaid + ",'" + date + "','"+paymentMethod+"')", connector.connection);
            try
            {
                connector.command.ExecuteNonQuery();
                //MessageBox.Show("Required car sales account receivables information updated into Sales Record");                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            int paid = int.Parse(amountPaid);
            connector.command = new System.Data.SqlClient.SqlCommand("select SalePrice from SalesRecord where EngineNumber='" + accountNumber + "'", connector.connection);
            int total = int.Parse(connector.command.ExecuteScalar().ToString());
            connector.command = new System.Data.SqlClient.SqlCommand("select Paid from SalesRecord where EngineNumber='" + accountNumber + "'", connector.connection);
            int totalPaid = int.Parse((connector.command.ExecuteScalar()).ToString());
            totalPaid += paid;
            int due = (total - totalPaid);
            connector.command = new System.Data.SqlClient.SqlCommand("update SalesRecord set Paid=" + totalPaid.ToString() + " where EngineNumber='" + accountNumber + "'", connector.connection);
            try
            {
                connector.command.ExecuteNonQuery();
                //MessageBox.Show("Required car sales information updated into Sales Record");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connector.command = new System.Data.SqlClient.SqlCommand("update SalesRecord set Due=" + due.ToString() + " where EngineNumber='" + accountNumber + "'", connector.connection);
            try
            {
                connector.command.ExecuteNonQuery();
                MessageBox.Show("Required car sales information updated into Sales Record");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (due == 0)
            {
                connector.command = new System.Data.SqlClient.SqlCommand("update SalesRecord set Status='Paid' where EngineNumber='" + accountNumber + "'", connector.connection);
                try
                {
                    connector.command.ExecuteNonQuery();
                    MessageBox.Show("This car is ready for delivery.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        internal void AccountsRecievableInitials(string accountNumber, string amountPaid, string date, string paymentMethod)
        {
            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();

            connector.command = new System.Data.SqlClient.SqlCommand("insert into AccountsReceivables values('" + accountNumber + "'," + amountPaid + ",'" + date + "','" + paymentMethod + "')", connector.connection);
            try
            {
                connector.command.ExecuteNonQuery();
                //MessageBox.Show("Required car sales account receivables information updated into Sales Record");                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connector.CloseDatabaseConnection();
        }
    }
}
