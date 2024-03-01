using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{

    public class AddCargoToStorageException : Exception
    {
        public AddCargoToStorageException(string message) : base(message)
        {

        }

        public AddCargoToStorageException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }

    
}
