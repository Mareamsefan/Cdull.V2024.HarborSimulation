using System;
using System.Collections.Generic;
using System.Text;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    /// <summary>
    /// Handles the history recording of harbor and ship events.
    /// </summary>
    public class HistoryHandler
    {
        private static readonly HistoryHandler instance = new HistoryHandler();
        private Dictionary<DateTime, Harbor> harborHistory = new Dictionary<DateTime, Harbor>();
        private List<(string shipName, string eventDescription)> shipHistory = new List<(string ShipName, string eventDescription)>();

        /// <summary>
        /// Private constructor to prevent external instantiation.
        /// </summary>
        private HistoryHandler() { }

        /// <summary>
        /// Gets the singleton instance of the <see cref="HistoryHandler"/> class.
        /// </summary>
        /// <returns>The singleton instance of the <see cref="HistoryHandler"/> class.</returns>
        /// https://csharpindepth.com/articles/singleton (hentet: 03.03.2024) (Skeet.Jon, 2019)
        public static HistoryHandler GetInstance()
        {
            return instance;
        }

        /// <summary>
        /// Saves the harbor history for a specific date.
        /// </summary>
        /// <param name="date">The date for which harbor history is saved.</param>
        /// <param name="harbor">The harbor instance to be saved in the history.</param>
        internal void SaveHarborHistory(DateTime date, Harbor harbor)
        {
            date = date.Date;
            harborHistory.Add(date, harbor);
        }

        /// <summary>
        /// Retrieves the harbor history for a given date.
        /// </summary>
        /// <param name="fromDate">The date for which harbor history is retrieved.</param>
        /// <returns>The harbor instance for the specified date.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if harbor history is not found for the given date.</exception>
        public Harbor GetHarborHistory(DateTime fromDate)
        {
            if (harborHistory.ContainsKey(fromDate.Date))
            {
                return harborHistory[fromDate.Date];
            }
            else
            {
                throw new KeyNotFoundException("Harbor history not found for the given date.");
            }
        }

        /// <summary>
        /// Retrieves the history of all ships.
        /// </summary>
        /// <returns>A string containing the history of all ships.</returns>
        public string GetShipsHistory()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Ship History:");

            foreach (var history in shipHistory)
            {
                sb.AppendLine(history.eventDescription);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Retrieves the history of a specific ship.
        /// </summary>
        /// <param name="ship">The ship for which history is retrieved.</param>
        /// <returns>A string containing the history of the specified ship.</returns>
        public string GetShipHistory(Ship ship)
        {
            StringBuilder sb = new StringBuilder();

            var shipEvents = shipHistory.Where(history => history.shipName == ship.Name);

            sb.AppendLine($"Ship History for {ship.Name}:");

            foreach (var shipEvent in shipEvents)
            {
                sb.AppendLine(shipEvent.eventDescription);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Adds a new event to the history of a specific ship.
        /// </summary>
        /// <param name="ship">The ship for which the event is recorded.</param>
        /// <param name="eventDescription">The description of the event.</param>
        public void AddEventToShipHistory(Ship ship, string eventDescription)
        {
            var newEvent = (ship: ship.Name, eventDescription: eventDescription);
            shipHistory.Add(newEvent);
        }
    }
}
