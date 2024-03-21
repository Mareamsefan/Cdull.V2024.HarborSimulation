using Cdull.V2024.HarborSimulation.SimulationFramework.Enums;
using Cdull.V2024.HarborSimulation.SimulationFramework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class ContainerHandler
    {
        private static readonly ContainerHandler instance = new ContainerHandler();
        private Dictionary<Ship, List<(DateTime, int, LoadingType)>> ScheduledContainerHandling = new Dictionary<Ship, List<(DateTime, int, LoadingType)>>();

        /// <summary>
        /// Private constructor to prevent external instantiation.
        /// </summary>
        private ContainerHandler() { }

        /// <summary>
        /// Gets the singleton instance of the <see cref="HistoryHandler"/> class.
        /// </summary>
        /// <returns>The singleton instance of the <see cref="HistoryHandler"/> class.</returns>
        /// https://csharpindepth.com/articles/singleton (hentet: 03.03.2024) (Skeet.Jon, 2019)
        public static ContainerHandler GetInstance()
        {
            return instance;
        }



        /// <summary>
        /// Adds container from docked ships to the container storage in the harbor.
        /// </summary>
        /// <returns>True if container is successfully added to the storage; otherwise, false.</returns>
        /// <remarks>
        /// This method checks for docked ships and unloads container from them into the container storage.
        /// It removes container items from the ships and adds them to the container storage.
        /// The method also raises an event when unloading is completed for a ship.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown when there are no docked ships in the harbor.</exception>
        /// <exception cref="StorageSpaceException">Thrown when there is not enough space in the container storage to add all container from the ship.</exception>
        internal bool AddContainerToStorage(Ship ship, Harbor harbor)
        {
            if (ship == null)
            {
                throw new ArgumentNullException(nameof(ship), "Ship cannot be null.");
            }

            if (!harbor.DockedShips.Contains(ship))
            {
                throw new ArgumentException("Ship is not docked in the harbor.", nameof(ship));
            }

            if (ship.Containers.Any() && !ship.IsUnloadingCompleted)
            {
                int totalContainerCount = ship.Containers.Count;
                if (harbor.ContainerStorage.GetSpecificColumn(1).GetOccupiedSpace() + totalContainerCount <= harbor.ContainerStorage.GetSpecificColumn(1).Capacity)
                {
                    foreach (Container container in ship.Containers.ToList())
                    {
                        StorageColumn storageColumn = harbor.ContainerStorage.GetSpecificColumn(1);

                        storageColumn.AddCargo(container);
                        storageColumn.OccupySpace(container);
                        ship.Containers.Remove(container);
                    }

                    ship.IsUnloadingCompleted = true;
                    harbor.RaiseShipCompletedUnloading(ship);
                    return true;
                }
                else
                {
                    throw new StorageSpaceException("Not enough space in CargoStorage to add all container from the ship.");
                }
            }
            return false;
        }




        /// <summary>
        /// Adds container from the container storage to the docked ships.
        /// </summary>
        /// <param name="numberOfContainers">The maximum number of container items to add to each ship.</param>
        /// <remarks>
        /// This method iterates through all docked ships and attempts to add container to each ship up to the specified limit.
        /// It removes container items from the container storage and adds them to the ship's container list.
        /// The method also updates container history with Loading information.
        /// </remarks>
        /// <exception cref="ArgumentException">Thrown when the number of container to add is not greater than zero.</exception>
        /// <exception cref="InsufficientCargoException">Thrown when there is not enough container in the container storage.</exception>
        internal void AddContainerToShip(Ship ship, int numberOfContainers, Harbor harbor)
        {
            if (numberOfContainers <= 0)
            {
                throw new ArgumentException("Number of containers to add must be greater than zero.");
            }

            if (ship != null)
            {

                if (ship.Containers.Count < numberOfContainers)
                {

                    for (int i = 0; i < numberOfContainers; i++)
                    {

                        if (harbor.ContainerStorage.GetSpecificColumn(1).Containers.Count > 0 && !ship.IsLoadingCompleted)
                        {

                            Container container = harbor.ContainerStorage.GetSpecificColumn(1).Containers.First();
                            ship.Containers.Add(container);
                            harbor.ContainerStorage.GetSpecificColumn(1).RemoveCargo(container);
                            harbor.ContainerStorage.GetSpecificColumn(1).deOccupySpace(container);
                            container.History.Add($"{container.Name} loaded at {harbor.CurrentTime} on {ship.Name}");
                        }
                        else
                        {
                            throw new InsufficientCargoException("Not enough containers in cargostorage");
                        }
                    }
                    ship.IsReadyToSail = true;
                    ship.IsLoadingCompleted = true;
                    if (ship.IsUnloadingCompleted)
                    {
                        harbor.RaiseShipCompletedLoading(ship);
                    }

                }

            }
            else
            {
                throw new ArgumentNullException(nameof(ship), "Ship parameter cannot be null.");
            }
        }
        
        public void ScheduleContainerHandling(Ship ship, ContainerStorage containerStorage,  DateTime handlingTime, int startColumnId, 
            int endColumnId, LoadingType loadingType, Harbor harbor)
        {
            // Sjekk for ugyldige parametere
            if (handlingTime < harbor.CurrentTime)
            {
                throw new ArgumentException("Handling time cannot be in the past.");
            }

            if (startColumnId < 0 || endColumnId < 0)
            {
                throw new ArgumentException("Column IDs cannot be negative.");
            }

            if (startColumnId > endColumnId)
            {
                throw new ArgumentException("Start column ID cannot be greater than end column ID.");
            }

            // Gå gjennom alle kolonner i området og planlegg containerhåndtering
            for (int columnId = startColumnId; columnId <= endColumnId; columnId++)
            {
                // Finn riktig kolonne i lagringen
                StorageColumn storageColumn = containerStorage.GetSpecificColumn(columnId);

                // Sjekk om kolonnen eksisterer
                if (storageColumn == null)
                {
                    throw new ArgumentException($"Storage column with ID {columnId} not found.");
                }
                // Planlegg containerhåndtering i kolonnen
                if (!ScheduledContainerHandling.ContainsKey(ship))
                {
                    ScheduledContainerHandling[ship] = new List<(DateTime, int, LoadingType)>();
                }

                ScheduledContainerHandling[ship].Add((handlingTime, columnId, loadingType));
            }
        
        }
        
        public List<(DateTime, int, LoadingType)> CheckScheduledCargoHandling(Ship ship)
        {
            if (!ScheduledContainerHandling.ContainsKey(ship))
            {
                // Returner en tom liste hvis det ikke er noen planlagte seilaser for denne skipstypen
                return new List<(DateTime, int, LoadingType)>();
            }

            return ScheduledContainerHandling[ship];
        }
        
    }
}
