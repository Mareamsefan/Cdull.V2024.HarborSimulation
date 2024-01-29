using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Crane
    {   
        private string name {  get; set; }
        private bool isCraneAvalible { get; set; }
        private bool isCraneOutOfFuntion {  get; set; }

        public Crane(string craneName) {
            this.name = craneName;
            this.isCraneAvalible = true;
            this.isCraneOutOfFuntion = false;
        }

        public override string ToString()
        {

            return $"Crane Name: {name}";
        }
    }
}
