using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Model
{
    class SalesInfo
    {
        public int Id { get; set; }
        public string EngineNumber { get; set; }
        public string ChassisNumber { get; set; }
        public string Color { get; set; }
        public string Price { get; set; }
        public string Paid { get; set; }
        public string Due { get; set; }
        public string Status { get; set; }
        public string CustomerId { get; set; }
        public string Date { get; set; }
    }
}
