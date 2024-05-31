using Cdull.V2024.HarborSimulation.SimulationFramework.ShipOperations;
using Cdull.V2024.HarborSimulation.SimulationFramework.ContainerOperations;
using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;


namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    /// <summary>
    /// Represents the simulation of a harbor.
    /// </summary>
    public class Simulation : IHarborSimulation
    {
        /// <summary>
        /// Runs the harbor simulation from the specified start time to the end time.
        /// </summary>
        /// <param name="harbor">The harbor instance to simulate.</param>
        /// <param name="startTime">The start time of the simulation.</param>
        /// <param name="endTime">The end time of the simulation.</param>
        /// <param name="ships">The list of ships in the harbor.</param>
        /// <param name="docks">The list of docks in the harbor.</param>
        /// <exception cref="ArgumentNullException">Thrown when the harbor, list of ships, or list of docks is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the start time is after the end time or if either the list of ships or list of docks is empty.</exception>
        /// <example>
        /// This example shows how to use the Run method.
        /// <code>
        /// IHarborSimulation driver = new Simulation();
        /// driver.Run(harbor, startTime, endTime, ships, docks, agvs, storageColumns);
        /// </code>
        /// </example>
        public void Run(Harbor harbor, DateTime startTime, DateTime endTime, List<Ship> ships, List<Dock> docks, List<AGV> agvs, List<StorageColumn> storageColumns)
        {
      
            if (harbor == null)
            {
                throw new ArgumentNullException(nameof(harbor), "Harbor instance cannot be null.");
            }

            if (ships == null)
            {
                throw new ArgumentNullException(nameof(ships), "List of ships cannot be null.");
            }

            if (docks == null)
            {
                throw new ArgumentNullException(nameof(docks), "List of docks cannot be null.");
            }

            if (agvs == null)
            {
                throw new ArgumentNullException(nameof(agvs), "List of agvs cannot be null.");
            }


            if (startTime > endTime)
            {
                throw new ArgumentException("Start time cannot be after end time.", nameof(startTime));
            }

            HistoryHandler historyHandler = HistoryHandler.GetInstance();
            ContainerHandler containerHandler = ContainerHandler.GetInstance();

            Driver driver = new Driver(); 
            Docking docking = new Docking();
            harbor.SetCurrentTime(startTime);

       
            harbor.GetWaitingShips.Clear();
            harbor.GetShips.Clear();
            harbor.GetDocks.Clear();
            harbor.GetDockedShips.Clear();
            harbor.GetSailingShips.Clear();

      
            harbor.GetDocks.AddRange(docks);
            harbor.GetShips.AddRange(ships);
            harbor.GetAGVs.AddRange(agvs);

            foreach(StorageColumn storageColumn in storageColumns)
            {
               harbor.GetContainerStorage.AddStorageColumn(storageColumn);
            }



            while (harbor.GetCurrentTime < endTime)
            {



                if (harbor.GetCurrentTime.Hour == 0 && harbor.GetCurrentTime.Minute == 0 && harbor.GetCurrentTime.Second == 0)
                {
                    historyHandler.SaveHarborHistory(harbor.GetCurrentTime, harbor);
                    harbor.GetContainerStorage.GetStorageColumns.ForEach(storageColumn =>
                    {
                        storageColumn.GetContainers.ForEach(container =>
                        {
                            int updatedDaysInStorage = container.GetNumberOfDaysInStorage + 1;
                            container.UpdateNumberofDaysInStorage(updatedDaysInStorage);
                        });
                    });

                }
                harbor.GetContainerStorage.GetStorageColumns.ForEach(storageColumn =>
                {

                    var containersCopy = storageColumn.GetContainers.ToList();

                    containersCopy.ForEach(container =>
                    {
                        if (container.GetNumberOfDaysInStorage == 4)
                        {
                            containerHandler.RemovePercentageOfContainersFromSource(0.1m, storageColumn: storageColumn);
                        }
                    });
                });


                harbor.QueueShipsToDock();

                if (harbor.GetWaitingShips.Any())
                {
                    Ship ship = harbor.GetWaitingShips.First();
                    if (!driver.Move(ship.GetCurrentLocation, ship.GetSpeed))
                    {
                        docking.DockShip(harbor, ship);
                        containerHandler.RemovePercentageOfContainersFromSource(0.15m, ship);

                    }

                    

                }

                if (harbor.GetDockedShips.Any())
                {
                    harbor.GetDockedShips.ForEach(ship =>
                    { 
                        ship.GetDockedAt.UpdateNumberOfShipsPerDay(ship.GetDockedAt.GetNumberOfShipsPerDay+1);

                        if (!ship.GetModel.Equals(Model.ContainerShip))
                        {
                            ship.SetIsReadyToSail(true);
                        }
                    });
                 
                }

                if (harbor.GetDockedShips.Any())
                {
                    Ship ship = harbor.GetDockedShips.First();
                    
                    if (ship.ScheduledContainerHandlings.Any())
                    {
                        ScheduledContainerHandling scheduledContainerHandling = ship.ScheduledContainerHandlings.First(); 
                        AGV agv = harbor.GetAvailableAGV();
                        if (ship.GetContainers.Any()) 
                        {

                            if (scheduledContainerHandling.HandlingTime.Date == harbor.GetCurrentTime.Date
                                && scheduledContainerHandling.HandlingTime.Hour == harbor.GetCurrentTime.Hour &&
                               
                                scheduledContainerHandling.LoadingType == LoadingType.Unload)
                            {
                                Container container = ship.GetContainers.First();
                                containerHandler.MoveContainerFromShipToAGV(ship, container, harbor);

                                if (agv.GetContainer != null)
                                {
                                    containerHandler.MoveContainerFromAGVToStorageColumn(harbor, 1, 3, agv);
                                }

                                StorageColumn storageColumn = harbor.GetContainerStorage.GetSpecificColumn(1);


                                Console.WriteLine(storageColumn.GetContainers.Count);
                               
                            }


                        }
                        

                        if (ship.GetContainers.Count == 0)
                        {
                            
                            if (scheduledContainerHandling.HandlingTime.Date == harbor.GetCurrentTime.Date &&
                                  scheduledContainerHandling.LoadingType == LoadingType.Load)
                            {

                                StorageColumn storagecolumn = harbor.GetContainerStorage.GetStorageColumns.First();
                                Console.WriteLine("LOADTEST");
                                if (storagecolumn.GetContainers.Any())
                                {
                                    Console.WriteLine("LOADTEST");
                                    Container container = storagecolumn.GetContainers.First();
                                    containerHandler.MoveContainerFromStorageColumnToAGV(1, harbor);
                                    if (agv.GetContainer != null)
                                    {
                                        containerHandler.MoveContainerFromAGVToShip(harbor, agv, ship);
                                    }


                                }


                            }
                        }
                    }
               

                 
                }
               

                ScheduledSailingsExecutor.ExecuteScheduledSailings(harbor);

                harbor.SetCurrentTime(harbor.GetCurrentTime.AddSeconds(1));

            }
        }
    }
}
