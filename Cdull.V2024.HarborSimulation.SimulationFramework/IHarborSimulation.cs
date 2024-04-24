using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public interface IHarborSimulation
    {
        /// <summary>
        /// Runs the harbor simulation from the specified start time to the end time.
        /// </summary>
        /// <param name="harbor">The harbor where the simulation takes place.</param>
        /// <param name="startTime">The start time of the simulation.</param>
        /// <param name="endTime">The end time of the simulation.</param>
        /// <param name="ships">The list of ships in the harbor.</param>
        /// <param name="docks">The list of docks in the harbor.</param>
        /// <remarks>
        /// This method simulates harbor activities such as queuing ships, docking ships, adding cargo to storage, adding cargo to ships,
        /// and starting scheduled sailings within the specified time frame.
        /// </remarks>
        /// <example>
        /// This example shows how to use the Run method.
        /// <code>
        /// IHarborSimulation driver = new Simulation();
        /// driver.Run(harbor, startTime, endTime, ships, docks, agvs, storageColumns);
        /// </code>
        /// </example>
        public void Run(Harbor harbor, DateTime startTime, DateTime endTime, List<Ship> ships, List<Dock> docks, List<AGV> agvs, List<StorageColumn> storageColumns);



    }


}

