using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.Exceptions
{
    public class TransportationFailedException : Exception
    {
        public TransportationFailedException() : base() { }

        public TransportationFailedException(string message) : base(message) { }

        public TransportationFailedException(string message, Exception innerException) : base(message, innerException) { }
    }

}
