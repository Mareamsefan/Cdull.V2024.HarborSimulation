using Cdull.V2024.HarborSimulation.SimulationFramework;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Cdull.V2024.HarborSimulation.TestClient
{
    internal class Program
    {
       
        static void Main(string[] args)
        {

          /*  Scenario_1(); 
            Scenario_2();
            Scenario_3();*/

            IHarborSimulation driver = new Simulation();
            DateTime startTime = new DateTime(2024, 1, 1);
            DateTime endTime = new DateTime(2024, 1, 4);
            driver.Run(startTime, endTime, 2, Enums.Size.Medium, 10, Enums.Model.ContainerShip, 2, Enums.Size.Medium, Enums.Model.ContainerShip);
            
        }

        static void Scenario_1()
        {
            Console.WriteLine($"\n");
            // Scenario 1: 2 minutes elapses when small ships gets docked.
            Console.WriteLine(" Scenario 1: 2 minutes elapses when small ships gets docked.");
            //CargoStorage harborCargoStorage = new("cargoStorage");  
            // Eksempel på starttidspunkt (1. januar 2024 kl. 00:00:00)
            // År , måned, dag 
            DateTime startTime = new DateTime(2023, 1, 1, 12, 0, 0);
            DateTime EndTime = new DateTime(2023, 1, 3, 12, 0, 0);

            Harbor harbor = new Harbor("Cdull Harbor");
            //harbor.SetUpWatch(startTime, EndTime);
            Console.WriteLine($"Starttime: {startTime}");
            Console.WriteLine($"Endtime: {EndTime}");
            harbor.Watch.StartCountingTime();

            //setter opp noen et ship, og et dockslik at et skip kan docked: 
            Ship ship = new Ship("SKIPET", Enums.Model.ContainerShip, Enums.Size.Small);
            Crane crane = new Crane("DOCK CRANE");
            Dock dock = new Dock("DOCKEN", Enums.Size.Small, Enums.Model.ContainerShip, crane);

            // legger til shipet i harbor
            harbor.Ships.Add(ship);

            // legger til shipet i vente-kø for å docke 
            harbor.QueueShipsToDock();
            Console.WriteLine($"Waiting ships in harbor: {harbor.WaitingShips.Count}");
            harbor.Docks.Add(dock);
            Console.WriteLine($"Docks in harbor: {harbor.Docks.Count}");

            // docker shipet: 
            harbor.DockShips();
            Console.WriteLine($"Docked ships in harbor: {harbor.DockedShips.Count}");
            TimeSpan elapsedTime = harbor.Watch.MeasureTimeElapsed();
            Console.WriteLine($"Elapsed time: {elapsedTime}");
            Console.WriteLine($"\n");


        }

        static void Scenario_2()
        {
            Console.WriteLine($"\n");
            // Scenario 2: 5 minutes elapses when small ships gets docked.
            Console.WriteLine("Scenario 2:  5 minutes elapses when small ships gets docked.");
            //CargoStorage harborCargoStorage = new("cargoStorage");  
            // Eksempel på starttidspunkt (1. januar 2024 kl. 00:00:00)
            // År , måned, dag 
            DateTime startTime = new DateTime(2023, 1, 1, 12, 0, 0);
            DateTime EndTime = new DateTime(2023, 1, 3, 12, 0, 0);

            Harbor harbor = new Harbor("Cdull Harbor");
            //harbor.SetUpWatch(startTime, EndTime);
            Console.WriteLine($"Starttime: {startTime}");
            Console.WriteLine($"Endtime: {EndTime}");
            harbor.Watch.StartCountingTime();

            //setter opp noen et ship, og et dockslik at et skip kan docked: 
            Ship ship = new Ship("SKIPET", Enums.Model.ContainerShip, Enums.Size.Medium);
            Crane crane = new Crane("DOCK CRANE");
            Dock dock = new Dock("DOCKEN", Enums.Size.Medium, Enums.Model.ContainerShip, crane);

            // legger til shipet i harbor
            harbor.Ships.Add(ship);

            // legger til shipet i vente-kø for å docke 
            harbor.QueueShipsToDock();
            Console.WriteLine($"Waiting ships in harbor: {harbor.WaitingShips.Count}");
            harbor.Docks.Add(dock);
            Console.WriteLine($"Docks in harbor: {harbor.Docks.Count}");

            // docker shipet: 
            harbor.DockShips();
            Console.WriteLine($"Docked ships in harbor: {harbor.DockedShips.Count}");
            TimeSpan elapsedTime = harbor.Watch.MeasureTimeElapsed();
            Console.WriteLine($"Elapsed time: {elapsedTime}"); 
            Console.WriteLine($"\n");
        }

        static void Scenario_3()
        {
            Console.WriteLine($"\n");
            // Scenario 3: 8 minutes elapses when small ships gets docked.
            Console.WriteLine("Scenario 3:  8 minutes elapses when small ships gets docked.");
            //CargoStorage harborCargoStorage = new("cargoStorage");  
            // Eksempel på starttidspunkt (1. januar 2024 kl. 00:00:00)
            // År , måned, dag 
            DateTime startTime = new DateTime(2023, 1, 1, 12, 0, 0);
            DateTime EndTime = new DateTime(2023, 1, 3, 12, 0, 0);

            Harbor harbor = new Harbor("Cdull Harbor");
            //harbor.SetUpWatch(startTime, EndTime);
            Console.WriteLine($"Starttime: {startTime}");
            Console.WriteLine($"Endtime: {EndTime}");
            harbor.Watch.StartCountingTime();

            //setter opp noen et ship, og et dockslik at et skip kan docked: 
            Ship ship = new Ship("SKIPET", Enums.Model.ContainerShip, Enums.Size.Large);
            Crane crane = new Crane("DOCK CRANE");
            Dock dock = new Dock("DOCKEN", Enums.Size.Large, Enums.Model.ContainerShip, crane);

            // legger til shipet i harbor
            harbor.Ships.Add(ship);

            // legger til shipet i vente-kø for å docke 
            harbor.QueueShipsToDock();
            Console.WriteLine($"Waiting ships in harbor: {harbor.WaitingShips.Count}");
            harbor.Docks.Add(dock);
            Console.WriteLine($"Docks in harbor: {harbor.Docks.Count}");

            // docker shipet: 
            harbor.DockShips();
            Console.WriteLine($"Docked ships in harbor: {harbor.DockedShips.Count}");
            TimeSpan elapsedTime = harbor.Watch.MeasureTimeElapsed();
            Console.WriteLine($"Elapsed time: {elapsedTime}");
            Console.WriteLine($"\n");
        }

       
    }
}