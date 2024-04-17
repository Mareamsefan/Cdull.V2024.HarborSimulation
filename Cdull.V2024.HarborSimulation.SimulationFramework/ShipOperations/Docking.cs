using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.ShipOperations
{
    /// <summary>
    /// Represents a docking operation in the harbor.
    /// </summary>
    internal class Docking
    {

        /// <summary>
        /// Attempts to dock a ship at the specified dock in the harbor.
        /// </summary>
        /// <param name="ship">The ship to be docked.</param>
        /// <param name="availableDock">The available dock to dock the ship.</param>
        /// <param name="harbor">The harbor object containing available docks and ships.</param>
        /// <returns>True if the ship successfully docks, otherwise false.</returns>
        /// <exception cref="InvalidOperationException">Thrown when there is an error updating the ship's status.</exception>
        private bool Dock(Ship ship, Dock availableDock, Harbor harbor)
        {
            HistoryHandler historyHandler = HistoryHandler.GetInstance();
            ship.HasDocked = true;
            ship.DockedAt = availableDock;
            availableDock.IsAvailable = false;
            availableDock.OccupiedBy = ship;
            ship.DockedAtTime = harbor.CurrentTime.ToString();

            try
            {
                historyHandler.AddEventToShipHistory(ship, $"{ship.Name} Docked at {ship.DockedAtTime} on {ship.DockedAt.Name}");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to add event to ship's history.", ex);
            }
            try
            {
                harbor.WaitingShips.Dequeue();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to dequeue ship from waiting ships queue.", ex);
            }

            return true;
        }

        /// <summary>
        /// Attempts to dock ships waiting in the queue to available docks in the harbor.
        /// </summary>
        /// <remarks>
        /// This method iterates through the ships in the waiting queue and attempts to dock them to available docks in the harbor. 
        /// It checks if there is an available dock of the appropriate size for each ship and if the ship is not currently sailing.
        /// If a ship successfully docks, it is removed from the queue and added to the list of docked ships.
        /// If there are no available docks or the ship is currently sailing, it remains in the queue.
        /// </remarks>
        internal void DockShip(Harbor harbor, Ship ship)
        {
            Dock availableDock = harbor.AvailableDockOfSize(ship.Size);

            if (availableDock is not null && !ship.IsSailing)
            {
                if (Dock(ship, availableDock, harbor))
                {
                    harbor.DockedShips.Add(ship);
                }
            }
            else
            {
                harbor.WaitingShips.Enqueue(harbor.WaitingShips.Dequeue());
            }
        }
    }
}
