
namespace Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure
{
    /// <summary>
    /// Represents an Automated Guided Vehicle (AGV) used in the harbor.
    /// </summary>

    public class AGV
    {
        /// <summary>
        /// Represents the Id of the AGV. 
        /// </summary>
        public int Id { get; set; }
        internal int Counter { get; set; }
        internal bool IsAvailable { get; set; }
        public Container Container { get; set; }
        public int Location { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AGV"/> class with the specified location.
        /// </summary>
        /// <param name="agvLocation">The initial location of the AGV within the harbor.</param>
        public AGV(int agvLocation)
        {
            Counter++;
            Id = Counter;
            Location = agvLocation;
            IsAvailable = true;
        }

        /// <summary>
        /// Loads the specified container onto the AGV.
        /// </summary>
        /// <param name="container">The container to be loaded onto the AGV.</param>
        /// <remarks>
        /// This method loads the specified container onto the AGV for transportation within the harbor.
        /// </remarks>
        internal void LoadContainerToAGV(Container container)
        {

            Container = container;

        }
    }
}
