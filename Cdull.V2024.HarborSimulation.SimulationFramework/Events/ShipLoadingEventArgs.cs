using System;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.Events
{
    /// <summary>
    /// Provides data for the ShipLoading event.
    /// </summary>
    public class ShipLoadingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShipLoadingEventArgs"/> class with the ship that has completed loading.
        /// </summary>
        /// <param name="completedLoadingShip">The ship that has completed loading.</param>
        public ShipLoadingEventArgs(Ship completedLoadingShip)
        {
            CompletedLoadingShip = completedLoadingShip;
        }

        /// <summary>
        /// Gets the ship that has completed loading.
        /// </summary>
        public Ship CompletedLoadingShip { get; private set; }
    }
}
