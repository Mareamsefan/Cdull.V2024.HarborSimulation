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
        internal List<Cargo> Cargo { get; set; } = new List<Cargo>();
        internal List<StorageColumns> StorageColumns { get; set; } = new List<StorageColumns>();
        public int Capacity { get; set; }


        public CargoStorage(string cargoStorageName)
        {
            name = cargoStorageName;

            foreach (StorageColumns column in StorageColumns)
            {
                Capacity += column.Capacity;
            }

        }        
    }
}
