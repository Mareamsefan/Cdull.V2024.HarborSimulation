using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.ShipOperations
{
    internal static class ScheduledSailingsExecutor
    {
        /// <summary>
        /// Executes the scheduled sailings for ships in the harbor based on their respective scheduled sailings.
        /// </summary>
        /// <param name="harbor">The harbor where the ships are docked.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="harbor"/> is null.</exception>
        public static void ExecuteScheduledSailings(Harbor harbor)
        {
            if (harbor == null)
            {
                throw new ArgumentNullException(nameof(harbor), "Harbor cannot be null.");
            }

            foreach (Ship ship in harbor.Ships)
            {
                if (ship.ScheduledSailings.Any()) // Use ToList() to allow modifications during iteration
                {
                    Sailing sailing = ship.ScheduledSailings.First(); 
                    if (sailing.RecurringType == RecurringType.Weekly && harbor.GetCurrentTime().DayOfWeek != sailing.DateTime.DayOfWeek)
                    {
                        continue;
                    }
                    // Check if it's time to sail
                    if ((harbor.GetCurrentTime().Hour == sailing.DateTime.Hour && harbor.GetCurrentTime().Minute == sailing.DateTime.Minute
                        && harbor.GetCurrentTime().Second == sailing.DateTime.Second))
                    {
                        // Check if ship is ready to sail and is not already sailing
                        if (ship.IsReadyToSail && !ship.IsSailing)
                        {
                            if (harbor.RemoveShipFromDock(ship))
                            {
                                // Perform sailing operations
                                PerformSailingOperations(ship, harbor, sailing);
                                if (sailing.RecurringType == RecurringType.None)
                                {
                                    ship.ScheduledSailings.Remove(sailing);
                                }
                            }
                        }
                      
                    }
                    else if (ship.HasReachedDestination)
                    {
                        ship.IsSailing = false;
                        harbor.SailingShips.Remove(ship);
                        harbor.QueueShipsToDock();
                    }
                    else
                    {
                        ship.IsWaitingForSailing = true;
                    }
                }
            }
        }

        /// <summary>
        /// Determines if it's time for a ship to sail based on the current time and scheduled sailing time.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <param name="sailing">The details of the scheduled sailing.</param>
        /// <returns>True if it's time for the ship to sail; otherwise, false.</returns>
        private static bool IsTimeToSail(DateTime currentTime, Sailing sailing)
        {
            if (currentTime.Hour == sailing.DateTime.Hour && currentTime.Minute == sailing.DateTime.Minute
                       && currentTime.Second == sailing.DateTime.Second)
            {
                return true;
            }
            else { return false; }
        }

        /// <summary>
        /// Performs sailing operations for a ship based on the scheduled sailing details.
        /// </summary>
        /// <param name="ship">The ship to sail.</param>
        /// <param name="harbor">The harbor where the ship is located.</param>
        /// <param name="sailing">The details of the scheduled sailing.</param>
        private static void PerformSailingOperations(Ship ship, Harbor harbor, Sailing sailing)
        {
            Driver driver = new Driver();
            HistoryHandler historyHandler = HistoryHandler.GetInstance();
            driver.Move(sailing.DestinationLocation, ship.Speed);
            ship.SailedAtTime = harbor.GetCurrentTime().ToString();
            historyHandler.AddEventToShipHistory(ship, $"{ship.Name} Sailed at {ship.SailedAtTime} to destination:{sailing.DestinationLocation}");
            ship.IsSailing = true;
            harbor.SailingShips.Add(ship);
            harbor.RaiseShipDeparted(ship);
            ship.HasReachedDestination = true;

        }
    }
}
    

