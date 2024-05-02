using System.ComponentModel;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure
{
    /// <summary>
    /// Represents a crane used in the harbor to Move containers from/to ships/harbor.
    /// </summary>
    public class Crane
    {
        /// <summary>
        /// Represents the name of the crane. 
        /// </summary>
        public int Id { get; set; }
        internal int handlingTime { get; set; }
        internal bool IsAvailable { get; set; }
        public Container? Container { get; set; }

        /// <summary>
        /// Initializes a new instance of the Crane class with the specified Id.
        /// </summary>
        /// <param name="craneId">The Id of the crane.</param>
        /// <example>
        /// This example shows how to use the Crane constructor to create a new crane instance.
        /// <code>
        ///     int craneId = 1;
        ///     Crane crane = new Crane(craneId);
        /// </code>
        /// </example>
        public Crane(int craneId)
        {
            Id = craneId;
            handlingTime = 0;
            IsAvailable = true;
        }

        /// <summary>
        /// Lifts a container and loads it onto the crane.
        /// </summary>
        /// <param name="container">The container to be lifted.</param>
        /// <exception cref="InvalidOperationException">Thrown when the crane is already loaded with cargo.</exception>
        internal void LiftContainer(Container container)
        {
            if (Container != null)
            {
                throw new InvalidOperationException("Crane already loaded with cargo.");
            }
            Container = container;
        }
    }

}
