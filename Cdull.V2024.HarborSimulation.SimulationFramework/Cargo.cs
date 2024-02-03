using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Cargo
    {
        private string name;
        private double weight; 
        private List<String> history = new List<String>();
  
        public Cargo(string cargoName, double cargoWeight) 
        {   name = cargoName; 
            weight = cargoWeight; 
        
        }

        /// <summary>
        /// A method that returns information about the cargo, such as name and weight in tons.
        /// </summary>
        /// <returns>Info about the cargo.</returns>
        public override string ToString()
        {

            string cargoInfo = name + " " +  weight;

            return cargoInfo;
        }

    }
}
