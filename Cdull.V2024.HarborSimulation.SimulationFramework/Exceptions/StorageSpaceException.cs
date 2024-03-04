using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.Exceptions
{

    public class StorageSpaceException : Exception
    {
        public StorageSpaceException(string message) : base(message)
        {

        }

        public StorageSpaceException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }


}
