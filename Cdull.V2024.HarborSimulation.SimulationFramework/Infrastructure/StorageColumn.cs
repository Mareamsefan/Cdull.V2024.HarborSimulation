
namespace Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure
{

    /// <summary>
    /// Represents a storage column in the harbor for storing containers.
    /// </summary>
    public class StorageColumn
    {
      
        private int _id;
       
        private int _capacity;

        private int _height;

        private int _length;

        private int _width; 

        private  List<Container> _containers = new List<Container>();

        private PortalCrane _crane;

        private int _occupiedSpace;

        private bool _isAvailable;

        private int _location; 


        /// <summary>
        /// Initializes a new instance of the <see cref="StorageColumn"/> class.
        /// </summary>
        /// <param name="location">The location of the storage column.</param>
        /// <param name="columnId">The ID of the storage column.</param>
        /// <param name="columnLength">The length of the storage column.</param>
        /// <param name="columnWidth">The width of the storage column.</param>
        /// <param name="columnHeight">The height of the storage column.</param>
        public StorageColumn(int location, int columnId, int columnLength, int columnWidth, int columnHeight)
        {
            _id = columnId;
            _length = columnLength;
            _width = columnWidth;
            _height = columnHeight;
            _isAvailable = true;
            _location = location;
            _crane = new PortalCrane();

            _capacity = _length * _width * _height;
        }

        /// <summary>
        /// Adds the specified container to the container storage.
        /// </summary>
        /// <param name="container">The container object to be added.</param>
        /// <remarks>
        /// This method adds the specified container to the container storage if it is not already present.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when the container parameter is null.</exception>
        internal void AddContainer(Container container)
        {
            
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container), "Containers parameter cannot be null.");
            }
            

            if (!_containers.Contains(container))
            {
                _containers.Add(container);

            }
        }

        /// <summary>
        /// Removes the specified container from the container storage.
        /// </summary>
        /// <param name="container">The container object to be removed.</param>
        /// <remarks>
        /// This method removes the specified container from the container storage if it exists.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when the container parameter is null.</exception>
        internal void RemoveContainer(Container container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container), "Containers parameter cannot be null.");
            }

            if (_containers.Contains(container))
            {
                _containers.Remove(container);
            }

        }


        /// <summary>
        /// Initializes container on the ship.
        /// </summary>
        /// <param name="number">The number of container units to initialize.</param>
        /// <param name="size">The size of each container unit. Default is set to CargoSize.Large.</param>
        /// <example>
        /// This example shows how to use the InitializeContainers method
        /// <code>
        /// Initialize 5 large containers on the ship
        /// StorageColumn column = new StorageColumn();
        /// column.InitializeContainers(5);
        /// </code>
        /// </example>
        public void InitializeContainers(int number, ContainerSize size = ContainerSize.Large)
        {
            for (int i = 0; i < number; i++)
            {
                Container container = new($"container{i}", size);
                _containers.Add(container);
            }

        }

        /// <summary>
        /// Occupies space in the container storage by adding the weight of the provided container.
        /// </summary>
        /// <param name="container">The container to occupy space for.</param>
        /// <remarks>
        /// This method adds the weight of the provided container to the occupied space of the container storage.
        /// If the occupied space exceeds the capacity of the container storage, it marks the storage as unavailable.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown when the container storage capacity is exceeded.</exception>
        internal void OccupySpace(Container container)
        {
            if (_occupiedSpace < _capacity)
            {
                _occupiedSpace += (int)container.GetSize;
            }

            else
            {
                _isAvailable = false;
                throw new InvalidOperationException("Containers storage capacity exceeded.");
            }

        }

        /// <summary>
        /// Frees up space in the container storage by subtracting the weight of the provided container.
        /// </summary>
        /// <param name="container">The container to free up space for.</param>
        /// <remarks>
        /// This method subtracts the weight of the provided container from the occupied space of the container storage.
        /// If the occupied space becomes less than or equal to the capacity of the container storage, it marks the storage as available.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown when the occupied space or capacity is invalid.</exception>
        internal void deOccupySpace(Container container, Harbor harbor)
        {
            if (_occupiedSpace <= _capacity && _occupiedSpace > 0)
            {
                _occupiedSpace -= (int)container.GetSize;
                int updatedCapacity = harbor.GetContainerStorage.GetCapacity - (int)container.GetSize; 
                harbor.GetContainerStorage.SetCapacity(updatedCapacity);
                _isAvailable = true;
            }
            else
            {
                throw new InvalidOperationException("Invalid occupied space or capacity.");
            }

        }

        /// <summary>
        /// Returns the occupied space in the container storage based on the weight of the stored container.
        /// </summary>
        /// <returns>The total weight of the container stored in the container storage.</returns>
        public int GetOccupiedSpace()
        {
            return _occupiedSpace;
        }


        /// <summary>
        /// Checks if the storage column has enough space for the specified containers.
        /// </summary>
        /// <param name="containers">The list of containers to check.</param>
        /// <returns>True if the storage column has enough space; otherwise, false.</returns>
        internal bool HasEnoughSpaceForContainers(List<Container> containers)
        {
            int requiredSpace = containers.Sum(container => (int)container.GetSize);

            return _capacity - _occupiedSpace >= requiredSpace;
        }


        /// <summary>
        /// Gets the Id of the Storage cloumn.
        /// </summary>
        public int GetId => _id;

        /// <summary>
        /// Gets the capacity of the Storage cloumn.
        /// </summary>
        public int GetCapacity => _capacity;

        /// <summary>
        /// Gets the height of the Storage cloumn.
        /// </summary>
        public int GetHeight => _height;

        /// <summary>
        /// Gets the length of the Storage cloumn.
        /// </summary>
        public int GetLength => _length;

        /// <summary>
        /// Gets the width of the Storage cloumn.
        /// </summary>
        public int GetWidth => _width;

        /// <summary>
        /// Gets the list of containers stored in the Storage cloumn.
        /// </summary>
        public List<Container> GetContainers => _containers;


        /// <summary>
        /// Gets whether the Storage cloumn is available.
        /// </summary>
        public bool GetIsAvailable => _isAvailable;

        /// <summary>
        /// Sets whether the Storage cloumn is available.
        /// </summary>
        /// <param name="isAvailable">True if the Storage cloumn is available; otherwise, false.</param>
        public void SetIsAvailable(bool isAvailable) => _isAvailable = isAvailable;

        /// <summary>
        /// Gets the location of the Storage cloumn.
        /// </summary>
        public int GetLocation => _location;

        /// <summary>
        /// Sets the location of the Storage cloumn.
        /// </summary>
        /// <param name="location">The new location of the Storage cloumn.</param>
        public void SetLocation(int location) => _location = location;

        /// <summary>
        /// Returns a string representation of the storage column.
        /// </summary>
        /// <returns>A string representing the storage column.</returns>
        public override string ToString()
        {
            return $"Storage Column {_id}";
        }


    

    }
}
