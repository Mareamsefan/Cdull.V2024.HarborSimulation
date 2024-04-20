using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure
{
    /// <summary>
    /// Represents the model types of ships and docks, along with their corresponding speeds in kilometers per hour.
    /// </summary>
    public enum Model
    {
        /// <summary>
        /// Represents a container ship model with a speed of 30 kilometers per hour.
        /// </summary>
        ContainerShip = 30,
        /// <summary>
        /// Represents a bulker ship model with a speed of 40 kilometers per hour.
        /// </summary>
        Bulker = 40,
        /// <summary>
        /// Represents a tanker ship model with a speed of 50 kilometers per hour.
        /// </summary>
        Tanker = 50,
        /// <summary>
        /// Represents an LNG carrier ship model with a speed of 45 kilometers per hour.
        /// </summary>
        LNGCarrier = 45,
        /// <summary>
        /// Represents a RoRo (Roll-on/Roll-off) ship model with a speed of 35 kilometers per hour.
        /// </summary>
        RoRo = 35
}

}
