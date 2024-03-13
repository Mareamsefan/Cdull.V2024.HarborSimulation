using Cdull.V2024.HarborSimulation.SimulationFramework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class CargoHandler
    {
        Harbor harbor;


        public CargoHandler(Harbor CargoHandlerHarbor)
        {
            harbor = CargoHandlerHarbor; 
        }



        /// <summary>
        /// Adds cargo from docked ships to the cargo storage in the harbor.
        /// </summary>
        /// <returns>True if cargo is successfully added to the storage; otherwise, false.</returns>
        /// <remarks>
        /// This method checks for docked ships and unloads cargo from them into the cargo storage.
        /// It removes cargo items from the ships and adds them to the cargo storage.
        /// The method also raises an event when unloading is completed for a ship.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown when there are no docked ships in the harbor.</exception>
        /// <exception cref="StorageSpaceException">Thrown when there is not enough space in the cargo storage to add all cargo from the ship.</exception>
        internal bool AddCargoToStorage()
        {
            if (harbor.DockedShips.Any())
            {
                Ship ship = harbor.DockedShips.First();
                if (harbor.DockedShips.Count == 0)
                {
                    throw new InvalidOperationException("No docked ships in harbor to unload cargo from.");
                }

                if (ship.Cargo.Any() && !ship.IsUnloadingCompleted)
                {
                    int totalCargoCount = ship.Cargo.Count;
                    if (harbor.CargoStorage.GetOccupiedSpace() + totalCargoCount <= harbor.CargoStorage.Capacity)
                    {
                        foreach (var cargo in ship.Cargo.ToList())
                        {
                            harbor.CargoStorage.AddCargo(cargo);
                            harbor.CargoStorage.OccupySpace(cargo);
                            ship.Cargo.Remove(cargo);
                        }

                        ship.IsUnloadingCompleted = true;
                        if (ship.IsUnloadingCompleted)
                        {
                            harbor.RaiseShipCompletedUnloading(ship);
                        }
                        return true;
                    }
                    else
                    {
                        throw new StorageSpaceException("Not enough space in CargoStorage to add all cargo from the ship.");
                    }
                }
            }
            return false;
        }


        /// <summary>
        /// Adds cargo from the cargo storage to the docked ships.
        /// </summary>
        /// <param name="numberOfCargo">The maximum number of cargo items to add to each ship.</param>
        /// <remarks>
        /// This method iterates through all docked ships and attempts to add cargo to each ship up to the specified limit.
        /// It removes cargo items from the cargo storage and adds them to the ship's cargo list.
        /// The method also updates cargo history with loading information.
        /// </remarks>
        /// <exception cref="ArgumentException">Thrown when the number of cargo to add is not greater than zero.</exception>
        /// <exception cref="InsufficientCargoException">Thrown when there is not enough cargo in the cargo storage.</exception>
        internal void AddCargoToShips(int numberOfCargo)
        {
            if (numberOfCargo <= 0)
            {
                throw new ArgumentException("Number of cargo to add must be greater than zero.");
            }

            foreach (Ship ship in harbor.DockedShips)
            {
                if (ship.Cargo.Count < numberOfCargo)
                {
                    for (int i = 0; i < numberOfCargo; i++)
                    {
                        if (harbor.CargoStorage.Cargo.Count > 0 && !ship.IsLoadingCompleted)
                        {
                            Cargo cargo = harbor.CargoStorage.Cargo.First();
                            ship.Cargo.Add(cargo);
                            harbor.CargoStorage.RemoveCargo(cargo);
                            harbor.CargoStorage.deOccupySpace(cargo);
                            cargo.History.Add($"{cargo.Name} loaded at {harbor.CurrentTime} on {ship.Name}");
                        }
                        else
                        {
                            throw new InsufficientCargoException("Not enough cargo in cargostorage");
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
        }
    }
}
