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
            // År , måned, dag 
            DateTime startTime = new DateTime(2023, 1, 1);
            DateTime EndTime = new DateTime(2023, 1, 1);
            // Eksempel på slutttidspunkt (5. januar 2024 kl. 23:59:59)

            Harbor harbor = new Harbor("My Harbor", new CargoStorage("cargoStoage"));
            harbor.InitializeShips(10, Enums.ShipSize.Small, Enums.ShipType.ContainerShip, 10);
            harbor.InitializeCranes(10);
            harbor.InitializeDocks(10, Enums.DockType.ContainerDock, Enums.DockSize.Medium);
            Console.WriteLine(harbor);
            harbor.SetUpWatch(new DateTime(2023, 12, 03), new DateTime(2024, 01, 02));
            Console.WriteLine(harbor.Watch.StartTime);
            Console.WriteLine(harbor.Watch.EndTime);
            harbor.QueueShipsToDock(); 
            harbor.DockShips();
            Console.WriteLine(harbor.Watch.MeasureTimeElapsed());



           
        }
    }
}