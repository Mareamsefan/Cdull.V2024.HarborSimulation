﻿
namespace Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure
{
    internal class PortalCrane
    {
        /// <summary>
        /// Represents a Poralcrane used in harbor to Move containers from AVG to container storage.
        /// </summary>
        private bool _isAvailable;

        private int _handlingTime;

        private Container _container; 


        /// <summary>
        /// Initializes a new instance of the PortalCrane class.
        /// </summary>
        internal PortalCrane()
        {
            _handlingTime = 0;
            _isAvailable = true;
        }

        // <summary>
        /// Lifts a container with the portal crane.
        /// </summary>
        /// <param name="container">The container to be lifted.</param>
        /// <exception cref="InvalidOperationException">Thrown when the portal crane is already loaded with cargo.</exception>
        internal void LiftContainer(Container container)
        {
            if (_container != null)
            {
                throw new InvalidOperationException("Portalcrane already loaded with cargo.");
            }
            _container = container;
        }

        /// <summary>
        /// Gets the container currently lifted by the portal crane.
        /// </summary>
        /// <returns>The container currently lifted by the portal crane, or null if no container is lifted.</returns>
        public Container GetContainer => _container;


        /// <summary>
        /// Sets the container to be loaded onto the portal crane.
        /// </summary>
        /// <param name="container">The container to be loaded onto the portal crane.</param>
        public void SetContainer(Container container) => _container = container;    

        /// <summary>
        /// Gets a bool-value indicating whether the portal crane is available for use.
        /// </summary>
        /// <returns>True if the portal crane is available; otherwise, false.</returns>
        public bool GetIsAvailable => _isAvailable;


        /// <summary>
        /// Sets the availability status of the object.
        /// </summary>
        /// <param name="isAvailable">True if the object is available; otherwise, false.</param>
        public void SetIsAvailable(bool isAvailable) => _isAvailable = isAvailable;

        /// <summary>
        /// Gets the time it takes for the portal crane to handle a container.
        /// </summary>
        /// <returns>The handling time of the portal crane in minutes.</returns>
        public int GetHandlingTime => _handlingTime;

        /// <summary>
        /// Sets the handling time for the portal crane.
        /// </summary>
        /// <param name="handlingTime">The new handling time to set.</param>

        public void SetHandlingTime(int handlingTime) => _handlingTime = handlingTime;


    }

}
