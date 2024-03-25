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
        internal double speed { get; set; }
        internal bool IsAvailable { get; set; }
        internal Container Container { get; set; }
        internal int Location { get; set; }


        public AGV(int agvLocation ,double agvSpeed)
        {
            Counter++;
            Id = Counter;
            speed = agvSpeed;
            Location = agvLocation;
            IsAvailable = true;
        }

        public void LoadContainerToAGV(Container container)
        {
            if (Container != null)
            {
                throw new InvalidOperationException("AGV already loaded with cargo.");
            }
            Container = container;
        }



    }
}
