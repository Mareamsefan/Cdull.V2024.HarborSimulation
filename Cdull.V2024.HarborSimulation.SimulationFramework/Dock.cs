using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    internal class Dock
    {
        private string name { get; }
        private string size { get; }
        private string type { get; }
        private bool isAvalible { get; set; }
        private Crane crane { get; set; }
        private Ship? OccupiedBy {  get; set; }
    public Dock(string dockName, string dockSize, string dockType, Crane dockCrane) {
            this.name = dockName;
            this.size = dockSize; 
            this.type = dockType;
            this.isAvalible = true;
            this.crane = dockCrane;
            this.OccupiedBy = null; 
            
           
        }

    }
}
