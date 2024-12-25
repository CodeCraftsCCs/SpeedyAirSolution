using SpeedyAir.Models;
using SpeedyAir.Services;
using System;

namespace SpeedyAir.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // User Story #1: Loading flights
            var flightService = new FlightService();
            flightService.LoadFlights();
            flightService.DisplayFlights();

            // User Story #2:
            // Load Orders
            var orderService = new OrderService();
            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Contents", "orders.json");
            orderService.LoadOrders(jsonFilePath);

            // Scheduling orders to flights
            var assignments = orderService.AssignOrdersToFlights((List<Flight>)flightService.GetFlights());
            orderService.DisplayScheduledOrders(assignments);

            Console.WriteLine("Application completed. Press any key to exit.");
            Console.ReadKey();
        }
    }
}
