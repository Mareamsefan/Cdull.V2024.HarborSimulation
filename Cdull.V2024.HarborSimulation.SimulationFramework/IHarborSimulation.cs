

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public interface IHarborSimulation
    {
        /// <summary>
        /// Defines the method signature for running the harbor simulation.
        /// </summary>
        /// <param name="harbor">The harbor object representing the simulation environment.</param>
        /// <param name="startTime">The start time of the simulation.</param>
        /// <param name="endTime">The end time of the simulation.</param>
        /// <param name="ships">The list of ships in the simulation.</param>
        /// <param name="docks">The list of docks in the harbor.</param>
        /// <param name="startSailingTime">The start time for sailing ships.</param>
        /// <param name="destinationLocation">The destination location that the ships will be sailing to(km).</param>
        /// <param name="IsRecurringSailing">bool indicating whether sailing is recurring.</param>
        /// <param name="recurringType">The type of recurring sailing (daily or weekly).</param>
        public void Run(Harbor harbor, DateTime startTime, DateTime endTime, List<Ship> ships, List<Dock> docks, 
            DateTime startSailingTime, int destinationLocation, bool IsRecurringSailing, RecurringType? recurringType=null);




    }


}

