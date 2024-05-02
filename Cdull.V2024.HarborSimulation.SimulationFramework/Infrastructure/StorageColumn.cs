
namespace Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure
{

    /// <summary>
    /// Represents a storage column in the harbor for storing containers.
    /// </summary>
    public class StorageColumn
    {
        /// <summary>
        /// Represents the Id of the storage column. 
        /// </summary>
        public int ColumnId { get; set; }
        /// <summary>
        /// Represents the total capacity of the storage cloumn. 
        /// </summary>
        public int Capacity { get; set; }
        public int Height { get; private set; }
        public int Length { get; private set; }
        public int Width { get; private set; }
        public List<Container> Containers { get; set; } = new List<Container>();
        internal PortalCrane Crane { get; set; }
        public int OccupiedSpace { get; set; }
        internal bool IsAvailable { get; set; }
        public int Location { get; set; }


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
            ColumnId = columnId;
            Length = columnLength;
            Width = columnWidth;
            Height = columnHeight;
            IsAvailable = true;
            Location = location;
            Crane = new PortalCrane();

            Capacity = Length * Width * Height;
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


            if (!Containers.Contains(container))
            {
                Containers.Add(container);

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

            if (Containers.Contains(container))
            {
                Containers.Remove(container);
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
                Containers.Add(container);
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
            if (OccupiedSpace < Capacity)
            {
                OccupiedSpace += (int)container.Size;
            }

            else
            {
                IsAvailable = false;
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
            if (OccupiedSpace <= Capacity && OccupiedSpace > 0)
            {
                OccupiedSpace -= (int)container.Size;
                harbor.ContainerStorage.Capacity -= (int)container.Size;
                IsAvailable = true;
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
            return OccupiedSpace;
        }


        /// <summary>
        /// Checks if the storage column has enough space for the specified containers.
        /// </summary>
        /// <param name="containers">The list of containers to check.</param>
        /// <returns>True if the storage column has enough space; otherwise, false.</returns>
        internal bool HasEnoughSpaceForContainers(List<Container> containers)
        {
            int requiredSpace = containers.Sum(container => (int)container.Size);

            return Capacity - OccupiedSpace >= requiredSpace;
        }


        /// <summary>
        /// Returns a string representation of the storage column.
        /// </summary>
        /// <returns>A string representing the storage column.</returns>
        public override string ToString()
        {
            return $"Storage Column {ColumnId}";
        }

    }
}
