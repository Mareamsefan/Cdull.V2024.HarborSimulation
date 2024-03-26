using Cdull.V2024.HarborSimulation.SimulationFramework.Enums;
using System;
using System.Text;
using System.Xml.Linq;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    /// <summary>
    /// Represents a storage facility for container items.
    /// </summary>
    public class ContainerStorage
    {
        private string name;
        internal List<StorageColumn> StorageColumns { get; set; } = new List<StorageColumn>();
        internal List<int> LocationIndexes { get; set; } = new List<int> ();
        public int Capacity { get; set; }




        /// <summary>
        /// Initializes a new instance of the ContainerStorage class with the specified name and location indexes range.
        /// </summary>
        /// <param name="cargoStorageName">The name of the container storage.</param>
        /// <param name="startLocationIndex">The starting location index.</param>
        /// <param name="endLocationIndex">The ending location index.</param>
        public ContainerStorage(string cargoStorageName, int startLocationIndex, int endLocationIndex)
        {
            name = cargoStorageName;

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
        public StorageColumn GetSpecificColumn(int columnId)
        {
           StorageColumn correctColumn = StorageColumns.First(); 

           foreach(StorageColumn storageColumn in StorageColumns)
            {
                if(storageColumn.ColumnId.Equals(columnId))
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
        public void AddStorageColumn(StorageColumn column)
        {
            if (!LocationIndexes.Contains(column.Location))
            {
                throw new ArgumentException("The specified column location is outside the range of this storage."); 
            }
            else {
                StorageColumns.Add(column);
                Capacity += column.Capacity;
            }
         
        }

        // <summary>
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
