using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Utilities;
using Shop.Model;
using System.Data.SqlClient;
using System.Data;

namespace Shop.ViewModel
{
    class ModelInfoManager
    {
        public DatabaseConnector connector;
        public SqlDataAdapter dataAdapter;
        public DataTable dataTable;

        public ModelInfoManager()
        {
            connector = new DatabaseConnector();
        }

        internal ModelInfo ModelInfo
        {
            get => default(ModelInfo);
            set
            {
            }
        }

        public void RetriveModels()
        {
            connector.ConnectToDatabase();

            connector.command = new SqlCommand("select * from Model Cross Join Make where Model.MakeId=Make.Id", connector.connection);
            dataAdapter = new SqlDataAdapter(connector.command);
            dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            connector.CloseDatabaseConnection();
        }

        public void InsertModel(string makeId, string name, string year, string edition, string engine, string bodyType, string transmission)
        {
            connector.ConnectToDatabase();
            connector.command = new SqlCommand("insert into Model values('"+makeId+ "','"+name+ "','"+year+ "','"+edition+ "','"+engine+ "','"+bodyType+ "','"+transmission+"')", connector.connection);
            connector.command.ExecuteNonQuery();
            connector.CloseDatabaseConnection();
        }

        public void RemoveModel(string id)
        {
            connector.ConnectToDatabase();
            connector.command = new System.Data.SqlClient.SqlCommand("delete from Model where Id=" + id, connector.connection);
            connector.command.ExecuteNonQuery();
            connector.CloseDatabaseConnection();
        }
    }
}
