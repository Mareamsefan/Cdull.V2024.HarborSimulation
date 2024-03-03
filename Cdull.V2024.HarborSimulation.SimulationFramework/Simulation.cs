using System;
using System.Collections.Generic;
using System.Text;
using Cdull.V2024.HarborSimulation.SimulationFramework;

namespace Cdull.V2024.HarborSimulation.TestClient
{
    /// <summary>
    /// Represents the simulation of a harbor.
    /// </summary>
    public class Simulation : IHarborSimulation
    {
   
        public void Run(Harbor harbor, DateTime startTime, DateTime endTime, List<Ship> ships, List<Dock> docks)
        {
            HistoryHandler historyHandler = HistoryHandler.GetInstance();
            harbor.SetCurrentTime(startTime);

       
            harbor.WaitingShips.Clear();
            harbor.Ships.Clear();
            harbor.Docks.Clear();
            harbor.DockedShips.Clear();
            harbor.SailingShips.Clear();

      
            harbor.Docks.AddRange(docks);
            harbor.Ships.AddRange(ships);

            while (harbor.GetCurrentTime() < endTime)
            {
        
                if (harbor.GetCurrentTime().Hour == 0 && harbor.GetCurrentTime().Minute == 0)
                {
                    historyHandler.SaveHarborHistory(harbor.GetCurrentTime(), harbor); 
                }

                harbor.QueueShipsToDock();

    
                harbor.DockShips();


         
                harbor.AddCargoToStorage();

              
                harbor.AddCargoToShips(10);

                Sailing sailing = Sailing.GetInstance();

                sailing.StartScheduledSailings(harbor, historyHandler);


                harbor.SetCurrentTime(harbor.GetCurrentTime().AddMinutes(1));
            }
        }
    }
}
