using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.ContainerOperations
{
    /// <summary>
    /// Provides data for the ShipLoading event.
    /// </summary>
    public class ShipLoadingEventArgs : EventArgs
    {

        /// <summary>
        /// Gets the ship that has completed Loading.
        /// </summary>
        public Ship CompletedLoadingShip { get; private set; }
        public Harbor Harbor { get; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ShipLoadingEventArgs"/> class with the ship that has completed Loading.
        /// </summary>
        /// <param name="completedLoadingShip">The ship that has completed Loading.</param>
        public ShipLoadingEventArgs(Ship completedLoadingShip)
        {
            CompletedLoadingShip = completedLoadingShip;
        }



    }
}
