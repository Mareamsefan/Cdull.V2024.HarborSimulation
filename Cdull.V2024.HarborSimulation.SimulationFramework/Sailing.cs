
using Cdull.V2024.HarborSimulation.SimulationFramework.Enums;
using Cdull.V2024.HarborSimulation.SimulationFramework.Exceptions;

/// <summary>
/// Represents the scheduling and execution of ship sailings.
/// </summary>
namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Sailing
    {
        private static readonly Sailing instance = new Sailing();
        private Dictionary<Model, List<(DateTime, int, RecurringType)>> ScheduledSailings { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Sailing"/> class.
        /// </summary>
        private Sailing()
        {
            ScheduledSailings = new Dictionary<Model, List<(DateTime, int, RecurringType)>>();
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="Sailing"/> class.
        /// </summary>
        /// <returns>The singleton instance of the <see cref="Sailing"/> class.</returns>
        /// https://csharpindepth.com/articles/singleton (hentet: 03.03.2024) (Skeet.Jon, 2019)
        public static Sailing GetInstance()
        {
            return instance;
        }

        /// <summary>
        /// Schedules a sailing for a specific ship model.
        /// </summary>
        /// <param name="shipModel">The model of the ship.</param>
        /// <param name="sailingTime">The time at which the sailing is scheduled to occur.</param>
        /// <param name="destinationLocation">The destination location of the sailing.</param>
        /// <param name="recurringType">The type of recurring sailing, if any.</param>
        public void ScheduleSailing(Harbor harbor, Model shipModel, DateTime sailingTime, int destinationLocation, RecurringType recurringType)
        {

            if (sailingTime < harbor.CurrentTime)
            {
                throw new ArgumentException("Sailing time cannot be in the past.");
            }

            if (destinationLocation < 0)
            {
                throw new ArgumentException("Destination location cannot be negative.");
            }

            if (ScheduledSailings.ContainsKey(shipModel))
            {
                foreach (var scheduledSailing in ScheduledSailings[shipModel])
                {
                    if (scheduledSailing.Item1 == sailingTime && scheduledSailing.Item3 == recurringType)
                    {
                        throw new DuplicateSailingException("Sailing with the same time and recurring type already exists.");
                    }
                }
            }

            if (!ScheduledSailings.ContainsKey(shipModel))
            {
                ScheduledSailings[shipModel] = new List<(DateTime, int, RecurringType)>();
            }

            ScheduledSailings[shipModel].Add((sailingTime, destinationLocation, recurringType));
        }

        /// <summary>
        /// Starts the scheduled sailings for ships in the harbor.
        /// </summary>
        /// <param name="harbor">The harbor where the ships are docked.</param>
        /// <param name="historyHandler">The history handler for recording sailing events.</param>
        /// <exception cref="ArgumentNullException">Thrown when either harbor or historyHandler is null.</exception>
        public void StartScheduledSailings(Harbor harbor, HistoryHandler historyHandler)
        {
            if (harbor == null)
            {
                throw new ArgumentNullException(nameof(harbor), "Harbor cannot be null.");
            }

            if (historyHandler == null)
            {
                throw new ArgumentNullException(nameof(historyHandler), "HistoryHandler cannot be null.");
            }

            foreach (var kvp in ScheduledSailings)
            {
                Model shipModel = kvp.Key;
                foreach (var (sailingTime, destinationLocation, recurringType) in kvp.Value)
                {
                    if (recurringType == RecurringType.Weekly && harbor.GetCurrentTime().DayOfWeek != sailingTime.DayOfWeek)
                    {
                        continue;
                    }
                    if (harbor.GetCurrentTime().Hour == sailingTime.Hour && harbor.GetCurrentTime().Minute == sailingTime.Minute)
                    {
                        foreach (Ship ship in harbor.Ships)
                        {
                            if (ship.Model == shipModel && ship.IsReadyToSail && !ship.IsSailing)
                            {
                                if (harbor.RemoveShipFromDock(ship))
                                {
                                    ship.SetDestinationLocationFrom(ship.CurrentLocation, destinationLocation);
                                    ship.CalculateMovement(harbor);
                                    ship.SailedAtTime = harbor.GetCurrentTime().ToString();
                                    historyHandler.AddEventToShipHistory(ship, $"{ship.Name} Sailed at {ship.SailedAtTime} to destination:{destinationLocation}  ship cargo: {ship.Cargo.Count} --");
                                    ship.IsSailing = true;
                                    harbor.SailingShips.Add(ship);
                                    harbor.RaiseShipDeparted(ship);
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
            }
        }

    }
}