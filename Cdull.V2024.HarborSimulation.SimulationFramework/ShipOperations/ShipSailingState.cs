using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.ShipOperations
{
    internal enum ShipSailingState
    {

        NotSailing, // Skipet seiler ikke
        Sailing,    // Skipet er i ferd med å seile
        Waiting,    // Skipet venter på seiling
        Arrived     // Skipet har nådd destinasjonen

    }
}
