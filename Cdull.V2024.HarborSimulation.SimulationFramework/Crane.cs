using System.ComponentModel;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    /// <summary>
    /// Represents a crane used in harbor to move containers from/to ships/harbor.
    /// </summary>
    public class Crane
    {
        private string Name { get; set; }
        internal int handlingTime { get; set; }
        internal bool IsAvailable { get; set; }   
        internal bool Done { get; set; }
        internal Container Container { get; set; }


        /// <summary>
        /// Initializes a new instance of the Crane class with the specified name.
        /// </summary>
        /// <param name="craneName">The name of the crane.</param>
        public Crane(string craneName)
        {
            Name = craneName;
            handlingTime = 0;
            IsAvailable = true;
            Done = false; 
        }


        /// <summary>
        /// Lifts a container and loads it onto the crane.
        /// </summary>
        /// <param name="container">The container to be lifted.</param>
        /// <exception cref="InvalidOperationException">Thrown when the crane is already loaded with cargo.</exception>
        public void LiftContainer(Container container)
        {
            if (Container != null)
            {
                throw new InvalidOperationException("Crane already loaded with cargo.");
            }
            Container = container;
        }


    }
}
