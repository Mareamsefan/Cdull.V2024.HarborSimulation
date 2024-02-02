using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cdull.V2024.HarborSimulation.SimulationFramework.Enums;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Dock
    {
        public string Name { get; }
        public Size Size { get; }
        private DockType Type { get; }
        public bool IsAvalible { get; set; }
        private Crane Crane { get; set; }
        private Ship? OccupiedBy {  get; set; }
        public Dock(string dockName, Size dockSize, DockType dockType, Crane dockCrane) {
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
