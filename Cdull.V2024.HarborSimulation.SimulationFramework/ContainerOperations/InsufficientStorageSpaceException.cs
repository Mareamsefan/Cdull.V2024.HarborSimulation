
namespace Cdull.V2024.HarborSimulation.SimulationFramework.ContainerOperations
{/// <summary>
 /// Represents an exception that is thrown when there is insufficient space in a storage column to add containers.
 /// </summary>
    public class InsufficientStorageSpaceException : Exception
    {
        /// <summary>
        /// Gets the ID of the storage column.
        /// </summary>
        public int ColumnId { get; }

        /// <summary>
        /// Gets the remaining space in the storage column.
        /// </summary>
        public int RemainingSpace { get; }

        /// <summary>
        /// Gets the number of remaining containers that cannot be added due to insufficient space.
        /// </summary>
        public int RemainingContainers { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsufficientStorageSpaceException"/> class with the specified column ID, remaining space, and remaining containers.
        /// </summary>
        /// <param name="columnId">The ID of the storage column.</param>
        /// <param name="remainingSpace">The remaining space in the storage column.</param>
        /// <param name="remainingContainers">The number of remaining containers that cannot be added.</param>
        public InsufficientStorageSpaceException(int columnId, int remainingSpace, int remainingContainers)
        {
            ColumnId = columnId;
            RemainingSpace = remainingSpace;
            RemainingContainers = remainingContainers;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message => $"Not enough space in storage column {ColumnId} to add {RemainingContainers} containers. Remaining space: {RemainingSpace}";
    }


}
