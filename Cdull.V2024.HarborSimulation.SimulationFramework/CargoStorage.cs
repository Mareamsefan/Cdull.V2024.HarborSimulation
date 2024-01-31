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
        private List<Cargo> Cargo {  get; set; } = new List<Cargo>();
        private bool IsAvailable { get; set; }

        public CargoStorage(string cargoStorageName) {

            this.Name = cargoStorageName;
            this.IsAvailable = true;
        }
    }
}
