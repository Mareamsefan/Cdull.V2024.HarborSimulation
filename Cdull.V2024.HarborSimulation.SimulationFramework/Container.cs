﻿
using Cdull.V2024.HarborSimulation.SimulationFramework.Enums;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    /// <summary>
    /// Represents container that can be loaded onto ships in the harbor simulation.
    /// </summary>
    public class Container
    {
        private static int lastId = 0;
        internal int Id; 
        internal string Name { get; set; }
        internal List<String> History { get; } = new List<String>();
        internal ContainerSize Size { get; set; }
        internal int numberOfDaysInStorage { get; set; }

        /// <summary>
        /// Initializes a new instance of Container class with the specified name and weight.
        /// </summary>
        /// <param name="cargoName">The name of the cargo.</param>
        public Container(string containerName, ContainerSize containerSize)
        {
            Id = ++lastId;
            Name = containerName;
            Size = containerSize;
            numberOfDaysInStorage = 0; 

        }

        /// <summary>
        /// Retrieves the history of the container.
        /// </summary>
        /// <returns>A list of strings representing the c's history.</returns>
        public List<string> GetContainerHistory()
        {
            return History;
        }


    }
}
