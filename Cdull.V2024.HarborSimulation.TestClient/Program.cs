using Cdull.V2024.HarborSimulation.SimulationFramework;
using System.ComponentModel.DataAnnotations;

namespace Cdull.V2024.HarborSimulation.TestClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            //CargoStorage harborCargoStorage = new("cargoStorage");  
            // Eksempel på starttidspunkt (1. januar 2024 kl. 00:00:00)
            DateTime startTime = new DateTime(2024, 1, 1, 0, 0, 0);

            // Eksempel på slutttidspunkt (5. januar 2024 kl. 23:59:59)
            DateTime stopTime = new DateTime(2024, 1, 5, 23, 59, 59);
            Watch watch = new(startTime, stopTime);
            watch.StartCountingTime(); 
            Harbor harbor = new Harbor("My Harbor", new CargoStorage("cargoStoage"));
            harbor.InitializeShips(10, "small", 10);
            harbor.InitializeCranes(10);
            List<Crane> cranes = harbor.GetCraneList();  
            harbor.InitializeDocks(10, "normal", "small", cranes);
            Console.WriteLine(harbor);
            



            watch.StopCountingTime();
            Console.WriteLine(watch.MeasureTimeElapsed()); 
           
        }
    }
}