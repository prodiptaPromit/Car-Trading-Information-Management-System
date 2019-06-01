using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Utilities;
using Shop.Model;
using System.IO;
using System.Windows;

namespace Shop.ViewModel
{
    class VehicleInforManager
    {
        private DatabaseConnector connector;
        private List<VehicleInfo> vehicleInfo;
        private List<string> makeList;
        private List<ModelInfo> models;

        public VehicleInforManager()
        {
            
        }

        private void RetriveData()
        {
            connector = new DatabaseConnector();
            connector.ConnectToDatabase();
            
            vehicleInfo = new List<VehicleInfo>();
            RetriveMakeInfo();
            connector.command = new System.Data.SqlClient.SqlCommand("select * from Model Cross Join Vehicle where Model.Id = Vehicle.ModelId Order by Date desc", connector.connection);
            try
            {
                connector.reader = connector.command.ExecuteReader();
                while (connector.reader.Read())
                {
                    vehicleInfo.Add(new VehicleInfo { Id = int.Parse(connector.reader["Id"].ToString()), Make = Make[int.Parse(connector.reader["MakeId"].ToString())], Model = connector.reader["Name"].ToString(), Year = connector.reader["Year"].ToString(), Edition = connector.reader["Edition"].ToString(), Engine = connector.reader["Engine"].ToString(), BodyType = connector.reader["BodyType"].ToString(), Transmission = connector.reader["Transmission"].ToString(), EngineNumber = connector.reader["EngineNumber"].ToString(), ChassisNumber = connector.reader["ChassisNumber"].ToString(), Color = connector.reader["Color"].ToString(), BuyPrice = int.Parse(connector.reader["Purchase"].ToString()), SalePrice = int.Parse(connector.reader["SalePrice"].ToString()), Milage=connector.reader["Milage"].ToString() });
                }
                connector.reader.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ooops\n" + ex.ToString());
            }
            connector.CloseDatabaseConnection();
        }

        public List<VehicleInfo> GetVehicles()
        {
            RetriveData();
            return vehicleInfo;
        }

        private string[] Make;

        private void RetriveMakeInfo()
        {
            Make = new string[101];
            int count = 1;
            Make[0] = "Select Any...";
            connector.command = new System.Data.SqlClient.SqlCommand("select Make from Make", connector.connection);
            try
            {
                connector.reader = connector.command.ExecuteReader();
                while (connector.reader.Read())
                {
                    Make[count] = connector.reader["Make"].ToString();
                    count++;
                }
                connector.reader.Close();
            }
            catch
            {

            }
            makeList = new List<string>();
            makeList = Make.ToList();
        }

        public List<string> MakeList()
        {
            return makeList;
        }

        public void InsertIntoInventory(string model, string edition, string year, string engineNumber, string chassisNumber, string color, string purchase, string salePrice, string date, string description, string milage)
        {           
            int modelId = GetModelId(model, edition, year);
            connector = new DatabaseConnector();
            connector.ConnectToDatabase();
            string query = "insert into Vehicle values('" + engineNumber + "','" + chassisNumber + "','" + color + "'," + purchase + "," + salePrice + "," + modelId.ToString() + ",'" + date + "','"+description+"','"+milage+"')";
            connector.command = new System.Data.SqlClient.SqlCommand(query, connector.connection);
            try
            {
                connector.command.ExecuteNonQuery();                
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            connector.CloseDatabaseConnection();            
        }

        public void UpdateAccounts(string purchase, string date)
        {
            connector = new DatabaseConnector();
            connector.ConnectToDatabase();
            connector.command = new System.Data.SqlClient.SqlCommand("insert into Expenses values('Car Purchased'," + purchase + ",'" + date + "')", connector.connection);
            try
            {
                connector.command.ExecuteNonQuery();
                //MessageBox.Show("Required entry added successfully.");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            connector.CloseDatabaseConnection();
        }

        private int GetModelId(string model, string edition, string year)
        {
            int id=1;
            connector = new DatabaseConnector();
            connector.ConnectToDatabase();
            connector.command = new System.Data.SqlClient.SqlCommand("select Id from Model where Name='"+model+"' and Edition='"+edition+"' and Year='"+year+"'", connector.connection);
            id = int.Parse(connector.command.ExecuteScalar().ToString());
            return id;
        }

        public void UpdateVehicleInfo(string field, string data, string id)
        {
            connector = new DatabaseConnector();
            connector.ConnectToDatabase();
            connector.command = new System.Data.SqlClient.SqlCommand("update Vehicle set " + field + "='" + data + "' where EngineNumber='" + id+"'", connector.connection);
            try
            {
                connector.command.ExecuteNonQuery();
                System.Windows.MessageBox.Show("Required vehicle updated successfully");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            connector.CloseDatabaseConnection();
        }

        private List<string> years;

        public List<string> RetriveYears(string model, string edition)
        {
            connector = new DatabaseConnector();
            connector.ConnectToDatabase();
            years = new List<string>();
            connector.command = new System.Data.SqlClient.SqlCommand("select Year from Model where Name='"+model+"' AND Edition='"+edition+"'", connector.connection);
            try
            {
                connector.reader = connector.command.ExecuteReader();
                while (connector.reader.Read())
                {
                    years.Add(connector.reader["Year"].ToString());
                    //System.Windows.MessageBox.Show(connector.reader["Year"].ToString());
                }
                connector.reader.Close();
            }
            catch
            {

            }
            connector.CloseDatabaseConnection();
            return years;
        }

        public void InsertImage(string engineNumber, string imageFile)
        {
            connector = new DatabaseConnector();
            connector.ConnectToDatabase();
            connector.command = new System.Data.SqlClient.SqlCommand("Insert Into Images(EngineNumber,Image) Select '" + engineNumber + "',BulkColumn FROM Openrowset( Bulk '" + imageFile + "', Single_Blob) as Image", connector.connection);
            try
            {
                connector.command.ExecuteNonQuery();
                System.Windows.MessageBox.Show("Required vehicle image updated successfully");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            connector.CloseDatabaseConnection();
        }

        private void RetriveModels()
        {
            models = new List<ModelInfo>();
            connector = new DatabaseConnector();
            connector.ConnectToDatabase();
            connector.command = new System.Data.SqlClient.SqlCommand("select *,* from Model cross join Make where Model.MakeId=Make.Id", connector.connection);
            try
            {
                connector.reader = connector.command.ExecuteReader();
                while (connector.reader.Read())
                {
                    models.Add(new ModelInfo { Id = int.Parse(connector.reader["Id"].ToString()), Make = connector.reader["Make"].ToString(), Model = connector.reader["Name"].ToString(), Year = connector.reader["Year"].ToString(), Edition = connector.reader["Edition"].ToString(), BodyType = connector.reader["BodyType"].ToString(), Engine = connector.reader["Engine"].ToString(), Transmission = connector.reader["Transmission"].ToString() });
                }
                connector.reader.Close();
            }
            catch
            {

            }
            connector.CloseDatabaseConnection();
        }

        public List<ModelInfo> Models()
        {
            RetriveModels();
            return models;
        }

        internal VehicleInfo VehicleInfo
        {
            get => default(VehicleInfo);
            set
            {
            }
        }

        internal ModelInfo ModelInfo
        {
            get => default(ModelInfo);
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
    }
}
