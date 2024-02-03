using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class CargoStorage
    { 
        private string Name {  get; set; }
        public List<Cargo> Cargo {  get; set; } = new List<Cargo>();
        private bool IsAvailable { get; set; }

        public CargoStorage(string cargoStorageName) {

            this.Name = cargoStorageName;
            this.IsAvailable = true;
        }

        /// <summary>
        /// A method to add cargo to the cargo storage in the harbor.
        /// </summary>
        /// <param name="cargo">The cargo object thats being added</param>
        public void AddCargo(Cargo cargo)
        {
            Cargo.Add(cargo);
        }

        /// <summary>
        /// A method to remove the cargor from the cargo storage in the harbor.
        /// </summary>
        /// <param name="cargo">The cargo object thats being removed</param>
        public void RemoveCargo(Cargo cargo)
        {
            Cargo.Remove(cargo);
        }
    }
}
