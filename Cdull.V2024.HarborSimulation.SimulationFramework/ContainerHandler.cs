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
        Harbor harbor;


        public ContainerHandler(Harbor CargoHandlerHarbor)
        {
            harbor = CargoHandlerHarbor;
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
        internal bool AddContainerToStorage(Ship ship)
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
        /// The method also updates container history with loading information.
        /// </remarks>
        /// <exception cref="ArgumentException">Thrown when the number of container to add is not greater than zero.</exception>
        /// <exception cref="InsufficientCargoException">Thrown when there is not enough container in the container storage.</exception>
        internal void AddContainerToShip(Ship ship, int numberOfContainers)
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
    }

}
