using System;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class PortalCrane
    {
        /// <summary>
        /// Represents a Poralcrane used in harbor to move containers from AVG to container storage.
        /// </summary>
  
        internal bool IsAvailable { get; set; }

        internal int handlingTime { get; set; }
        internal Container Container { get; set; }


        public PortalCrane()
        {
            // 1 min 
            handlingTime = 0; 
            IsAvailable = true;
        }

        public void LiftContainer(Container container)
        {
            if (Container != null)
            {
                throw new InvalidOperationException("Portalcrane already loaded with cargo.");
            }
            Container = container;
        }

    }

}
