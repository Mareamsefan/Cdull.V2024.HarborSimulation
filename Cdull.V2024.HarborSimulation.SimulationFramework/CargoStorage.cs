using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    internal class CargoStorage
    {
        private int number {  get; set; }
        private bool isAvailable { get; set; }

        public CargoStorage(int cargoStorageNumber) {
            this.number = cargoStorageNumber;
            isAvailable = true;
        }
    }
}
