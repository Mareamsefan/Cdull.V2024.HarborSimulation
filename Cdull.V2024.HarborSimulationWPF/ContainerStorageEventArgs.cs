using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulationWPF
{
    public class ContainerStorageEventArgs : EventArgs
    {
        public ContainerStorage CreatedContainerStorage { get; }

        public ContainerStorageEventArgs(ContainerStorage containerStorage)
        {
            CreatedContainerStorage = containerStorage;
        }
    }

}
