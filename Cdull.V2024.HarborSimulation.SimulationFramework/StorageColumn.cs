using Cdull.V2024.HarborSimulation.SimulationFramework.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{

    /// <summary>
    /// Represents a storage column in the harbor for storing containers.
    /// </summary>
    public class StorageColumn
    {
        public int ColumnId { get; set;  }
        internal int Height { get; private set; }
        internal int Length { get; private set; }
        internal int Width { get; private set; }
        public int Capacity { get; set; }
        internal List<Container> Containers {  get; set; } = new List<Container>();
        internal PortalCrane Crane { get; set; } 
        internal int OccupiedSpace { get; set; }
        internal bool IsAvailable { get; set; }
        internal int Location { get; set; }


        /// <summary>
        /// Represents a storage column in the harbor for storing containers.
        /// </summary>
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
        /// Returns a string representation of the harbor simulation.
        /// </summary>
        /// <returns>A string containing information about the harbor simulation, including harbor name, current time, ship counts, and container storage details.</returns>
        public override string ToString()
        {
            return $"{ColumnId}"; 
        }


        /// <summary>
        /// Checks if the storage column has enough space for the specified containers.
        /// </summary>
        /// <param name="containers">The list of containers to check.</param>
        /// <returns>True if the storage column has enough space; otherwise, false.</returns>
        public bool HasEnoughSpaceForContainers(List<Container> containers)
        {
            int requiredSpace = containers.Sum(container => (int)container.Size);

            return Capacity - OccupiedSpace >= requiredSpace;
        }


    }
}
