using NUnit.Framework;
using SpeedyAir.Models;
using SpeedyAir.Services;
using System.Linq;

namespace SpeedyAir.Tests
{
    [TestFixture]
    public class FlightServiceTests
    {
        private FlightService _flightService;

        [SetUp]
        public void Setup()
        {
            _flightService = new FlightService();
            _flightService.LoadFlights();
        }

        [Test]
        public void LoadFlights_ShouldPopulateFlightsCorrectly()
        {
            var flights = _flightService.GetFlights().ToList();
            Assert.AreEqual(6, flights.Count, "Should load exactly six flights."); //in current case total 6 flights
        }

        [Test]
        public void GetFlights_ShouldReturnIEnumerableOfTypeFlight()
        {
            var flights = _flightService.GetFlights();
            Assert.IsInstanceOf<IEnumerable<Flight>>(flights, "The returned type should be IEnumerable of Flight.");
        }

        [Test]
        public void LoadFlights_ShouldHaveCorrectFlightDetails()
        {
            var flights = _flightService.GetFlights().ToList();
            
            var first_flight = flights.First();
            Assert.Multiple(() =>
            {
                Assert.That(first_flight.FlightNumber, Is.EqualTo(1));
                Assert.That(first_flight.Departure, Is.EqualTo("YUL"));
                Assert.That(first_flight.Arrival, Is.EqualTo("YYZ"));
                Assert.That(first_flight.Day, Is.EqualTo(1));
            });
        }

        [Test]
        public void LoadFlights_ShouldHaveUniqueFlightNumbers()
        {
            var flights = _flightService.GetFlights().ToList();
            var unique_flight_numbers = flights.Select(f => f.FlightNumber).Distinct();
            
            Assert.That(unique_flight_numbers.Count(), Is.EqualTo(flights.Count));
        }
    }
}
