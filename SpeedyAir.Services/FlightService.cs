using SpeedyAir.Models;
using System.Collections.Generic;
using System.Linq;

namespace SpeedyAir.Services
{

    /// <summary>
    /// Service class to manage flight operations
    /// </summary>
    public class FlightService
    {
        private List<Flight> _flights = new List<Flight>();

        /// <summary>
        /// Load Flights (hardcoded)
        /// </summary>
        public void LoadFlights()
        {
            // Predefined flights based on the scenario
            _flights.Add(new Flight { FlightNumber = 1, Departure = "YUL", Arrival = "YYZ", Day = 1 });
            _flights.Add(new Flight { FlightNumber = 2, Departure = "YUL", Arrival = "YYC", Day = 1 });
            _flights.Add(new Flight { FlightNumber = 3, Departure = "YUL", Arrival = "YVR", Day = 1 });
            _flights.Add(new Flight { FlightNumber = 4, Departure = "YUL", Arrival = "YYZ", Day = 2 });
            _flights.Add(new Flight { FlightNumber = 5, Departure = "YUL", Arrival = "YYC", Day = 2 });
            _flights.Add(new Flight { FlightNumber = 6, Departure = "YUL", Arrival = "YVR", Day = 2 });
        }

        /// <summary>
        /// Returns read only list of Flights
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Flight> GetFlights() => _flights;

        /// <summary>
        /// Displays/Prints list of flights to console
        /// </summary>
        public void DisplayFlights()
        {
            foreach (var flight in _flights)
            {
                Console.WriteLine($"Flight: {flight.FlightNumber}, departure: {flight.Departure}, arrival: {flight.Arrival}, day: {flight.Day}");
            }
        }
    }
}
