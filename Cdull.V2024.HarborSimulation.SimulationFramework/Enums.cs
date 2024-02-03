﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Enums
    {
        public enum Model
        {
            ContainerShip,
            Bulker,
            Tanker, 
            LNGCarrier, 
            RoRo

        }

        public enum Size
        {
            Small, 
            Medium, 
            Large
        }

    

        public enum RecurringType
        {
            Daily,
            Weekly
        }
    }
}
