using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure
{
    [Flags]
    public enum RecurringType
    {
        /// <summary>
        /// Represents the recurring types for scheduling ship sailings.
        /// </summary>
        Daily,
        Weekly
    }
}
