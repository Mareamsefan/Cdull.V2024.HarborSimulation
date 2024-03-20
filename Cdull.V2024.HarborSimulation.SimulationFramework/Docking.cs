using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    internal class Docking
    {


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
                historyHandler.AddEventToShipHistory(ship, $"{ship.Name} Docked at {ship.DockedAtTime} on {ship.DockedAt.Name} ship containers: {ship.Containers.Count}");
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
        /// <exception cref="InvalidOperationException">Thrown when there are no waiting ships in the queue.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the ship's docked status cannot be updated.</exception>
        /// <exception cref="InvalidOperationException">Thrown when an event cannot be added to the ship's history.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the ship cannot be added to the list of docked ships.</exception>
        /// <exception cref="InvalidOperationException">Thrown when a ship cannot be dequeued from the waiting queue.</exception>
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
