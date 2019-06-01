using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class VehicleImage
    {
        public int Id { get; set; }
        public string EngineNumber { get; set; }
        public byte[] Image { get; set; }

        public Controllers.HomeController HomeController
        {
            get => default(Controllers.HomeController);
            set
            {
            }
        }
    }
}