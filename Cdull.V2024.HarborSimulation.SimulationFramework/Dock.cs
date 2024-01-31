using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Dock
    {
        public string Name { get; }
        public string Size { get; }
        private string Type { get; }
        public bool IsAvalible { get; set; }
        private Crane Crane { get; set; }
        private Ship? OccupiedBy {  get; set; }
        public Dock(string dockName, string dockSize, string dockType, Crane dockCrane) {
                this.Name = dockName;
                this.Size = dockSize; 
                this.Type = dockType;
                this.IsAvalible = true;
                this.Crane = dockCrane;
                this.OccupiedBy = null; 
            
           
        }
        public override string ToString()
        {

            string dockInfo = Name + " " + Size + " " + Type + " " + Crane;

            return dockInfo;
        }

    }
}
