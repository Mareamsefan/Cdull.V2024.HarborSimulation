namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    /// <summary>
    /// Represents cargo that can be loaded onto ships in the harbor simulation.
    /// </summary>
    public class Cargo
    {
        internal string Name { get; set; }
        internal double Weight { get; private set; }
        internal List<String> History { get; } = new List<String>();

        /// <summary>
        /// Initializes a new instance of Cargo class with the specified name and weight.
        /// </summary>
        /// <param name="cargoName">The name of the cargo.</param>
        /// <param name="cargoWeight">The weight of the cargo.</param>
        public Cargo(string cargoName, double cargoWeight)
        {
            Name = cargoName;
            Weight = cargoWeight;

        }


    }
}
