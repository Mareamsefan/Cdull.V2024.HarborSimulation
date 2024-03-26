
using Cdull.V2024.HarborSimulation.SimulationFramework.Enums;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    /// <summary>
    /// Represents a dock in the harbor.
    /// </summary>
    public class Dock
    {
        internal string Name { get; set; }
        internal Size Size { get; set; }
        internal bool IsAvailable { get; set; }
        public List<Crane> Cranes { get; set; } = new List<Crane>();
        internal Ship? OccupiedBy { get; set; }
        internal int numberOfShipsPerDay { get; set; }


        /// <summary>
        /// Initializes a new instance of the Dock class with the specified name, size, and model.
        /// </summary>
        /// <param name="dockName">The name of the dock.</param>
        /// <param name="dockSize">The size of the dock.</param>
        /// <param name="dockCranes">Optional list of cranes available at the dock.</param>
        /// <param name="dockCrane">Optional single crane available at the dock.</param>
        public Dock(string dockName, Size dockSize, List<Crane>? dockCranes = null, Crane? dockCrane = null)
        {
            Name = dockName;
            Size = dockSize;
            IsAvailable = true;
            OccupiedBy = null;
            numberOfShipsPerDay = 0;

            if (dockCranes != null)
            {
                Cranes = dockCranes;
            }
            else if (dockCrane != null)
            {
                Cranes.Add(dockCrane);
            }

        }

        /// <summary>
        /// Checks if the specified crane is available.
        /// </summary>
        /// <param name="crane">The crane to check.</param>
        /// <returns>True if the crane is available, otherwise false.</returns>
        public bool GetAvailableCrane(Crane crane)
        {
            if (crane != null && crane.IsAvailable)
            {
                return true;
            }

            return false;
        }


    }
}
