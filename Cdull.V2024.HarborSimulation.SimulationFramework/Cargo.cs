using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Cargo
    {
        private string Name {  get; set; }
        private double Weight { get; set; }
        private List<String> History { get; set; } = new List<String>();
        public Cargo(string cargoName, double cargoWeight) 
        {   this.Name = cargoName; 
            this.Weight = cargoWeight; 
        
        }
        public override string ToString()
        {

            string cargoInfo = Name + " " +  Weight;

            return cargoInfo;
        }

    }
}
