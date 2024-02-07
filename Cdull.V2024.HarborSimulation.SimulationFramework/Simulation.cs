using Cdull.V2024.HarborSimulation.SimulationFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;



namespace Cdull.V2024.HarborSimulation.TestClient
{

    public class Simulation : IHarborSimulation
    {
      

        void IHarborSimulation.Run(Harbor harbor, DateTime starttime, DateTime endTime, int numberOfShips, Enums.Size shipSize, int numberOfCargoOnShip, Enums.Model shipModel, int numberOfDocks, int numberOfCranes, Enums.Size dockSize, Enums.Model dockModel)
        {
            var currentTime = starttime;
            CargoStorage cargoStorage = new CargoStorage("CargoStorage");
            harbor.InitializeDocks(numberOfDocks, dockModel, dockSize, numberOfCranes);
            harbor.InitializeShips(harbor, numberOfShips, shipSize, shipModel, numberOfCargoOnShip);
            Console.WriteLine($"Currentime: {currentTime}");
            Console.WriteLine($"HARBOR SIMULATION STARTED: {harbor.name}");


            while (currentTime < endTime)
            {
                harbor.QueueShipsToDock();
                harbor.DockShips(currentTime);
                //harbor.AddCargoToStorage();
                //harbor.AddCargoToShips(10, currentTime);

                // Geting the ship I want to sail: 
                foreach (Ship ship in harbor.Ships)
                {
                    ship.Sailing(ship, currentTime, new DateTime(2024, 1, 2), 1);
                }

                if (currentTime.Hour == 0 && currentTime.Minute == 0)
                {
                    Console.WriteLine($"\n");
                    Console.WriteLine($"Currentime: {currentTime}\n");

                    Console.WriteLine($"ships waiting to dock to harbor: " + harbor.Ships.Count());

                    Console.WriteLine($"Docks in harbor: " + harbor.Docks.Count());


                    Console.WriteLine($"ships waiting to dock to harbor: " + harbor.WaitingShips.Count());


                    Console.WriteLine($"ships docked in harbor: " + harbor.DockedShips.Count());

                    foreach (Ship ship in harbor.Ships)
                    {  
                        if (currentTime.Date.Equals(ship.DockedAtTime.Date))
                        {

                            Console.WriteLine($"\n{ship.Name}: Docked at {ship.DockedAtTime} \n");
                        }

                    }
                 

                }

                currentTime = currentTime.AddMinutes(1);

                if (currentTime >= endTime)
                {
                    break;
                }
            }


            
        }
    }


}
    