using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Sailing
    {
        private static Sailing instance;
        private Dictionary<Model, List<(DateTime, RecurringType)>> ScheduledSailings { get; }

        private Sailing()
        {
            ScheduledSailings = new Dictionary<Model, List<(DateTime, RecurringType)>>();

        }


        public static Sailing GetInstance()
        {
            if (instance == null)
            {
                instance = new Sailing();
            }
            return instance;
        }

        public void ScheduleSailing(Model shipModel, DateTime sailingTime, RecurringType recurringType)
        {
            if (!ScheduledSailings.ContainsKey(shipModel))
            {
                ScheduledSailings[shipModel] = new List<(DateTime, RecurringType)>();
            }

            ScheduledSailings[shipModel].Add((sailingTime, recurringType));
        }

        public void StartScheduledSailings(Harbor harbor, int destinationLocation, HistoryHandler historyHandler)
        {
            foreach (var kvp in ScheduledSailings)
            {
                Model shipModel = kvp.Key;
                foreach (var (sailingTime, recurringType) in kvp.Value)
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
                                    historyHandler.AddEventToShipHistory(ship, $"{ship.Name} Sailed at {harbor.GetCurrentTime()} to destination:{ship.DestinationLocation}");
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
