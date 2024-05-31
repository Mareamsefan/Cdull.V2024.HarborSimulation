
namespace Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure
{
    /// <summary>
    /// Represents a storage facility for container items.
    /// </summary>
    public class ContainerStorage
    {
        private string _name; 

        private List<StorageColumn> _storageColumns = new List<StorageColumn>();

        private List<int> _locationIndexes = new List<int>();

        private int _maxWaitingDays;

        private int _capacity; 

        private int _percentageTakenByTruck;


        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerStorage"/> class with the specified parameters.
        /// </summary>
        /// <param name="containerStorageName">The name of the container storage.</param>
        /// <param name="startLocationIndex">The starting location index.</param>
        /// <param name="endLocationIndex">The ending location index.</param>
        /// <param name="maxWaitingDays">The maximum number of days a container can wait in storage.</param>
        /// <param name="percentageTakenByTruck">The percentage of containers taken by trucks from storage.</param>
        /// <example>
        /// This example shows how to use the <see cref="ContainerStorage"/> constructor to create a new container storage instance.
        /// <code>
        ///     ContainerStorage storage = new ContainerStorage("MainStorage", 0, 500, 30, 10);
        /// </code>
        /// </example>
        public ContainerStorage(string containerStorageName, int startLocationIndex, int endLocationIndex,
            int maxWaitingDays, int percentageTakenByTruck)
        {
            _name = containerStorageName;
            _maxWaitingDays = maxWaitingDays;
            _percentageTakenByTruck = percentageTakenByTruck; 
            for (int i = startLocationIndex; i <= endLocationIndex; i++)
            {
                _locationIndexes.Add(i);
            }
        }


        /// <summary>
        /// Retrieves the storage column with the specified ID.
        /// </summary>
        /// <param name="columnId">The ID of the storage column to retrieve.</param>
        /// <returns>The storage column with the specified ID.</returns>
        internal StorageColumn GetSpecificColumn(int columnId)
        {
            StorageColumn correctColumn = _storageColumns.First();

            foreach (StorageColumn storageColumn in _storageColumns)
            {
                if (storageColumn.GetId.Equals(columnId))
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
            if (!_locationIndexes.Contains(column.GetLocation))
            {
                throw new ArgumentException("The specified column location is outside the range of this storage.");
            }
            else
            {
                _storageColumns.Add(column);
                _capacity += column.GetCapacity;
            }

        }


        /// <summary>
        /// Calculates and returns the total occupied space in the container storage.
        /// </summary>
        /// <returns>The total occupied space in the container storage.</returns>
        public int GetOccupiedSpace()
        {
            int occupiedSpace = 0;
            _storageColumns.ForEach(column =>
            {
                occupiedSpace += column.GetOccupiedSpace();
            });
            return occupiedSpace;
        }


        /// <summary>
        /// Gets the name of the container storage.
        /// </summary>
        public string GetName => _name;
        


        /// <summary>
        /// Gets the maximum waiting days in storage.
        /// </summary>
        public int GetMaxWaitingDays => _maxWaitingDays;
        


        /// <summary>
        /// Gets the capacity of the container storage.
        /// </summary>
        public int GetCapacity => _capacity;

        /// <summary>
        /// Sets the capacity of the container storage.
        /// </summary>
        public void SetCapacity (int capacity) => _capacity = capacity; 

        /// <summary>
        /// Gets the percentage of containers in storage taken by trucks.
        /// </summary>
        public int GetPercentageTakenByTruck => _percentageTakenByTruck;
        


        /// <summary>
        /// Gets the list of storage columns.
        /// </summary>
        public List<StorageColumn> GetStorageColumns => new List<StorageColumn>(_storageColumns);
        


        /// <summary>
        /// Gets the list of location indexes.
        /// </summary>
        public List<int> GetLocationIndexes =>  new List<int>(_locationIndexes);
        



    }
}
