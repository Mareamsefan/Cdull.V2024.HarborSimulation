using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{

    public class NotEnoughSpaceInStorage : Exception
    {
        public NotEnoughSpaceInStorage(string message) : base(message)
        {

        }

        public NotEnoughSpaceInStorage(string message, Exception innerException) : base(message, innerException)
        {

        }
    }

    
}
