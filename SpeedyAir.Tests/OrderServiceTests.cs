using NUnit.Framework;
using SpeedyAir.Models;
using SpeedyAir.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpeedyAir.Tests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private OrderService _orderService;
        private FlightService _flightService;
        private string _test_json_path;

        [SetUp]
        public void Setup()
        {
            _orderService = new OrderService();
            _flightService = new FlightService();
            _flightService.LoadFlights();
            
            // Create test JSON content
            _test_json_path = Path.GetTempFileName();
            File.WriteAllText(_test_json_path, @"{
                ""order-001"": { ""destination"": ""YYZ"" },
                ""order-002"": { ""destination"": ""YYC"" },
                ""order-003"": { ""destination"": ""YVR"" }
            }");
        }

        [TearDown]
        public void Cleanup()
        {
            if (File.Exists(_test_json_path))
            {
                File.Delete(_test_json_path);
            }
        }

        [Test]
        public void LoadOrders_ShouldLoadOrdersCorrectly()
        {
            _orderService.LoadOrders(_test_json_path);
            Assert.That(_orderService.GetOrderCounts(), Is.EqualTo(3));
        }

        [Test]
        public void AssignOrdersToFlights_ShouldAssignOrdersCorrectly()
        {
            _orderService.LoadOrders(_test_json_path);
            var flights = _flightService.GetFlights().ToList();
            
            var assignments = _orderService.AssignOrdersToFlights(flights);
            
            Assert.Multiple(() =>
            {
                Assert.That(assignments, Is.Not.Empty);
                Assert.That(assignments.Count, Is.EqualTo(3));
                Assert.That(assignments.All(a => a.FlightNumber != -1), Is.True, "All orders should be assigned");
            });
        }

        [Test]
        public void AssignOrdersToFlights_ShouldHandleUnassignableOrders()
        {
            // Create JSON with invalid destination
            var invalid_json_path = Path.GetTempFileName();
            File.WriteAllText(invalid_json_path, @"{
                ""order-001"": { ""destination"": ""INVALID"" }
            }");
            
            _orderService.LoadOrders(invalid_json_path);
            var flights = _flightService.GetFlights().ToList();
            
            var assignments = _orderService.AssignOrdersToFlights(flights);
            
            Assert.Multiple(() =>
            {
                Assert.That(assignments.Count, Is.EqualTo(1));
                Assert.That(assignments[0].FlightNumber, Is.EqualTo(-1), "Unassignable order should have flight number -1");
            });

            File.Delete(invalid_json_path);
        }

        [Test]
        public void AssignOrdersToFlights_ShouldRespectFlightCapacity()
        {
            // Create JSON with more than 20 orders to same destination
            var capacity_test_json = Path.GetTempFileName();
            var json_content = "{" + string.Join(",", Enumerable.Range(1, 25)
                .Select(i => $@"""order-{i:D3}"": {{ ""destination"": ""YYZ"" }}")) + "}";
            
            File.WriteAllText(capacity_test_json, json_content);
            
            _orderService.LoadOrders(capacity_test_json);
            var flights = _flightService.GetFlights().ToList();
            
            var assignments = _orderService.AssignOrdersToFlights(flights);
            var assigned_to_first_flight = assignments.Count(a => a.FlightNumber == 1);
            
            Assert.That(assigned_to_first_flight, Is.LessThanOrEqualTo(20), "Should not exceed max capacity per flight");
            
            File.Delete(capacity_test_json);
        }
    }
}
