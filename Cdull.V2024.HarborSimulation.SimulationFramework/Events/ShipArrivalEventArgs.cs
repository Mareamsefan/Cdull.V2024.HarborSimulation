﻿using System;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.Events
{
    /// <summary>
    /// Provides data for the ShipArrival event.
    /// </summary>
    public class ShipArrivalEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShipArrivalEventArgs"/> class with the specified arrived ship.
        /// </summary>
        /// <param name="arrivedShip">The ship that has arrived.</param>
        public ShipArrivalEventArgs(Ship arrivedShip)
        {
            ArrivedShip = arrivedShip;
        }

        /// <summary>
        /// Gets the ship that has arrived.
        /// </summary>
        public Ship ArrivedShip { get; private set; }
    }
}
