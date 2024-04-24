using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.ShipOperations
{
    /// <summary>
    /// Provides data for the ShipArrival event.
    /// </summary>
    public class ShipArrivalEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the ship that has arrived.
        /// </summary>
        public Ship ArrivedShip { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipArrivalEventArgs"/> class with the specified arrived ship.
        /// </summary>
        /// <param name="arrivedShip">The ship that has arrived.</param>
        internal ShipArrivalEventArgs(Ship arrivedShip)
        {
            ArrivedShip = arrivedShip;
        }


    }
}
