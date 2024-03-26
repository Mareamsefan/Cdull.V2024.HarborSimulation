using System;
using System.Collections.Generic;
using System.Text;
using Cdull.V2024.HarborSimulation.SimulationFramework;
using Cdull.V2024.HarborSimulation.SimulationFramework.Enums;

namespace Cdull.V2024.HarborSimulation.TestClient
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
        public void Run(Harbor harbor, DateTime startTime, DateTime endTime, List<Ship> ships, List<Dock> docks, List<AGV> agvs)
        {
            // Ensure harbor, ships, and docks are not null
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

            while (harbor.GetCurrentTime() < endTime)
            {
        
                //10% av container fjernes 
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
                    storageColumn.Containers.ForEach(container =>
                    {
                        if(container.numberOfDaysInStorage == 4)
                        {
                            //Console.WriteLine("IT WORKS GUYS :))))"); 
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
                      
                    }
                   

                }
                if (harbor.DockedShips.Any())
                {
                   containerHandler.PerformScheduledContainerHandling(harbor); 
                    
                    harbor.DockedShips.ForEach(ship =>
                    {
                        if (!ship.Model.Equals(Model.ContainerShip))
                        {
                            ship.IsReadyToSail = true;
                        }
                    });
                    
                    foreach (var key in containerHandler.ScheduledContainerHandling.Keys.ToList()) 
                    {
                        Ship ship = key.Item1;
                        DateTime handlingTime = key.Item2;
                        var handlingInfo = containerHandler.ScheduledContainerHandling[key];

                        if (harbor.GetCurrentTime().Date == handlingTime.Date)
                        {
                            foreach (var info in handlingInfo)
                            {
                                int startColumnId = info.Item1;
                                int endColumnId = info.Item2;
                                int numberOfContainers = info.Item3;
                                LoadingType loadingType = info.Item4;
                                Console.WriteLine(harbor.CurrentTime);
                                if (loadingType == LoadingType.Load)
                                {
                                    Console.WriteLine("THIS WORKS 22222 --->");
                                }
                                else if (loadingType == LoadingType.Unload)
                                {
                                    Console.WriteLine("THIS WORKS 22222");
                                }

                                containerHandler.ScheduledContainerHandling.Remove(key);
                            }
                        }
                    }


                    sailing.StartScheduledSailings(harbor, historyHandler);
                }




                harbor.SetCurrentTime(harbor.GetCurrentTime().AddSeconds(1));

            }
        }
    }
}
