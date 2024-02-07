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
      
        internal string Name { get; }
        internal Size Size { get; }

        private Model model; 
        internal bool IsAvailable { get; set; }
        private List<Crane> cranes { get; set; } = new List<Crane>(); 
        internal Ship? OccupiedBy {  get; set; }

      
        

        public Dock(string dockName, Size dockSize, Model dockModel, List<Crane>? dockCranes = null, Crane? dockCrane = null) {
            Name = dockName;
            Size = dockSize; 
            model = dockModel;
            IsAvailable = true;
            OccupiedBy = null;

            if (dockCranes != null)
            {
                cranes = dockCranes;
            }
            else if(dockCrane != null)
            {
                cranes.Add(dockCrane); 
            }



        }


        /// <summary>
        /// A method that returns information about the dock, such as name, size, 
        /// type and the specific crane thats on this particular dock.
        /// </summary>
        /// <returns>A string with info about the dock.</returns>
        public override string ToString()
        {

            string dockInfo = "Dock: "+ Name + " Size:  " + Size + " Model:  " + model + " Crane:  " + crane + " IsAvailable: " + IsAvailable;

            return dockInfo;
        }

       
  
    }
}
