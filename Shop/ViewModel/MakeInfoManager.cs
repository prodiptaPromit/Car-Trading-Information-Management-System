using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Utilities;
using Shop.Model;
using System.Data.SqlClient;
using System.Data;
using System.Data.Sql;
using System.Configuration;


namespace Shop.ViewModel
{
    class MakeInfoManager
    {
        public DatabaseConnector connector;
        public SqlDataAdapter dataAdapter;
        public DataTable dataTable;

        public MakeInfoManager()
        {
            connector = new DatabaseConnector();
        }

        internal MakeInfo MakeInfo
        {
            get => default(MakeInfo);
            set
            {
            }
        }

        public void RetriveMakes()
        {
            connector.ConnectToDatabase();

            connector.command = new SqlCommand("select * from Make", connector.connection);
            dataAdapter = new SqlDataAdapter(connector.command);
            dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            connector.CloseDatabaseConnection();
        }

        public void InsertMake(string make, string country)
        {
            connector.ConnectToDatabase();
            connector.command = new SqlCommand("insert into Make values('"+make+"','"+country+"')",connector.connection);
            connector.command.ExecuteNonQuery();
            connector.CloseDatabaseConnection();
        }

        public void RemoveMake(string id)
        {
            connector.ConnectToDatabase();
            connector.command = new System.Data.SqlClient.SqlCommand("delete from Make where Id=" + id, connector.connection);
            connector.command.ExecuteNonQuery();
            connector.CloseDatabaseConnection();
        }
    }
}
