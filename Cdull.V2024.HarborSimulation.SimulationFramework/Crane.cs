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
        internal Container Container { get; set; }


        /// <summary>
        /// Initializes a new instance of the Crane class with the specified name.
        /// </summary>
        /// <param name="craneName">The name of the crane.</param>
        public Crane(string craneName)
        {
            Name = craneName;
            handlingTime = 60;
        }

        public void LiftContainer(Container container)
        {
            if (Container != null)
            {
                throw new InvalidOperationException("AGV already loaded with cargo.");
            }
            Container = container;
        }


    }
}
