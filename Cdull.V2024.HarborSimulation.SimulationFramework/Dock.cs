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
        public bool IsAvailable { get; set; }
        private Crane Crane { get; set; }
        public Ship? OccupiedBy {  get; set; }

        

        public Dock(string dockName, Size dockSize, DockType dockType, Crane dockCrane) {
                this.Name = dockName;
                this.Size = dockSize; 
                this.Type = dockType;
                this.IsAvailable = true;
                this.Crane = dockCrane;
                this.OccupiedBy = null; 
            
           
        }

        /// <summary>
        /// A method that returns information about the dock, such as name, size, 
        /// type and the specific crane thats on this particular dock.
        /// </summary>
        /// <returns>A string with info about the dock.</returns>
        public override string ToString()
        {

            string dockInfo = Name + " " + Size + " " + Type + " " + Crane;

            return dockInfo;
        }
        /*
        public string DockedBy()
        {
            if (OccupiedBy != null)
            {
                return OccupiedBy.Name;
            }
            else
            {
                return "No ship docked";
            }
        }*/

    }
}
