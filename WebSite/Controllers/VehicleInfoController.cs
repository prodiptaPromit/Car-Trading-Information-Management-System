using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using WebSite.Utility;
using WebSite.Models;

namespace WebSite.Controllers
{
    public class VehicleInfoController : Controller
    {
        List<VehicleInfo> info;

        public DatabaseConnector DatabaseConnector
        {
            get => default(DatabaseConnector);
            set
            {
            }
        }

        // GET: VehicleInfo
        public ActionResult Index()
        {
            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();

            info = new List<VehicleInfo>();

            SqlCommand cmd = new SqlCommand("select * from Make cross join Model Cross Join Vehicle where Model.MakeId=Make.Id and Model.Id = Vehicle.ModelId order by Vehicle.Date desc", connector.conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                info.Add(new VehicleInfo { Id= reader["EngineNumber"].ToString(), Make = reader["Make"].ToString(), Model = reader["Name"].ToString(), Year = reader["Year"].ToString(), Transmission = reader["Transmission"].ToString(), Price = reader["SalePrice"].ToString() });
            }
            reader.Close();
            connector.CloseDatabaseConnection();
            SetImage();
            return View(info);
        }

        private void SetImage()
        {
            foreach (var x in info)
            {
                x.CoverImage = GetImagePath(x.Id);
            }
        }

        private byte[] GetImagePath(string engineNumber)
        {
            byte[] image = null;

            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();

            SqlCommand cmd = new SqlCommand("select Image from Images where EngineNumber='"+engineNumber+ "' order by Id desc", connector.conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                image = (byte[])reader["Image"];
                break;
            }
            connector.CloseDatabaseConnection();
            return image;
        }

        // GET: VehicleInfo/Details/5
        public ActionResult Details(string id)
        {
            VehicleInfo vehicleInfo = new VehicleInfo();

            DatabaseConnector connector = new DatabaseConnector();
            connector.ConnectToDatabase();

            SqlCommand cmd = new SqlCommand("select * from Make cross join Model Cross Join Vehicle where Model.MakeId = Make.Id and Model.Id = Vehicle.ModelId and Vehicle.EngineNumber='"+id+"'", connector.conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                vehicleInfo.Id = reader["Id"].ToString();
                vehicleInfo.Make = reader["Make"].ToString();
                vehicleInfo.Model = reader["Name"].ToString();
                vehicleInfo.Year = reader["Year"].ToString();
                vehicleInfo.Price = reader["SalePrice"].ToString();
                vehicleInfo.Engine = reader["Edition"].ToString();
                vehicleInfo.Transmission = reader["Transmission"].ToString();
                vehicleInfo.Displacement = reader["Engine"].ToString();
                vehicleInfo.Milage = reader["Milage"].ToString();
                vehicleInfo.Specification = reader["Description"].ToString();
                vehicleInfo.BodyType = reader["BodyType"].ToString();
            }
            reader.Close();           

            List<byte[]> images = new List<byte[]>();
            cmd = new SqlCommand("select * from Images where EngineNumber='"+id+ "' order by Id desc", connector.conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                images.Add((byte[])reader["Image"]);
            }
            reader.Close();
            ViewBag.Images = images.ToList();
            connector.CloseDatabaseConnection();
            return View(vehicleInfo);
        }

        // GET: VehicleInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VehicleInfo/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: VehicleInfo/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VehicleInfo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: VehicleInfo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VehicleInfo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
