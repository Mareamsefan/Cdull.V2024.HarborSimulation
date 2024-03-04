using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.Exceptions
{
    internal class InsufficientCargoException : Exception
    {
        public InsufficientCargoException(string message) : base(message)
        {

        }

        public InsufficientCargoException(string message, Exception innerException) : base(message, innerException)
        {


        }
    }
}
