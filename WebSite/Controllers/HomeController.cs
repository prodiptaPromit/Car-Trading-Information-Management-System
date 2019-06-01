using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using WebSite.Models;
using WebSite.Utility;
using System.Net.Mail;

namespace WebSite.Controllers
{
    public class HomeController : Controller
    {
        List<VehicleInfo> info;
        public ActionResult Index()
        {
            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();

            info = new List<VehicleInfo>();
            
            SqlCommand cmd = new SqlCommand("select * from Make cross join Model Cross Join Vehicle where Model.MakeId=Make.Id and Model.Id = Vehicle.ModelId Order by Date desc", connector.conn);
            SqlDataReader reader = cmd.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {
                info.Add(new VehicleInfo { Id= reader["EngineNumber"].ToString(), Make = reader["Make"].ToString(), Model = reader["Name"].ToString(), Year = reader["Year"].ToString(), Transmission = reader["Transmission"].ToString(), Price = reader["SalePrice"].ToString() });
                ;
                count++;
                if (count == 3)
                    break;
            }
            reader.Close();
            connector.CloseDatabaseConnection();
            SetImage();
            return View(info);
        }

        private void SetImage()
        {
            foreach(var x in info)
            {
                x.CoverImage = GetImagePath(x.Id);
            }
        }

        private byte[] GetImagePath(string engineNumber)
        {
            byte[] image = null;
            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();

            SqlCommand cmd = new SqlCommand("select Image from Images where EngineNumber='" + engineNumber + "' order by Id desc", connector.conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                image = (byte[])reader["Image"];
                break;
            }
            connector.CloseDatabaseConnection();
            return image;
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Services()
        {
            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();

            info = new List<VehicleInfo>();

            SqlCommand cmd = new SqlCommand("select * from Make cross join Model Cross Join Vehicle where Model.MakeId=Make.Id and Model.Id = Vehicle.ModelId Order by Date desc", connector.conn);
            SqlDataReader reader = cmd.ExecuteReader();
            int count = 0;
            while (reader.Read())
            {
                info.Add(new VehicleInfo { Id = reader["EngineNumber"].ToString(), Make = reader["Make"].ToString(), Model = reader["Name"].ToString(), Year = reader["Year"].ToString(), Transmission = reader["Transmission"].ToString(), Price = reader["SalePrice"].ToString() });
                ;
                count++;
                if (count == 3)
                    break;
            }
            reader.Close();
            connector.CloseDatabaseConnection();
            SetImage();
            return View(info);
        }

        public ActionResult Pages()
        {
            return View();
        }

        /*public Action Notify()
        {
            var smtp = "smtp.outlook.com";
            var sender = "prodipta.promit@outlook.com";
            var receiver = "prodipta.promit@gmail.com";
            var senderPassword = "WarriorProdipta";
            var subject = "Response from car shop.";

            var port = 587;
            var mailMessage = new MailMessage();
            var smtpClient = new SmtpClient(smtp);

            mailMessage.From = new MailAddress(sender);
            mailMessage.To.Add(receiver);
            mailMessage.Subject = subject;
            mailMessage.Body = "Test";

            smtpClient.Port = port;
            smtpClient.Credentials = new System.Net.NetworkCredential(sender, senderPassword);
            smtpClient.EnableSsl = true;

            smtpClient.Send(mailMessage);
            

        }*/
    }
}