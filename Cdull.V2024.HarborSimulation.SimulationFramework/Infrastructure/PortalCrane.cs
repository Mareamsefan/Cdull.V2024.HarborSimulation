
namespace Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure
{
    public class PortalCrane
    {
        /// <summary>
        /// Represents a Poralcrane used in harbor to Move containers from AVG to container storage.
        /// </summary>

        internal bool IsAvailable { get; set; }

        internal int handlingTime { get; set; }
        internal Container Container { get; set; }


        /// <summary>
        /// Initializes a new instance of the PortalCrane class.
        /// </summary>
        public PortalCrane()
        {
            handlingTime = 0;
            IsAvailable = true;
        }

        // <summary>
        /// Lifts a container with the portal crane.
        /// </summary>
        /// <param name="container">The container to be lifted.</param>
        /// <exception cref="InvalidOperationException">Thrown when the portal crane is already loaded with cargo.</exception>
        internal void LiftContainer(Container container)
        {
            if (Container != null)
            {
                throw new InvalidOperationException("Portalcrane already loaded with cargo.");
            }
            Container = container;
        }

    }

}
