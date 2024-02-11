﻿using static Cdull.V2024.HarborSimulation.SimulationFramework.Enums;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Dock
    {

        internal string Name { get; }
        internal Size Size { get; }

        internal Model Model  { get; }
        internal bool IsAvailable { get; set; }
        internal List<Crane> Cranes { get; set; } = new List<Crane>();
        internal Ship? OccupiedBy { get; set; }




        public Dock(string dockName, Size dockSize, Model dockModel, List<Crane>? dockCranes = null, Crane? dockCrane = null)
        {
            Name = dockName;
            Size = dockSize;
            Model = dockModel;
            IsAvailable = true;
            OccupiedBy = null;

            if (dockCranes != null)
            {
                Cranes = dockCranes;
            }
            else if (dockCrane != null)
            {
                Cranes.Add(dockCrane);
            }



        }



        /// <summary>
        /// A method that returns information about the dock, such as name, size, 
        /// type and the specific crane thats on this particular dock.
        /// </summary>
        /// <returns>A string with info about the dock.</returns>
        public override string ToString()
        {

            return "\nDock: " + Name + " Size:  " + Size + " Model:  " + Model + "Number of cranes: " + Cranes.Count() + " IsAvailable: " + IsAvailable + "\n";
        }



    }
}
