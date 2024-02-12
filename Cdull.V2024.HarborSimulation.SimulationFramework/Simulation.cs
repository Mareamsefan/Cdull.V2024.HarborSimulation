using System;
using System.Collections.Generic;
using System.Text;
using Cdull.V2024.HarborSimulation.SimulationFramework;

namespace Cdull.V2024.HarborSimulation.TestClient
{
    public class Simulation : IHarborSimulation
    {
   
        public void Run(Harbor harbor, DateTime startTime, DateTime endTime, List<Ship> ships, List<Dock> docks, DateTime startSailingTime, int numberOfDaysSailing)
        {
            
            // Sett starttidspunktet for simuleringen
            harbor.SetCurrentTime(startTime);

            // Tilbakestill alle lister i havnen
            harbor.WaitingShips.Clear();
            harbor.Ships.Clear();
            harbor.Docks.Clear();
            harbor.DockedShips.Clear();
            harbor.SailingShips.Clear();

            // Legg til skip og dokker i havnen
            harbor.Docks.AddRange(docks);
            harbor.Ships.AddRange(ships);


            foreach (Ship ship in harbor.Ships)
            {
                ship.SetDestinationFromHarbor(harbor.Location, ship.DestinationLocation);
            }

        


            // Simuler havneaktiviteter fram til slutttidspunktet
            while (harbor.GetCurrentTime() < endTime)
            {
                // Lagre havnens tilstand ved midnatt
                if (harbor.GetCurrentTime().Hour == 0 && harbor.GetCurrentTime().Minute == 0)
                {
                    harbor.SaveHarborHistory(harbor.GetCurrentTime());
                }

                harbor.QueueShipsToDock();

                // Dokk skipene som venter
                harbor.DockShips();


                // Flytt last fra skip til lagring
                harbor.AddCargoToStorage();

                // Last skipene som er dokket
                harbor.AddCargoToShips(10);

           
                //harbor.Sailing(startSailingTime, numberOfDaysSailing);

                harbor.RecurringSailing(startSailingTime, numberOfDaysSailing, Enums.RecurringType.Weekly);


                // Oppdater tid
                harbor.SetCurrentTime(harbor.GetCurrentTime().AddMinutes(1));
            }
        }
    }
}
