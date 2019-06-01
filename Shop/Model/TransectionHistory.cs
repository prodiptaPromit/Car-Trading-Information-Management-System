using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Model
{
    class TransectionHistory
    {
        public int Id { get; set; }
        public string EngineNumber { get; set; }
        public string Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string Date { get; set; }
    }
}
