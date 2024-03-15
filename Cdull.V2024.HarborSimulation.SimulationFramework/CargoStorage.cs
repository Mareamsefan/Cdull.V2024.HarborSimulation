using System.Text;
using System.Xml.Linq;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    /// <summary>
    /// Represents a storage facility for cargo items.
    /// </summary>
    public class CargoStorage
    {
        private string name;
        internal List<StorageColumn> StorageColumns { get; set; } = new List<StorageColumn>();
        public int Capacity { get; set; }


        public CargoStorage(string cargoStorageName)
        {
            name = cargoStorageName;

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
            StorageColumns.Add(column); 
            Capacity += column.Capacity;
        }
    }
}
