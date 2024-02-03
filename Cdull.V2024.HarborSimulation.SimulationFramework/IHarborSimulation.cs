using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cdull.V2024.HarborSimulation.SimulationFramework.Enums;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    internal interface IHarborSimulation
    {

        public void Run(DateTime starttime, DateTime endTime, int numberOfShips, Size shipSize, int numberOfCargoOnShip, Model shipModel, int numberOfDocks,
             Size dockSize, Model dockModel); 


    }
}

