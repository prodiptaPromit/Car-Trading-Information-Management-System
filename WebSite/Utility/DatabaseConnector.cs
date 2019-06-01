using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace WebSite.Utility
{
    public class DatabaseConnector
    {
        public SqlConnection conn;

        public DatabaseConnector()
        {
            conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=G:\BUBT\4th Year\Finalization\Project\Car Trading Management System V4.0\Implementation\Shop\Shop\App Data\ShopDatabase.mdf;Integrated Security=True");
        }

        public void ConnectToDatabase()
        {
            try
            {
                conn.Open();
            }
            catch
            {

            }
        }

        public void CloseDatabaseConnection()
        {
            conn.Close();
        }
    }
}