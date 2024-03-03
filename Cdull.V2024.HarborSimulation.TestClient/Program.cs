using System;
using System.Collections;
using System.Collections.Generic;
using Cdull.V2024.HarborSimulation.SimulationFramework;
using Cdull.V2024.HarborSimulation.TestClient;

namespace HarborSimulationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Harbor harbor = new Harbor("ExceptiontestHarbor", new CargoStorage("cargo", 1000));

            List<Dock> docks = harbor.InitializeDocks(10, Model.ContainerShip, Size.Large, 2);


            // Made a list of 5 ships of the same Type and Size: 
            List<Ship> ships = harbor.InitializeShips(5, Model.ContainerShip, Size.Large, 100);
            ships.AddRange(harbor.InitializeShips(5, Model.LNGCarrier, Size.Medium, 50)); 
            //Made a instance of our simulation - class that implements IHarborSimulation interface:  
            IHarborSimulation driver = new Simulation();

            // Made a startTime and a endTime for mye simulation: 
            DateTime startTime = new DateTime(2024, 1, 1);
            DateTime endTime = new DateTime(2024, 1, 10);

            // Made a startTime for when sailing starts for all ships in harbor non-recurring sailing: 
            DateTime startSailingTime = new DateTime(2024, 1, 2);

            harbor.ShipDeparted += Harbor_ShipDeparted;

            Sailing sailing = Sailing.GetInstance();
            sailing.ScheduleSailing(Model.ContainerShip, new DateTime(2024, 1, 2), RecurringType.Weekly);
            sailing.ScheduleSailing(Model.LNGCarrier, new DateTime(2024, 1, 2), RecurringType.Daily);


            // Runing the simulation: 
            driver.Run(harbor, startTime, endTime, ships, docks, 2000);

            //Tester at den nye generiske historikk klassen funker for ship og harbor. 
            HistoryHandler  historyHandler = HistoryHandler.GetInstance();
  

            Console.WriteLine(historyHandler.GetHarborHistory(new DateTime(2024, 1, 2)));
            Ship ship = harbor.GetShips().First();

            Console.WriteLine(historyHandler.GetShipHistory(ship));
        }

        private static void Harbor_ShipDeparted(object? sender, ShipDepartureEventArgs e)
        {
            Console.WriteLine($"Ship '{e.DepartedShip}' left harbor.");
        }

    }
}
