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




        public ContainerStorage(string cargoStorageName, int startLocationIndex, int endLocationIndex)
        {
            name = cargoStorageName;

            for (int i = startLocationIndex; i <= endLocationIndex; i++)
            {
                LocationIndexes.Add(i);
            }
        } 
        
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
