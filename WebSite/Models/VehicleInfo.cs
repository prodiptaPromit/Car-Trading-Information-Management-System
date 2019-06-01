using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class VehicleInfo
    {
        public string Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Price { get; set; }
        public string Engine { get; set; }
        public string Transmission { get; set; }
        public string Displacement { get; set; }
        public string Milage { get; set; }
        public string Specification { get; set; }
        public string BodyType { get; set; }
        public byte[] CoverImage { get; set; }

        public Controllers.VehicleInfoController VehicleInfoController
        {
            get => default(Controllers.VehicleInfoController);
            set
            {
            }
        }
    }
}