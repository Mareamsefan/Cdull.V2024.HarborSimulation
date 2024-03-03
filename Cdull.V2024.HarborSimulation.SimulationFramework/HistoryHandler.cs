using System;
using System.Collections.Generic;
using System.Text;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class HistoryHandler
    {
        private static HistoryHandler instance;
        private Dictionary<DateTime, Harbor> harborHistory = new Dictionary<DateTime, Harbor>();
        private List<(string shipName, string eventDescription)> shipHistory = new List<(string ShipName, string eventDescription)>();

        // Privat konstruktør for å forhindre instansiering utenfra.
        private HistoryHandler() { }

        // Metode for å få tilgang til den eneste instansen av HistoryHandler.
        public static HistoryHandler GetInstance()
        {
            if (instance == null)
            {
                instance = new HistoryHandler();
            }
            return instance;
        }

        // Metode for å lagre havnehistorikk.
        public void SaveHarborHistory(DateTime date, Harbor harbor)
        {
            date = date.Date;
            harborHistory.Add(date, harbor);
        }

        // Metode for å hente havnehistorikk.
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

       
        public string GetShipsHistory()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Ship History:");

            foreach ( var history in shipHistory)
            {
                sb.AppendLine(history.eventDescription);
            }

            return sb.ToString();
        }

        public string GetShipHistory(Ship ship)
        {
            StringBuilder sb = new StringBuilder();

            // Finn alle hendelsene knyttet til det angitte skipet
            var shipEvents = shipHistory.Where(history => history.shipName == ship.Name);

            // Legg til tittel på historien
            sb.AppendLine($"Ship History for {ship.Name}:");

            // Legg til hver hendelse i skipets historie
            foreach (var shipEvent in shipEvents)
            {
                sb.AppendLine(shipEvent.eventDescription);
            }

            // Returner den formaterte strengen
            return sb.ToString();
        }



        public void AddEventToShipHistory(Ship ship, string eventDescription)
        {
            var newEvent = (ship: ship.Name, eventDescription: eventDescription);
            shipHistory.Add(newEvent); 
        }
    }
}
