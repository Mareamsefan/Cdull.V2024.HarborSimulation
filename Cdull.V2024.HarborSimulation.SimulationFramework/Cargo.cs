using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Cargo
    {
   
        private string name {  get; set; }
        private double weight { get; set; }
        private List<String> historyList { get; set; } = new List<String>();
        public Cargo(string cargoName, double cargoWeight) 
        {   this.name = cargoName; 
            this.weight = cargoWeight; 
        
        }
        public override string ToString()
        {

            string cargoInfo = name + " " +  weight;

            return cargoInfo;
        }

    }
}
