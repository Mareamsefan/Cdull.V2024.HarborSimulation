using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.ShipOperations

{
    /// <summary>
    /// Provides data for the ShipDeparture event.
    /// </summary>
    public class ShipDepartureEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the ship that has departed.
        /// </summary>
        public Ship DepartedShip { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipDepartureEventArgs"/> class with the specified departed ship.
        /// </summary>
        /// <param name="departedShip">The ship that has departed.</param>
        public ShipDepartureEventArgs(Ship departedShip)
        {
            DepartedShip = departedShip;
        }


    }
}
