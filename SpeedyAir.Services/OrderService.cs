using SpeedyAir.Models;
using SpeedyAir.Utilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace SpeedyAir.Services
{
    /// <summary>
    /// Service class to manage order operations
    /// </summary>
    public class OrderService
    {
        private List<Order> _orders = new List<Order>();

        /// <summary>
        /// Returns count of orders
        /// </summary>
        /// <returns></returns>
        public int GetOrderCounts() => _orders.Count;

        /// <summary>
        /// Loads orders from json file
        /// </summary>
        /// <param name="jsonFilePath"></param>
        public void LoadOrders(string jsonFilePath)
        {
            string json = JsonLoader.LoadJson(jsonFilePath);
            var ordersDict = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);
            foreach (var item in ordersDict)
            {
                _orders.Add(new Order { OrderId = item.Key, Destination = item.Value.destination });
            }

        }

        /// <summary>
        /// Assigns orders to flights
        /// </summary>
        /// <param name="flights"></param>
        /// <returns></returns>
        public List<OrderAssignment> AssignOrdersToFlights(List<Flight> flights)
        {
            List<OrderAssignment> assignments = new List<OrderAssignment>();
            const int MaxCapacityPerFlight = 20; // Assuming each flight can carry 20 orders

            foreach (var flight in flights)
            {
                var ordersForFlight = _orders.Where(o => o.Destination == flight.Arrival)
                                            .Take(MaxCapacityPerFlight).ToList();

                // Assign orders to this flight
                foreach (var order in ordersForFlight)
                {
                    assignments.Add(new OrderAssignment
                    {
                        OrderId = order.OrderId,
                        FlightNumber = flight.FlightNumber,
                        Departure = flight.Departure,
                        Arrival = flight.Arrival,
                        Day = flight.Day
                    });
                    // Remove the assigned order from the list to avoid reassignment
                    _orders.Remove(order);
                }
            }

            // For orders that are not assigned
            foreach (var remainingOrder in _orders)
            {
                assignments.Add(new OrderAssignment
                {
                    OrderId = remainingOrder.OrderId,
                    FlightNumber = -1, // Indicate not scheduled
                    Departure = "",
                    Arrival = "",
                    Day = -1
                });
            }

            return assignments;
        }

        /// <summary>
        /// Displays scheduled orders
        /// scheduled orders are the ones with assigned flights
        /// </summary>
        /// <param name="assignments"></param>
        public void DisplayScheduledOrders(List<OrderAssignment> assignments)
        {
            foreach (var assignment in assignments)
            {
                if (assignment.FlightNumber != -1) //Assigned
                {
                    Console.WriteLine($"order: {assignment.OrderId}, flightNumber: {assignment.FlightNumber}, departure: {assignment.Departure}, arrival: {assignment.Arrival}, day: {assignment.Day}");
                }
                else //Unassigned
                {
                    Console.WriteLine($"order: {assignment.OrderId}, flightNumber: not scheduled");
                }
            }
        }
    }

}
