using System;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class PortalCrane
    {
        /// <summary>
        /// Represents a Poralcrane used in harbor to move containers from AVG to container storage.
        /// </summary>
        public string Id {  get; set; } 
        public double MaxCapacity { get; set; }
        private bool IsPortalCraneAvalible { get; set; }
        private bool IsPortalCraneOutOffFuntion { get; set; }

        private int PortalCraneSpeed { get; set; }  

        /// <summary>
        /// Initializes a new instance of the PortalCrane class with specified name.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="MaxCapacity"></param>
        public PortalCrane(string portalCraneId, double MaxCapacity)
        {
            Id = portalCraneId;
            MaxCapacity = MaxCapacity;
            IsPortalCraneAvalible = true;
            IsPortalCraneOutOffFuntion = false;
            PortalCraneSpeed = 20; 
        }
    }

}
