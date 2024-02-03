using Cdull.V2024.HarborSimulation.SimulationFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

            while (currentTime < endTime)
            {
                Thread.Sleep(10);
                Harbor harbor = new Harbor("Harbor");
                CargoStorage cargoStorage = new CargoStorage("CargoStorage");
                harbor.InitializeDocks(numberOfDocks, dockModel, dockSize);
                harbor.InitializeShips(numberOfShips, shipSize, shipModel, numberOfCargoOnShip);
                harbor.QueueShipsToDock();
                harbor.DockShips();
               
                if (currentTime.Hour == 0 && currentTime.Minute == 0)
                {
                    Console.WriteLine($"Currentime: {currentTime}");
                    Console.WriteLine($"Docks in harbor: ");
                    foreach (Dock dock in harbor.Docks)
                    {
                        Console.WriteLine(dock.ToString());
                    }
                    Console.WriteLine($"ships waiting to dock to harbor: ");
                    foreach (Ship ship in harbor.WaitingShips)
                    {
                        Console.WriteLine(ship.ToString());
                    }
                    Console.WriteLine($"ships docked in harbor: ");
                    foreach (Ship ship in harbor.DockedShips)
                    {
                        Console.WriteLine(ship.ToString());
                    }
                }
     

                //harbor.AddCargoToStorage(); 
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
