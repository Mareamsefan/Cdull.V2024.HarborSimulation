
namespace Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure
{
    /// <summary>
    /// Represents a storage facility for container items.
    /// </summary>
    public class ContainerStorage
    {
        public string Name { get; set; }
        public List<StorageColumn> StorageColumns { get; set; } = new List<StorageColumn>();
        public List<int> LocationIndexes { get; set; } = new List<int>();
        public int Capacity { get; set; }



        /// <summary>
        /// Initializes a new instance of the ContainerStorage class with the specified name and location indexes range.
        /// </summary>
        /// <param name="containerStorageName">The name of the container storage.</param>
        /// <param name="startLocationIndex">The starting location index.</param>
        /// <param name="endLocationIndex">The ending location index.</param>
        /// <example>
        /// This example shows how to use the ContainerStorage constructor to create a new container storage instance.
        /// <code>
        ///     ContainerStorage storage = new ContainerStorage(("ContainerStorage", 0, 500);
        /// </code>
        /// </example>
        public ContainerStorage(string containerStorageName, int startLocationIndex, int endLocationIndex)
        {
            Name = containerStorageName;

            for (int i = startLocationIndex; i <= endLocationIndex; i++)
            {
                LocationIndexes.Add(i);
            }
        }

        /// <summary>
        /// Retrieves the storage column with the specified ID.
        /// </summary>
        /// <param name="columnId">The ID of the storage column to retrieve.</param>
        /// <returns>The storage column with the specified ID.</returns>
        internal StorageColumn GetSpecificColumn(int columnId)
        {
            StorageColumn correctColumn = StorageColumns.First();

            foreach (StorageColumn storageColumn in StorageColumns)
            {
                if (storageColumn.ColumnId.Equals(columnId))
                {
                    correctColumn = storageColumn;
                }
            }
            return correctColumn;
        }

        /// <summary>
        /// Adds a storage column to the container storage.
        /// </summary>
        /// <param name="column">The storage column to add.</param>
        /// <exception cref="ArgumentException">Thrown when the specified column location is outside the range of this storage.</exception>
        internal void AddStorageColumn(StorageColumn column)
        {
            if (!LocationIndexes.Contains(column.Location))
            {
                throw new ArgumentException("The specified column location is outside the range of this storage.");
            }
            else
            {
                StorageColumns.Add(column);
                Capacity += column.Capacity;
            }

        }

        /// <summary>
        /// Calculates and returns the total occupied space in the container storage.
        /// </summary>
        /// <returns>The total occupied space in the container storage.</returns>
        public int GetOccupiedSpace()
        {
            int occupiedSpace = 0;
            StorageColumns.ForEach(column =>
            {
                occupiedSpace += column.OccupiedSpace;
            });
            return occupiedSpace;
        }


    }
}
