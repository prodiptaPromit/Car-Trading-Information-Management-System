using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Data.Sql;
using System.Configuration;

namespace Shop.Utilities
{
    class DatabaseConnector
    {
        public SqlConnection connection;
        public SqlCommand command;
        public SqlDataReader reader;

        public DatabaseConnector()
        {
            connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=G:\BUBT\4th Year\Finalization\Project\Car Trading Management System V4.0\Implementation\Shop\Shop\App Data\ShopDatabase.mdf;Integrated Security=True");
        }

        public void ConnectToDatabase()
        {            
            try
            {
                connection.Open();
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public void CloseDatabaseConnection()
        {
            connection.Close();
        }
    }
}
