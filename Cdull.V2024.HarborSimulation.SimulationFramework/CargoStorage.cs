using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class CargoStorage
    {
        private string name {  get; set; }
        private List<Cargo> cargoList {  get; set; } = new List<Cargo>();
        private bool isAvailable { get; set; }

        public CargoStorage(string cargoStoargeName) {
            this.name = cargoStoargeName;
            isAvailable = true;
        }
        public void AddCargoToList(Cargo cargo)
        {
            cargoList.Add(cargo);
        }
    }
}
