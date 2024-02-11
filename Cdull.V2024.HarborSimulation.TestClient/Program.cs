using System;
using System.Collections.Generic;
using Cdull.V2024.HarborSimulation.SimulationFramework;
using Cdull.V2024.HarborSimulation.TestClient;
using static Cdull.V2024.HarborSimulation.SimulationFramework.Enums;

namespace HarborSimulationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Opprett en instans av Harbor-klassen
            Harbor harbor = new Harbor("Test Harbor", new CargoStorage("CargoStorage", 100));

            // Opprett noen dokker
            List<Dock> docks = harbor.InitializeDocks(5, Model.ContainerShip, Size.Large, 2);

            // Opprett noen skip
            List<Ship> ships = harbor.InitializeShips(5, Model.ContainerShip, Size.Large, 10);

           
            IHarborSimulation driver = new Simulation();

            // Sett start- og slutttidspunkter for simuleringen
            DateTime startTime = new DateTime(2024, 1, 1, 0, 0, 0);
            DateTime endTime = new DateTime(2024, 1, 5, 0, 0, 0);

            

            DateTime startSailingTime = new DateTime(2024, 1, 2, 0, 0, 0); 

            // Kjør simuleringen
            driver.Run(harbor, startTime, endTime, ships, docks, startSailingTime, 2);

            foreach(Ship ship  in harbor.GetShips()) {
                Console.WriteLine(ship.GetDockedAtTime()); 
            }
          

            
            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 1)));

            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 2)));

            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 3)));

            Console.WriteLine(harbor.GetHarborHistory(new DateTime(2024, 1, 4)));
            


        }
    }
}
