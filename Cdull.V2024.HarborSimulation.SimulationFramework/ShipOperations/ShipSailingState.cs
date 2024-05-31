using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.ShipOperations
{
    /// <summary>
    /// Represents the states of a sailing ship. 
    /// </summary>
    internal enum ShipSailingState
    {
        /// <summary>
        /// Represnets that the ship is not sailing. 
        /// </summary>
        NotSailing,
        
        /// <summary>
        /// Represents that the ship is sailing. 
        /// </summary>
        Sailing, 

        /// <summary>
        /// Represents that the ship is waiting to sail. 
        /// </summary>
        Waiting,

        /// <summary>
        /// Represents that the ship has arrived at its destination location and completed the sailing. 
        /// </summary>
        Arrived, 


    }
}
