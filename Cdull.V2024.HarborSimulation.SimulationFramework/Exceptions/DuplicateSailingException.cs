using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.Exceptions
{
    public class DuplicateSailingException : Exception
    {
        public DuplicateSailingException(string message) : base(message)
        {

        }

        public DuplicateSailingException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
