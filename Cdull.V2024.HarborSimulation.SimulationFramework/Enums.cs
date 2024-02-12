namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    /// <summary>
    /// Contains enums used in the harbor simulation.
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// Represents the model types of ships and docks.
        /// </summary>
        public enum Model
        {
            ContainerShip,
            Bulker,
            Tanker,
            LNGCarrier,
            RoRo

        }

        /// <summary>
        /// Represents the size categories of ships and docks.
        /// </summary>
        public enum Size
        {
            Small,
            Medium,
            Large
        }

        /// <summary>
        /// Represents the recurring types for scheduling ship sailings.
        /// </summary>
        public enum RecurringType
        {
            Daily,
            Weekly
        }
    }
}
