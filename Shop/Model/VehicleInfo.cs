using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Model
{
    class VehicleInfo
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Edition { get; set; }
        public string Engine { get; set; }
        public string BodyType { get; set; }
        public string Transmission { get; set; }
        public string EngineNumber { get; set; }
        public string ChassisNumber { get; set; }
        public string Color { get; set; }
        public int BuyPrice { get; set; }
        public int SalePrice { get; set; }
        public string Milage { get; set; }
    }
}
