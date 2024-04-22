using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.ShipOperations
{   /// <summary>
    /// Represents the scheduling and execution of ship sailings.
    /// </summary>
    public static class ScheduleSailing
    {
        /// <summary>
        /// Schedules sailings for a specific ship model.
        /// </summary>
        /// <param name="harbor">The harbor where the ships are docked.</param>
        /// <param name="shipModel">The model of the ship.</param>
        /// <param name="sailingTime">The time at which the sailing is scheduled to occur.</param>
        /// <param name="destinationLocation">The destination location of the sailing.</param>
        /// <param name="recurringType">The type of recurring sailing, if any.</param>

        public static void ScheduleSailings(Harbor harbor, Model shipModel, DateTime sailingTime, int destinationLocation, RecurringType recurringType)
        {
            if (sailingTime < harbor.CurrentTime)
            {
                throw new ArgumentException("Sailing time cannot be in the past.");
            }

            if (destinationLocation < 0)
            {
                throw new ArgumentException("Destination location cannot be negative.");
            }

            foreach (Ship ship in harbor.Ships)
            {
                if (ship.Model.Equals(shipModel))
                {
                    if (ship.ScheduledSailings.Any(s => s.DateTime == sailingTime && s.RecurringType == recurringType))
                    {
                        throw new DuplicateSailingException("Sailing with the same time and recurring type already exists.");
                    }

                    // Legg til den nye seilasen i skipets liste over planlagte seilaser
                    Sailing sailing = new Sailing(sailingTime, destinationLocation, recurringType);
                    ship.ScheduledSailings.Add(sailing);
                    Console.WriteLine("THIS SHIP GETS A SAILING: " + ship.Name);
                }
            }
        }



        /// <summary>
        /// Schedules sailings for one specific ship.
        /// </summary>
        /// <param name="harbor">The harbor where the ships are docked.</param>
        /// <param name="shipModel">The model of the ship.</param>
        /// <param name="sailingTime">The time at which the sailing is scheduled to occur.</param>
        /// <param name="destinationLocation">The destination location of the sailing.</param>
        /// <param name="recurringType">The type of recurring sailing, if any.</param>
        public static void ScheduleOneSailing(Harbor harbor, Ship ship, DateTime sailingTime, int destinationLocation, RecurringType recurringType)
        {
            if (sailingTime < harbor.CurrentTime)
            {
                throw new ArgumentException("Sailing time cannot be in the past.");
            }

            if (destinationLocation < 0)
            {
                throw new ArgumentException("Destination location cannot be negative.");
            }

            if (ship.ScheduledSailings.Any(s => s.DateTime == sailingTime && s.RecurringType == recurringType))
            {
                throw new DuplicateSailingException("Sailing with the same time and recurring type already exists.");
            }

            Sailing sailing = new Sailing(sailingTime, destinationLocation, recurringType);
            ship.ScheduledSailings.Add(sailing);
        }

        /// <summary>
        /// Checks all scheduled sailings for a specific ship model.
        /// </summary>
        /// <param name="harbor">The harbor where the ships are docked.</param>
        /// <param name="shipModel">The model of the ship to check sailings for.</param>
        /// <returns>A list of scheduled sailings for the specified ship model.</returns>
        public static List<Sailing> CheckScheduledSailings(Harbor harbor, Model shipModel)
        {
            List<Sailing> allScheduledSailings = new List<Sailing>();

            foreach (Ship ship in harbor.Ships)
            {
                if (ship.Model == shipModel)
                {
                    allScheduledSailings.AddRange(ship.ScheduledSailings);
                }
            }

            return allScheduledSailings;
        }



        /// <summary>
        /// Checks all scheduled sailings for a specific ship.
        /// </summary>
        /// <param name="ship">The ship to check sailings for.</param>
        /// <returns>A list of scheduled sailings for the specified ship.</returns>
        public static List<Sailing> CheckScheduledSailingsForShip(Ship ship)
        {
            return ship.ScheduledSailings.ToList();
        }


    } 
}
    

