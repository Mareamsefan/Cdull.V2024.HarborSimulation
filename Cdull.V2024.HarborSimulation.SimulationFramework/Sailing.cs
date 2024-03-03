using Cdull.V2024.HarborSimulation.SimulationFramework;

public class Sailing
{
    private static Sailing instance;
    private Dictionary<Model, List<(DateTime, int, RecurringType)>> ScheduledSailings { get; }

    private Sailing()
    {
        ScheduledSailings = new Dictionary<Model, List<(DateTime, int, RecurringType)>>();
    }

    public static Sailing GetInstance()
    {
        if (instance == null)
        {
            instance = new Sailing();
        }
        return instance;
    }

    public void ScheduleSailing(Model shipModel, DateTime sailingTime, int destinationLocation, RecurringType recurringType)
    {
        if (!ScheduledSailings.ContainsKey(shipModel))
        {
            ScheduledSailings[shipModel] = new List<(DateTime, int, RecurringType)>();
        }

        ScheduledSailings[shipModel].Add((sailingTime, destinationLocation, recurringType));
    }

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
