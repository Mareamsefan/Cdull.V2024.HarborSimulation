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
            Sailing sailing = Sailing.GetInstance();

            Driver driver = new Driver(); 
            Docking docking = new Docking();
            harbor.SetCurrentTime(startTime);

       
            harbor.WaitingShips.Clear();
            harbor.Ships.Clear();
            harbor.Docks.Clear();
            harbor.DockedShips.Clear();
            harbor.SailingShips.Clear();

      
            harbor.Docks.AddRange(docks);
            harbor.Ships.AddRange(ships);
            harbor.AGVs.AddRange(agvs);
            foreach(StorageColumn storageColumn in storageColumns)
            {
               harbor.ContainerStorage.AddStorageColumn(storageColumn);
            }


    
            while (harbor.GetCurrentTime() < endTime)
            {
        

                
                if (harbor.GetCurrentTime().Hour == 0 && harbor.GetCurrentTime().Minute == 0 && harbor.GetCurrentTime().Second == 0)
                {
                    historyHandler.SaveHarborHistory(harbor.GetCurrentTime(), harbor);
                    harbor.ContainerStorage.StorageColumns.ForEach(storageColumn =>
                    {
                        storageColumn.Containers.ForEach(container =>
                        {
                            container.numberOfDaysInStorage++; 
                        });
                    }); 
                    
                }
                harbor.ContainerStorage.StorageColumns.ForEach(storageColumn =>
                {
            
                    var containersCopy = storageColumn.Containers.ToList();

                    containersCopy.ForEach(container =>
                    {
                        if (container.numberOfDaysInStorage == 4)
                        {
                            containerHandler.RemovePercentageOfContainersFromSource(0.1m, storageColumn: storageColumn);
                        }
                    });
                });


                harbor.QueueShipsToDock();

                if (harbor.WaitingShips.Any())
                {
                    Ship ship = harbor.WaitingShips.First();
                    if (!driver.Move(ship.CurrentLocation,ship.Speed))
                    {
                        docking.DockShip(harbor, ship);
                        containerHandler.RemovePercentageOfContainersFromSource(0.15m, ship);
                       
                    }
                   

                }
                if (harbor.DockedShips.Any())
                {
                    harbor.DockedShips.ForEach(ship =>
                    {
                        ship.DockedAt.numberOfShipsPerDay++;

                        if (!ship.Model.Equals(Model.ContainerShip))
                        {
                            ship.IsReadyToSail = true; 
                        }
                        if (ship.DockedAt.numberOfShipsPerDay < 7)
                        {
                            foreach (ScheduledContainerHandling scheduledContainerHandling in ship.ScheduledContainerHandlings)
                            {

                                if (harbor.GetCurrentTime().Date == scheduledContainerHandling.HandlingTime.Date)
                                {

                                    if (scheduledContainerHandling.LoadingType == LoadingType.Unload)
                                    {
                                        foreach (Container container in ship.Containers.ToList())
                                        {

                                            AGV agv = containerHandler.MoveContainerFromShipToAGV(ship, container, harbor);

                                            containerHandler.MoveContainerFromAGVToStorageColumn(harbor.ContainerStorage,
                                            scheduledContainerHandling.StartColumnId, scheduledContainerHandling.EndColumnId, agv);
                                        }
                                        harbor.RaiseShipCompletedUnloading(ship);

                                    }

                                    else if (scheduledContainerHandling.LoadingType == LoadingType.Load)
                                    {
                                        foreach (Container container in ship.Containers.ToList())
                                        {

                                            AGV agv = containerHandler.MoveContainerFromStorageColumnToAGV(harbor.ContainerStorage,
                                           scheduledContainerHandling.StartColumnId, harbor);
                                            containerHandler.MoveContainerFromAGVToShip(agv, ship);
                                        }
                                        harbor.RaiseShipCompletedUnloading(ship);
                                    }     
                                   
                                }
                            }
                        }
                    }); 
                  
                    sailing.StartScheduledSailings(harbor, historyHandler);
                }




                harbor.SetCurrentTime(harbor.GetCurrentTime().AddSeconds(1));

            }
        }
    }
}
