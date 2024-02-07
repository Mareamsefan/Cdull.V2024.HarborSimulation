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
      
        public void Run(DateTime starttime, DateTime endTime, int numberOfShips, Enums.Size shipSize, int numberOfCargoOnShip, Enums.Model shipModel, int numberOfDocks, Enums.Size dockSize, Enums.Model dockModel)
        {
            var currentTime = starttime;
            Harbor harbor = new Harbor("Harbor");
            CargoStorage cargoStorage = new CargoStorage("CargoStorage");
            harbor.WaitingShips.Clear();
            harbor.DockedShips.Clear();
            harbor.Docks.Clear();
            harbor.Ships.Clear();
            harbor.SailingShips.Clear();
            harbor.InitializeDocks(numberOfDocks, dockModel, dockSize);
            harbor.InitializeShips(numberOfShips, shipSize, shipModel, numberOfCargoOnShip);

            while (currentTime < endTime)
            {
                harbor.QueueShipsToDock();
                
                harbor.DockShips(currentTime);
                harbor.AddCargoToStorage();
                harbor.AddCargoToShips(10);
              
                // Geting the ship I want to sail: 
                foreach (Ship ship in harbor.Ships)
                {
                    ship.Sailing(ship, currentTime, new DateTime(2024, 1, 2), 1); 
                }
              
                if (currentTime.Hour == 0 && currentTime.Minute == 0)
                {
                    Console.WriteLine($"\n");
                    Console.WriteLine($"Currentime: {currentTime}");
                    Console.WriteLine($"Docks in harbor: " + harbor.Docks.Count());
                 
                    
                    Console.WriteLine($"ships waiting to dock to harbor: " + harbor.WaitingShips.Count());
                    

                    Console.WriteLine($"ships docked in harbor: " + harbor.DockedShips.Count());
                    
                    foreach (Ship ship in harbor.Ships)
                    {
                        foreach (String history in ship.History)
                        {
                            Console.WriteLine(history);
                        }
                    }
                    
                }
                
                currentTime = currentTime.AddMinutes(1);

                if (currentTime >= endTime)
                {
                    break;
                }
            }

            throw new NotImplementedException();
        }
    }


}
    