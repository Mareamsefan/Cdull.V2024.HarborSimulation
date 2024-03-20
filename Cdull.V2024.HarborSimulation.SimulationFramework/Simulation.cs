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
        /// <summary>
        /// Runs the harbor simulation from the specified start time to the end time.
        /// </summary>
        /// <param name="harbor">The harbor instance to simulate.</param>
        /// <param name="startTime">The start time of the simulation.</param>
        /// <param name="endTime">The end time of the simulation.</param>
        /// <param name="ships">The list of ships in the harbor.</param>
        /// <param name="docks">The list of docks in the harbor.</param>
        /// <exception cref="ArgumentNullException">Thrown when the harbor, list of ships, or list of docks is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the start time is after the end time or if either the list of ships or list of docks is empty.</exception>
        public void Run(Harbor harbor, DateTime startTime, DateTime endTime, List<Ship> ships, List<Dock> docks)
        {
            // Ensure harbor, ships, and docks are not null
            if (harbor == null)
            {
                throw new ArgumentNullException(nameof(harbor), "Harbor instance cannot be null.");
            }

            if (ships == null)
            {
                throw new ArgumentNullException(nameof(ships), "List of ships cannot be null.");
            }

            if (docks == null)
            {
                throw new ArgumentNullException(nameof(docks), "List of docks cannot be null.");
            }

         
            if (startTime > endTime)
            {
                throw new ArgumentException("Start time cannot be after end time.", nameof(startTime));
            }

            HistoryHandler historyHandler = HistoryHandler.GetInstance();
            ContainerHandler containerHandler = new ContainerHandler(harbor);
            Driver driver = new Driver(); 
            Docking docking = new Docking();
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
        
                if (harbor.GetCurrentTime().Hour == 0 && harbor.GetCurrentTime().Minute == 0 && harbor.GetCurrentTime().Second == 0)
                {
                    historyHandler.SaveHarborHistory(harbor.GetCurrentTime(), harbor); 
                }

                harbor.QueueShipsToDock();

                if (harbor.WaitingShips.Any())
                {
                    Ship ship = harbor.WaitingShips.First();
                    if (!driver.Move(ship.CurrentLocation,ship.Speed))
                    {
                        docking.DockShip(harbor, ship);
                    }

                }
                if (harbor.DockedShips.Any())
                {
                    foreach(Ship ship in harbor.DockedShips.ToList())
                    {
                        if (containerHandler.AddContainerToStorage(ship))
                        { 
                            containerHandler.AddContainerToShip(ship, 10);
                        }

                    }
                 

                    Sailing sailing = Sailing.GetInstance();

                    sailing.StartScheduledSailings(harbor, historyHandler);
                }





                harbor.SetCurrentTime(harbor.GetCurrentTime().AddSeconds(1));
            }
        }
    }
}
