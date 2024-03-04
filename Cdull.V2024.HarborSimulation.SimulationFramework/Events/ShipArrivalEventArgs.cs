using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.Events
{
    public class ShipArrivalEventArgs
    {

        public ShipArrivalEventArgs(Ship arrivedShip)
        {
            ArrivedShip = arrivedShip;
        }
        public Ship ArrivedShip { get; private set; }

    }

}
