using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    internal interface IHarborSimulation
    {

        public void Run(DateTime starttime, DateTime endTime, int numberOfShip, int numberOfDocks, 
            int numberOfCranes, int numberOfCargos); 

    }
}