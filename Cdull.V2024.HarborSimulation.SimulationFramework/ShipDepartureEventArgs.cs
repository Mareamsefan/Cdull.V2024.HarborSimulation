using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.ShipEvents
{
    public class ShipDepartureEventArgs
    {
        
        public ShipDepartureEventArgs(Ship departedShip)
        {
            DepartedShip = departedShip; 
        }

        public Ship DepartedShip { get; private set; }

    }

    public class ShipArrivalEventArgs
    {

        public ShipArrivalEventArgs(Ship arrivedShip)
        {
            ArrivedShip = arrivedShip;
        }
        public Ship ArrivedShip { get; private set; }

    }


}


