﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    internal class Enums
    {
        public enum ShipType
        {
            ContainerShip,
            Bulker,
            Tanker, 
            LNGCarrier, 
            RoRo

        }

        public enum ShipSize
        {
            Small, 
            Medium, 
            Large
        }

        public enum DockSize
        {
            Small,
            Medium,
            Large
        }
        public enum DOckType
        {
            ContainerDock,
            BulkerDock,
            TankerDock,
            LNGCarrierDock,
            RoRoDock
        }
    }
}
