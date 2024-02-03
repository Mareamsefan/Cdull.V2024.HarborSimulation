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
    internal class Simulation : IHarborSimulation
    {
      
        public void Run(DateTime starttime, DateTime endTime, int numberOfShips, Enums.Size shipSize, int numberOfCargoOnShip, Enums.Model shipModel, int numberOfDocks, Enums.Size dockSize, Enums.Model dockModel)
        {
            var currentTime = starttime;

            while (currentTime < endTime)
            {
                Harbor harbor = new Harbor("Harbor");
                CargoStorage cargoStorage = new CargoStorage("CargoStorage");
                harbor.InitializeDocks(numberOfDocks, dockModel, dockSize);
                harbor.InitializeShips(numberOfShips, shipSize, shipModel, numberOfCargoOnShip); 
                harbor.QueueShipsToDock();
                harbor.DockShips();
                harbor.AddCargoToStorage(); 
              
            }
            throw new NotImplementedException();
        }
    }


}
