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
        private bool IsPortalCraneOutOfFuntion { get; set; }

        /// <summary>
        /// Initializes a new instance of the PortalCrane class with specified name.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="MaxCapacity"></param>
        public PortalCrane(string Id, double MaxCapacity)
        {
            Id = PortalCraneId;
            MaxCapacity = MaxCapacity;
            IsCraneAvalible = true;
            IsCraneOutOfFuntion = false;
            PortalCraneSpeed = 200; // i sek?
        }
    }

}
