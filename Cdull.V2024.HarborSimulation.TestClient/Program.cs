using System;
using System.Collections;
using System.Collections.Generic;
using Cdull.V2024.HarborSimulation.SimulationFramework;
using Cdull.V2024.HarborSimulation.SimulationFramework.Enums;
using Cdull.V2024.HarborSimulation.SimulationFramework.Events;
using Cdull.V2024.HarborSimulation.TestClient;

namespace HarborSimulationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Harbor harbor = new Harbor("ExceptiontestHarbor", new CargoStorage("cargo", 10000));

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

            harbor.DepartedShip += Harbor_ShipDeparted;
            harbor.ArrivedShip += Harbor_ShipArrived;
            harbor.CompletedUnloadingShip += Harbor_ShipCompletedUnloading;
            harbor.CompletedloadingShip += Harbor_ShipCompletedLoading;

            Sailing sailing = Sailing.GetInstance();
            sailing.ScheduleSailing(harbor, Model.ContainerShip, new DateTime(2024, 1, 2),50, RecurringType.Weekly);
            sailing.ScheduleSailing(harbor, Model.LNGCarrier, new DateTime(2024, 1, 2), 40, RecurringType.Daily);


            // Running the simulation: 
            driver.Run(harbor, startTime, endTime, ships, docks);

            //Tester at den nye generiske historikk klassen funker for ship og harbor. 
            HistoryHandler  historyHandler = HistoryHandler.GetInstance();
  
           
            Console.WriteLine(historyHandler.GetHarborHistory(new DateTime(2024, 1, 2)));
            Ship ship1 = harbor.GetShips().First();
            Ship ship2 = harbor.GetShips().Last();
            Console.WriteLine(historyHandler.GetShipHistory(ship1));
            Console.WriteLine(historyHandler.GetShipHistory(ship2));
            foreach(Ship ship in harbor.GetShips())
            {

                Console.WriteLine(historyHandler.GetShipHistory(ship));
            }
       
        }

        private static void Harbor_ShipDeparted(object? sender, ShipDepartureEventArgs e)
        {
            Console.WriteLine($"Ship '{e.DepartedShip}' departed harbor.");
        }

        private static void Harbor_ShipArrived(object? sender, ShipArrivalEventArgs e)
        {

            Console.WriteLine($"Ship '{e.ArrivedShip}' arrived harbor.");
        }

        private static void Harbor_ShipCompletedUnloading(object? sender, ShipUnloadingEventArgs e)
        {

            Console.WriteLine($"Ship '{e.CompletedUnloadingShip}' completed unloading cargo.");
        }
        private static void Harbor_ShipCompletedLoading(object? sender, ShipLoadingEventArgs e)
        {

            Console.WriteLine($"Ship '{e.CompletedLoadingShip}' completed loading cargo.");
        }
    }
}
