using Cdull.V2024.HarborSimulation.SimulationFramework;
/// <summary>
/// Represents the scheduling and execution of ship sailings.
/// </summary>
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
    /// https://csharpindepth.com/articles/singleton (hentet: 03.03.2024)
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
    public void ScheduleSailing(Model shipModel, DateTime sailingTime, int destinationLocation, RecurringType recurringType)
    {
        if (!ScheduledSailings.ContainsKey(shipModel))
        {
            ScheduledSailings[shipModel] = new List<(DateTime, int, RecurringType)>();
        }

        ScheduledSailings[shipModel].Add((sailingTime, destinationLocation, recurringType));
    }

    /// <summary>
    /// Starts scheduled sailings based on the current time and recurring schedule.
    /// </summary>
    /// <param name="harbor">The harbor instance where the sailings occur.</param>
    /// <param name="historyHandler">The history handler for recording sailing events.</param>
    public void StartScheduledSailings(Harbor harbor, HistoryHandler historyHandler)
    {
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
                                ship.Move();
                                ship.SailedAtTime = harbor.GetCurrentTime().ToString();
                                historyHandler.AddEventToShipHistory(ship, $"{ship.Name} Sailed at {harbor.GetCurrentTime()} to destination:{destinationLocation}");
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
