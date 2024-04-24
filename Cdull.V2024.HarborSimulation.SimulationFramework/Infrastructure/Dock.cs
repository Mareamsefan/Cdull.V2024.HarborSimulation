

namespace Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure
{
    /// <summary>
    /// Represents a dock in the harbor.
    /// </summary>
    public class Dock
    {
        /// <summary>
        /// Represents the Id of the dock. 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents the size of the dock. 
        /// </summary>
        public Size Size { get; set; }
        internal bool IsAvailable { get; set; }

        /// <summary>
        /// Represents a list of the total cranes on dock. 
        /// </summary>
        public List<Crane> Cranes { get; set; } = new List<Crane>();
        internal Ship? OccupiedBy { get; set; }
        internal int numberOfShipsPerDay { get; set; }


        /// <summary>
        /// Initializes a new instance of the Dock class with the specified Id, size, and model.
        /// </summary>
        /// <param name="dockId">The Id of the dock.</param>
        /// <param name="dockSize">The size of the dock.</param>
        /// <param name="dockCranes">Optional list of cranes available at the dock.</param>
        /// <param name="dockCrane">Optional single crane available at the dock.</param>
        /// <example>
        /// This example shows how to use the Dock constructor to create a new dock instance with optional cranes.
        /// <code>
        ///     int dockId = 1;
        ///     Size dockSize = Size.Large;
        ///     List<Crane> dockCranes = new List<Crane> { new Crane(1), new Crane(2) };
        ///     Dock dock = new Dock(dockName, dockSize, dockCranes);
        /// </code>
        /// </example>
        public Dock(int dockId, Size dockSize, List<Crane>? dockCranes = null, Crane? dockCrane = null)
        {
            Id = dockId;
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
        internal bool GetAvailableCrane(Crane crane)
        {
            if (crane != null && crane.IsAvailable)
            {
                return true;
            }

            return false;
        }


    }
}
