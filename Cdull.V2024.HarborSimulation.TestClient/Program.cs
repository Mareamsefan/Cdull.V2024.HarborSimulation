using Cdull.V2024.HarborSimulation.SimulationFramework;
using System;
using System.Collections.Generic;

namespace Cdull.V2024.HarborSimulation.TestClient
{
    internal class Program
    {

        static void Main(string[] args)
        {


            IHarborSimulation driver = new Simulation();

            DateTime startTime = new DateTime(2024, 1, 1);
            DateTime endTime = new DateTime(2024, 1, 5);

            CargoStorage cargoStorage = new CargoStorage("CargoStorage", 100);
            Harbor testHarbor = new("MARI HARBOR", cargoStorage);

            List<Dock> docks = new List<Dock>();
            List<Ship> ships = new List<Ship>();

            docks.AddRange(testHarbor.InitializeDocks(10, Enums.Model.RoRo, Enums.Size.Medium, 10));
            docks.AddRange(testHarbor.InitializeDocks(10, Enums.Model.ContainerShip, Enums.Size.Small, 10));
            docks.AddRange(testHarbor.InitializeDocks(10, Enums.Model.Bulker, Enums.Size.Large, 10));
            docks.AddRange(testHarbor.InitializeDocks(10, Enums.Model.Tanker, Enums.Size.Large, 10));
            docks.AddRange(testHarbor.InitializeDocks(10, Enums.Model.LNGCarrier, Enums.Size.Large, 10));

            ships.AddRange(testHarbor.InitializeShips(10, Enums.Model.RoRo, Enums.Size.Medium, 10));
            ships.AddRange(testHarbor.InitializeShips(10, Enums.Model.ContainerShip, Enums.Size.Small, 10));
            ships.AddRange(testHarbor.InitializeShips(10, Enums.Model.Bulker, Enums.Size.Large, 10));
            ships.AddRange(testHarbor.InitializeShips(10, Enums.Model.Tanker, Enums.Size.Large, 10));
            ships.AddRange(testHarbor.InitializeShips(10, Enums.Model.LNGCarrier, Enums.Size.Large, 10));

           

            driver.Run(testHarbor, startTime, endTime, ships, docks);


           
            //Console.WriteLine(testHarbor.DockShips);
            //Console.WriteLine(testHarbor.GetDocks().Count);
            
            
            DateTime date1 = new DateTime(2024, 1, 2);
            Console.WriteLine($"Harbor history for date: {date1}" + testHarbor.GetHarborHistory(date1));
            
            DateTime date2 = new DateTime(2024, 1, 3);
            Console.WriteLine($"Harbor history for date:{date2}"+testHarbor.GetHarborHistory(date2));
            DateTime date3 = new DateTime(2024, 1, 4);
            Console.WriteLine($"Harbor history for date:{date3}"+testHarbor.GetHarborHistory(date3));
            
         
           

        }

        static void TestingShipHandler() {
           

        }



    }
}