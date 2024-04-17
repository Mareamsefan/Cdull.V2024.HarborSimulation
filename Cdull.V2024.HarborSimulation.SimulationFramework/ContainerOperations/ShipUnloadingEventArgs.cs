using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.ContainerOperations
{
    /// <summary>
    /// Provides data for the ShipUnloading event.
    /// </summary>
    public class ShipUnloadingEventArgs : EventArgs
    {

        /// <summary>
        /// Gets the ship that has completed unloading.
        /// </summary>
        public Ship CompletedUnloadingShip { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipUnloadingEventArgs"/> class with the ship that has completed unloading.
        /// </summary>
        /// <param name="completedUnloadingShip">The ship that has completed unloading.</param>
        public ShipUnloadingEventArgs(Ship completedUnloadingShip)
        {
            CompletedUnloadingShip = completedUnloadingShip;

        }


    }
}
