using Cdull.V2024.HarborSimulation.SimulationFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.TestClient
{
    internal class Simulation:IHarborSimulation
    {

        public void Run(DateTime starttime, DateTime endTime, )
        {
            var currentTime = starttime; 

            while(currentTime < endTime)
            {
                Harbor harbor = new Harbor("Harbor");
                CargoStorage cargoStorage = new CargoStorage("CargoStorage");
                harbor.InitializeCranes(); 
            }

            throw new NotImplementedException();
        }
    }
}
