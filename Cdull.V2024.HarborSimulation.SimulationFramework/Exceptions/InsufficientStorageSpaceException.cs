using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.Exceptions
{
    public class InsufficientStorageSpaceException : Exception
    {
        public int ColumnId { get; }
        public int RemainingSpace { get; }
        public int RemainingContainers { get; }

        public InsufficientStorageSpaceException(int columnId, int remainingSpace, int remainingContainers)
        {
            ColumnId = columnId;
            RemainingSpace = remainingSpace;
            RemainingContainers = remainingContainers;
        }

        public override string Message => $"Not enough space in storage column {ColumnId} to add {RemainingContainers} containers. Remaining space: {RemainingSpace}";
    }


}
