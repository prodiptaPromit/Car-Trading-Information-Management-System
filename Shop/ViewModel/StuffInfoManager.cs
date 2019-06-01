using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Shop.Model;
using Shop.Utilities;
using System.IO;

namespace Shop.ViewModel
{
    class StuffInfoManager
    {
        private List<StuffInfo> stuffs;

        private DatabaseConnector connector;

        public StuffInfoManager()
        {
            connector = new DatabaseConnector();
        }

        internal StuffInfo StuffInfo
        {
            get => default(StuffInfo);
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

        private void RetriveEmployeeData()
        {
            connector.ConnectToDatabase();
            stuffs = new List<StuffInfo>();
            connector.command = new SqlCommand("select * from Stuff", connector.connection);
            try
            {
                connector.reader = connector.command.ExecuteReader();
                while (connector.reader.Read())
                {
                    stuffs.Add(new StuffInfo { Id = connector.reader["Id"].ToString(), Name = connector.reader["Name"].ToString(), Designation = connector.reader["Designation"].ToString(), Salary = connector.reader["Salary"].ToString(), NId = connector.reader["NId"].ToString(), Address = connector.reader["Address"].ToString(), Contact = connector.reader["Contact"].ToString() });
                }
                connector.reader.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ooops\n" + ex.ToString());
            }
            connector.CloseDatabaseConnection();
        }

        public List<StuffInfo> Stuffs()
        {
            RetriveEmployeeData();
            return stuffs;
        }

        public void InsertStuff(string id, string name, string desg, string salary, string nid, string address, string contact)
        {
            connector.ConnectToDatabase();

            connector.command = new SqlCommand("insert into Stuff(Id,Name,Designation,Salary,NId,Address,Contact) values('" + id + "','" + name + "','" + desg + "'," + salary + ",'" + nid + "','" + address + "','" + contact + "')", connector.connection);
            connector.command.ExecuteNonQuery();

            connector.CloseDatabaseConnection();
        }

        public void UpdateStuffInfo(string field, string data, string id)
        {
            connector.ConnectToDatabase();
            string query = "update Stuff set ";
            if (field == "Salary")
            {
                query = query + field + "=" + data + " where Id='" + id + "'";
            }
            else
            {
                query = query + field + "='" + data + "' where Id='" + id + "'";
            }
            connector.command = new System.Data.SqlClient.SqlCommand(query, connector.connection);
            connector.command.ExecuteNonQuery();
            connector.CloseDatabaseConnection();
        }

        public void RemoveStuff(string id)
        {
            connector.ConnectToDatabase();
            connector.command = new System.Data.SqlClient.SqlCommand("delete from Stuff where Id='" + id + "'", connector.connection);
            connector.command.ExecuteNonQuery();
            connector.CloseDatabaseConnection();
        }
    }
}
