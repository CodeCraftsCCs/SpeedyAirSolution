using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyAir.Models
{
    public class OrderAssignment
    {
        public string OrderId { get; set; }
        public int FlightNumber { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public int Day { get; set; }
    }
}
