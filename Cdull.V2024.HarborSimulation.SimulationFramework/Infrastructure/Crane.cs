using System.ComponentModel;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure
{
    /// <summary>
    /// Represents a crane used in the harbor to Move containers from/to ships/harbor.
    /// </summary>
    public class Crane
    {
        private int _id;

        private int _handlingTime;

        private bool _isAvailable;

        private Container? _container; 

        /// <summary>
        /// Initializes a new instance of the Crane class with the specified _Id.
        /// </summary>
        /// <param name="craneId">The _Id of the crane.</param>
        /// <example>
        /// This example shows how to use the Crane constructor to create a new crane instance.
        /// <code>
        ///     int craneId = 1;
        ///     Crane crane = new Crane(craneId);
        /// </code>
        /// </example>
        public Crane(int craneId)
        {
            _id = craneId;
            _handlingTime = 0;
            _isAvailable = true;
        }



        /// <summary>
        /// Lifts a container and loads it onto the crane.
        /// </summary>
        /// <param name="container">The container to be lifted.</param>
        /// <exception cref="InvalidOperationException">Thrown when the crane is already loaded with cargo.</exception>
        internal void LiftContainer(Container container)
        {
            if (container != null)
            {
                throw new InvalidOperationException("Crane already loaded with cargo.");
            }
            _container = container;
        }

        /// <summary>
        /// Unloads the container from the crane.
        /// </summary>
        /// <returns>The container that was unloaded.</returns>
        /// <exception cref="InvalidOperationException">Thrown when there is no container to unload.</exception>
        internal Container UnloadContainer()
        {
            if (_container == null)
            {
                throw new InvalidOperationException("No container to unload.");
            }
            Container containerToUnload = _container;
            _container = null;
            return containerToUnload;
        }

        /// <summary>
        /// Gets the _Id of the crane.
        /// </summary>
        public int GetId => _id;

        /// <summary>
        /// Gets the handling time of the crane.
        /// </summary>
        public int GetHandlingTime => _handlingTime;

        /// <summary>
        /// Sets the handling time of the crane.
        /// </summary>
        public void SetHandlingTime(int handlingTime) => _handlingTime = handlingTime;


       
        /// <summary>
        /// Gets the availability status of the crane.
        /// </summary>
        public bool GetIsAvailable => _isAvailable;

        /// <summary>
        /// Sets the availability status of the crane.
        /// </summary>
        public void SetIsAvailable(bool isAvailable) => _isAvailable = isAvailable;

        /// <summary>
        /// Gets the container currently loaded on the crane.
        /// </summary>
        public Container? GetContainer => _container;

        /// <summary>
        /// Sets the container to be loaded onto the portal crane.
        /// </summary>
        /// <param name="container">The container to be loaded onto the portal crane.</param>
        public void SetContainer(Container container) => _container = container;
    }

}
