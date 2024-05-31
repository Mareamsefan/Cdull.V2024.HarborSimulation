using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure
{
    /// <summary>
    /// Represents the size categories of cargo containers.
    /// </summary>
    public enum ContainerSize
    {
        /// <summary>
        /// Represents a small cargo container, which occupies regular space. 
        /// </summary>
        Small = 1,

        /// <summary>
        /// Represents a large cargo container, which occupies double the space of a small container. 
        /// </summary>
        Large = 2,

    }
}
