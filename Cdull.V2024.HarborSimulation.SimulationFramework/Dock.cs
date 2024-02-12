using static Cdull.V2024.HarborSimulation.SimulationFramework.Enums;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    /// <summary>
    /// Represents a dock in the harbor.
    /// </summary>
    public class Dock
    {
        internal string Name { get; set; }
        internal Size Size { get; set; }
        internal Model Model  { get; set; }
        internal bool IsAvailable { get; set; }
        internal List<Crane> Cranes { get; set; } = new List<Crane>();
        internal Ship? OccupiedBy { get; set; }


        /// <summary>
        /// Initializes a new instance of the Dock class with the specified name, size, and model.
        /// </summary>
        /// <param name="dockName">The name of the dock.</param>
        /// <param name="dockSize">The size of the dock.</param>
        /// <param name="dockModel">The model of the dock.</param>
        /// <param name="dockCranes">Optional list of cranes available at the dock.</param>
        /// <param name="dockCrane">Optional single crane available at the dock.</param>
        public Dock(string dockName, Size dockSize, Model dockModel, List<Crane>? dockCranes = null, Crane? dockCrane = null)
        {
            Name = dockName;
            Size = dockSize;
            Model = dockModel;
            IsAvailable = true;
            OccupiedBy = null;

            if (dockCranes != null)
            {
                Cranes = dockCranes;
            }
            else if (dockCrane != null)
            {
                Cranes.Add(dockCrane);
            }


        }



    }
}
