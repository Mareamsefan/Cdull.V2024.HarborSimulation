using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.ShipOperations
{
    internal static class ScheduledSailingsExecutor
    {
        /// <summary>
        /// Executes the scheduled sailings for ships in the harbor based on their respective scheduled sailings.
        /// </summary>
        /// <param name="harbor">The harbor where the ships are docked.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="harbor"/> is null.</exception>
        internal static void ExecuteScheduledSailings (Harbor harbor)
        {
            if (harbor == null)
            {
                throw new ArgumentNullException(nameof(harbor), "Harbor cannot be null.");
            }
            Driver driver = new Driver();
            foreach (Ship ship in harbor.GetShips)
            {
                if(ship.ScheduledSailings.Count != 0) 
                {
                    Sailing sailing = ship.ScheduledSailings.First();
                    if (sailing.RecurringType == RecurringType.Weekly && harbor.GetCurrentTime.DayOfWeek != sailing.DateTime.DayOfWeek)
                    {   
                        continue;
                    }


                    if (IsTimeToSail(harbor.GetCurrentTime, sailing, sailing.RecurringType))
                    {
 
                        if (ship.GetIsReadyToSail && !ship.GetIsSailing)
                        {
                            if (harbor.RemoveShipFromDock(ship))
                            {
                                PerformSailingOperations(ship, harbor, sailing);
                              
                             
                                if (sailing.RecurringType == RecurringType.None)
                                {
                                    ship.ScheduledSailings.Remove(sailing);
                                }
                            }
                        }

                    }

                    else if (ship.GetHasReachedDestination && ship.GetSailingState.Equals(ShipSailingState.Arrived))
                    {
                        ship.SetIsSailing(false);
                        harbor.GetSailingShips.Remove(ship);
                        harbor.QueueShipsToDock();
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
        private static bool IsTimeToSail(DateTime currentTime, Sailing sailing, RecurringType recurring)
        {
            if(recurring.Equals(RecurringType.Weekly) || recurring.Equals(RecurringType.Daily))
            {
                return currentTime.Hour == sailing.DateTime.Hour && currentTime.Minute == sailing.DateTime.Minute
                      && currentTime.Second == sailing.DateTime.Second;
            }
            else
            {
                return currentTime.Date == sailing.DateTime.Date && currentTime.Hour == sailing.DateTime.Hour && currentTime.Minute == sailing.DateTime.Minute
                       && currentTime.Second == sailing.DateTime.Second;
            }
        }

        /// <summary>
        /// Performs sailing operations for a ship based on the scheduled sailing details.
        /// </summary>
        /// <param name="ship">The ship to sail.</param>
        /// <param name="harbor">The harbor where the ship is located.</param>
        /// <param name="sailing">The details of the scheduled sailing.</param>
        private static void PerformSailingOperations(Ship ship, Harbor harbor, Sailing sailing)
        {
            HistoryHandler historyHandler = HistoryHandler.GetInstance();
            ship.SetSailedAtTime(harbor.GetCurrentTime.ToString());
            historyHandler.AddEventToShipHistory(ship, $"{ship.GetName} Sailed at {ship.GetSailedAtTime} to destination:{sailing.DestinationLocation}");
            ship.SetIsSailing(true);
            harbor.GetSailingShips.Add(ship);
            harbor.RaiseShipDeparted(ship);
            ship.SetHasReachedDestination(true);
            ship.SailingState = ShipSailingState.Arrived; 

        }
    }
}
    

