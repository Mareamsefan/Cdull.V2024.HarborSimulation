using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class ShipHandler
    {

        internal Ship ship { get; private set; }
        internal Harbor Harbor { get; private set; }

        public ShipHandler(Ship shipHandlerShip, Harbor shipHandlerHarbor)
        {
            ship = shipHandlerShip;
            Harbor = shipHandlerHarbor;

        }

        /*
        public void AddCargoToStorage()
        {
            if (Harbor == null)
            {
                throw new ArgumentNullException(nameof(Harbor), "Harbor cannot be null.");
            }

            foreach (Cargo cargo in ship.Cargo)
            {
                Harbor.cargoStorage.AddCargo(cargo);
            }

            ship.Cargo.Clear();
        }

        public void AddCargoToShip(int numberOfCargo)
        {
            if (Harbor == null)
            {
                throw new ArgumentNullException(nameof(Harbor), "Harbor cannot be null.");
            }

            for (int i = 0; i < numberOfCargo; i++)
            {
                if (Harbor.cargoStorage.Cargo.Count > 0)
                {
                    Cargo cargo = Harbor.cargoStorage.Cargo.First();
                    ship.Cargo.Add(cargo);
                    Harbor.cargoStorage.RemoveCargo(cargo);
                }
                else
                {
                    break; // No more cargo available in the harbor
                }
            }
        }

        public bool RemoveShipFromDock()
        {

            if (ship.DockedAt != null)
            {
                Dock dock = ship.DockedAt;

                // Fjern skipet fra dokken
                dock.OccupiedBy = null;

                // Sett dokken til ledig igjen
                dock.IsAvailable = true;

                // Fjern skipet fra listen over dockede skip
                Harbor.DockedShips.Remove(ship);

                ship.HasDocked = false;
                ship.DockedAt = null;

                return true;
            }
            else
            {
                return false;
            }

        }

        public void SailingOneShip(DateTime currentTime, DateTime sailingStartTime, int numberOfDays)
        {
            // Sjekk om skipet er klart for seiling og om det er tid for å starte seilingen
            if (ship.IsReadyToSail && currentTime == sailingStartTime)
            {
                // Hvis skipet kan fjernes fra dokken
                if (Harbor.RemoveShipFromDock(ship))
                {
                    ship.SailedAtTime = currentTime.ToString();
                    ship.IsSailing = true;
                    Harbor.SailingShips.Add(ship);
                }
            }
            // Sjekk om det er tid for skipet å stoppe seilingen
            else if (currentTime >= sailingStartTime.AddDays(numberOfDays))
            {
                ship.IsSailing = false;
                Harbor.SailingShips.Remove(ship);
                Harbor.QueueShipsToDock();
            }
            else
            {
                ship.IsWaitingForSailing = true;
            }


        }
        */
    }

        
}