using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure
{
    /// <summary>
    /// Represents the recurring types for scheduling ship sailings.
    /// </summary>
    public enum RecurringType
    {
       /// <summary>
       /// Represents a non-recurring type sailing. 
       /// </summary>
        None,

        /// <summary>
        /// Represents a daily recurring type sailing. 
        /// </summary>
        Daily,

        /// <summary>
        /// Represents a weekly recurring type sailing. 
        /// </summary>
        Weekly, 

    }
}
