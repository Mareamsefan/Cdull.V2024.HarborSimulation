using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.ShipOperations
{   /// <summary>
    /// Represents the scheduling of ship sailings.
    /// </summary>
    public static class ScheduleSailing
    {
        /// <summary>
        /// Schedules sailings for a specific ship model.
        /// </summary>
        /// <param name="harbor">The harbor where the ships are docked.</param>
        /// <param name="ships">The list of the total ships on harbor.</param>
        /// <param name="shipModel">The model of the ship.</param>
        /// <param name="sailingTime">The time at which the sailing is scheduled to occur.</param>
        /// <param name="destinationLocation">The destination of the sailing.</param>
        /// <param name="recurringType">The type of recurring sailing, if any.</param>
        /// <example>
        /// This example shows how to use the ScheduleSailings method.
        /// <code>
        /// ScheduleSailing.ScheduleSailings(harbor, ships, Model.LNGCarrier, new DateTime(2024, 1, 2), 40, RecurringType.Weekly);
        /// </code>
        /// </example>

        public static void ScheduleSailings(Harbor harbor, List<Ship> ships,  Model shipModel, DateTime sailingTime, int destinationLocation, RecurringType recurringType)
        {
            if (sailingTime < harbor.CurrentTime)
            {
                throw new ArgumentException("Sailing time cannot be in the past.");
            }

            if (destinationLocation < 0)
            {
                throw new ArgumentException("Destination location cannot be negative.");
            }

            foreach (Ship ship in ships)
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
                }
            }
        }



        /// <summary>
        /// Schedules a single sailing for a specific ship.
        /// </summary>
        /// <param name="harbor">The harbor where the ship is docked.</param>
        /// <param name="ship">The ship for which to schedule the sailing.</param>
        /// <param name="sailingTime">The time at which the sailing is scheduled to depart.</param>
        /// <param name="destinationLocation">The destination for the sailing.</param>
        /// <param name="recurringType">The recurring type of the scheduled sailing.</param>
        /// <exception cref="ArgumentException">Thrown when the sailing time is in the past or the destination location is negative.</exception>
        /// <exception cref="DuplicateSailingException">Thrown when a sailing with the same time and recurring type already exists for the ship.</exception>
        /// <example>
        /// This example shows how to use the ScheduleOneSailing method.
        /// <code>
        /// ScheduleSailing.ScheduleOneSailing(harbor, ship, DateTime.Now.AddHours(24), 102, RecurringType.None);
        /// </code>
        /// </example>

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
        /// <param name="ships">The list of ships to check for scheduled sailings.</param>
        /// <param name="shipModel">The model of the ship to check sailings for.</param>
        /// <returns>A list of scheduled sailings for the specified ship model.</returns>
        /// <example>
        /// This example shows how to use the CheckScheduledSailings method.
        /// <code>
        /// List<Sailing> sailings = ScheduleSailing.CheckScheduledSailings(harbor, ships, Model.LNGCarrier);
        /// sailings.ForEach(sailing =>
        ///       Console.WriteLine(sailing.ToString())
        ///  );
        /// </code>
        /// </example>

        public static List<Sailing> CheckScheduledSailings(Harbor harbor, List<Ship> ships, Model shipModel)

        {
            List<Sailing> allScheduledSailings = new List<Sailing>();

            foreach (Ship ship in ships)
            {
                if (ship.Model == shipModel)
                {
                    allScheduledSailings.AddRange(ship.ScheduledSailings);
                }
            }

            return allScheduledSailings;
        }



        /// <summary>
        /// Retrieves all scheduled sailings for a specific ship.
        /// </summary>
        /// <param name="ship">The ship for which to retrieve scheduled sailings.</param>
        /// <returns>A list of scheduled sailings for the specified ship.</returns>
        /// <example>
        /// This example shows how to use the CheckScheduledSailingsForShip method.
        /// <code>
        /// List<Sailing> sailings = ScheduleSailing.CheckScheduledSailingsForShip(ship);
        /// </code>
        /// </example>
        public static List<Sailing> CheckScheduledSailingsForShip(Ship ship)
        {
            return ship.ScheduledSailings.ToList();
        }


    } 
}
    

