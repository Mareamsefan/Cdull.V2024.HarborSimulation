﻿
using Cdull.V2024.HarborSimulation.SimulationFramework;
using Cdull.V2024.HarborSimulation.SimulationFramework.ContainerOperations;
using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;
using Cdull.V2024.HarborSimulation.SimulationFramework.ShipOperations;

namespace Cdull.V2024.HarborSimulation.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {

            // Scenario setup:
            // Setting up a harbor with docks, ships, AGVs, and cargo handling operations.

            // Creating a container storage with a location range for the storage columns to be located. 
            // The capacity of the container storage is determined by the storage columns it contains.
            ContainerStorage containerStorage = new ContainerStorage("ContainerStorage", 0, 500, 3, 30);
            //Console.WriteLine(containerStorage.GetOccupiedSpace());

            // Creating a new harbor named "TestHarbor" with a location index range of 1000 (from 0-1000)

            Harbor harbor = new Harbor("TestHarbor", 1000, containerStorage, 4);


            // Creating 3 large docks with 7 cranes collectively.

            List<Dock> docks = harbor.InitializeDocks(1, Size.Large, 2);

            crane.GetId();

            crane.getHandlingTime();

            crane.setHandlingTime(60);

            crane.GetIsAvailable();

            crane.SetIsAvailable(true);

            crane.GetContainer();


            //docks.AddRange(harbor.InitializeDocks(1, Size.Large, 1));

            // Creating 20 AGVs for container movement.
            List<AGV> agvs = harbor.InitializeAGVs(1, 1000);

            // Creating a list to hold ships.
            List<Ship> ships = new List<Ship>();

            // Adding 5 medium-sized LNGCarrier ships with a current location 2000m away from the harbor.
            //ships.AddRange(harbor.InitializeShips(2000, 5, Model.LNGCarrier, Size.Medium));

            // Adding 5 large ContainerShip ships with 5 small containers each, located 1500m away from the harbor.
            //ships.AddRange(harbor.InitializeShips(1500, 5, Model.ContainerShip, Size.Large, 5, ContainerSize.Small));

            // Adding 5 large ContainerShip ships with 5 large containers each, located 1700m away from the harbor.
           // ships.AddRange(harbor.InitializeShips(1700, 5, Model.ContainerShip, Size.Large, 5, ContainerSize.Large));

            // Creating a test ContainerShip to demonstrate individual ship creation.
            Ship ContainerTestShip = new Ship("ContainerTestShip", Model.ContainerShip, Size.Small, 1000);
            ContainerTestShip.InitializeContainers(5, ContainerSize.Small);
            Ship LNGCarrierTestShip = new Ship("LNGCarrierTestShip", Model.LNGCarrier, Size.Medium, 2500);

          
            Console.WriteLine("CONTAINER COUNT "+ContainerTestShip.GetContainers.Count); 

            ships.Add(ContainerTestShip);
            //ships.Add(LNGCarrierTestShip);

            // Scheduling sailing for ContainerShip ships starting on January 2, 2024, with 50 ships, repeating weekly.
            // ScheduleSailing.ScheduleSailings(harbor, ships, Model.ContainerShip, new DateTime(2024, 1, 2), 50, RecurringType.Weekly);

            //Scheduling sailing for LNGCarrier ships starting on January 2, 2024, with 40 ships, repeating daily.
            ScheduleSailing.ScheduleSailings(harbor, ships, Model.LNGCarrier, new DateTime(2024, 1, 2), 40, RecurringType.Weekly);

            //ScheduleSailing.ScheduleOneSailing(harbor, LNGCarrierTestShip, new DateTime(2024, 1, 10), 50, RecurringType.None);
            ScheduleSailing.ScheduleOneSailing(harbor, ContainerTestShip, new DateTime(2024, 1, 2), 100, RecurringType.None); 
                
            // Creating an instance of the simulation driver.
            IHarborSimulation driver = new Simulation();

            // Setting the start and end times for the simulation.
            DateTime startTime = new DateTime(2024, 1, 1);
            DateTime endTime = new DateTime(2024, 1, 15);

            // Adding event handlers for ship departure, arrival, unloading, and loading in the harbor.
            harbor.DepartedShip += Harbor_ShipDeparted;
            harbor.ArrivedShip += Harbor_ShipArrived;
            harbor.CompletedUnloadingShip += Harbor_ShipCompletedUnloading;
            harbor.CompletedloadingShip += Harbor_ShipCompletedLoading;

