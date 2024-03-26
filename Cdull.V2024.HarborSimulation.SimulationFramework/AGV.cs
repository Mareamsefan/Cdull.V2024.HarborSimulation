using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class AGV
    {
        internal int Id { get; set; }
        internal int Counter { get; set; }
        internal bool IsAvailable { get; set; }
        internal Container Container { get; set; }
        internal int Location { get; set; }


        public AGV(int agvLocation)
        {
            Counter++;
            Id = Counter;
            Location = agvLocation;
            IsAvailable = true;
        }

        public void LoadContainerToAGV(Container container)
        {
          
            Container = container;
              
        }
    }
}