           List<Sailing> sailings = ScheduleSailing.CheckScheduledSailings(harbor, ships, Model.LNGCarrier);
            sailings.ForEach(sailing =>
                Console.WriteLine(sailing.ToString())
            );
       
            // Setting up storage columns at specific locations.
            List<int> longColumnLocations = new List<int> { 37, 111, 185, 259, 333, 407};
            List<int> shortColumnLocations = new List<int> { 30, 74, 148, 222, 292, 270, 444};

            // Setting up 4 storage columns at each of the longColumnLocations, and 1 storage column at each of the 
            // shortColumnLocations, resulting in a total of 24 long storage columns and 7 small storage columns.
            List<StorageColumn> storageColumns = harbor.InitializeStorageColumns(
                longColumnLocations, shortColumnLocations, 18, 15, 4, 1, 6, 4);

            // Creating a container handler instance.
            ContainerHandler containerHandler = ContainerHandler.GetInstance();
            
     

            // Scheduling container handling operations for the test ship on specific dates.
           /* containerHandler.ScheduleContainerHandling(ContainerTestShip, new DateTime(2024, 1, 4), 1, 2, 10, LoadingType.Unload);
            containerHandler.ScheduleContainerHandling(ContainerTestShip, new DateTime(2024, 1, 7), 1, 2, 10, LoadingType.Load);
            */
            
            // Scheduling container handling operations for ContainerShip ships.
            ships.ForEach(ship =>
            {
                if (ship.GetModel.Equals(Model.ContainerShip))
                {
                    containerHandler.ScheduleContainerHandling(ship, new DateTime(2024, 1, 6), 2, 3, 10, LoadingType.Unload);
                    containerHandler.ScheduleContainerHandling(ship, new DateTime(2024, 1, 8), 2, 3, 10, LoadingType.Load);
                }
            });

            // Checking all scheduled cargo handling operations for the test ship.
           // Console.WriteLine(containerHandler.CheckScheduledContainerHandling(ContainerTestShip));

            // Running the simulation.
            driver.Run(harbor, startTime, endTime, ships, docks, agvs, storageColumns);

            // Testing the new generic history class for harbor and ships.
            HistoryHandler historyHandler = HistoryHandler.GetInstance();
           

            // Printing history for the harbor on January 2, 2024.
            Console.WriteLine(historyHandler.GetHarborHistory(new DateTime(2024, 1, 2)));

            // Retrieving the first ship in the harbor and printing its history.
            Ship ship1 = harbor.GetShips.First();
            Console.WriteLine(historyHandler.GetShipHistory(ship1));
            Console.WriteLine(ContainerTestShip.GetContainers.Count);

            // Retrieving the last ship in the harbor and printing its history.
            //Ship ship2 = harbor.GetShips().Last();
           // Console.WriteLine(historyHandler.GetShipHistory(ship2));

            // Printing history for all ships in the harbor.
            Console.WriteLine(historyHandler.GetShipsHistory());

           /* List<Sailing> sailingscheck = ScheduleSailing.CheckScheduledSailings(
                harbor, ships, Model.LNGCarrier);
            sailings.ForEach(sailing =>
                Console.WriteLine(sailing.ToString())

            );*/

            harbor.GetLocationRange();
        }





        private static void Harbor_ShipDeparted(object? sender, ShipDepartureEventArgs e)
        {
            Console.WriteLine($"\nShip '{e.DepartedShip}' departed harbor.\n");
        }

        private static void Harbor_ShipArrived(object? sender, ShipArrivalEventArgs e)
        {

            Console.WriteLine($"\nShip '{e.ArrivedShip}' arrived harbor.\n");
        }

        private static void Harbor_ShipCompletedUnloading(object? sender, ShipUnloadingEventArgs e)
        {

            Console.WriteLine($"\nShip '{e.CompletedUnloadingShip}' completed unloading containers.\n" );
        }
        private static void Harbor_ShipCompletedLoading(object? sender, ShipLoadingEventArgs e)
        {

            Console.WriteLine($"\nShip '{e.CompletedLoadingShip}' completed Loading containers.\n");
        }

    }
}
