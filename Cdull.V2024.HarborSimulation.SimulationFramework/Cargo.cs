namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Cargo
    {
        internal string name;
        internal double Weight { get; private set; }
        internal List<String> History { get; } = new List<String>();

        public Cargo(string cargoName, double cargoWeight)
        {
            name = cargoName;
            Weight = cargoWeight;

        }

        /// <summary>
        /// A method that returns information about the cargo, such as name and weight in tons.
        /// </summary>
        /// <returns>Info about the cargo.</returns>
        public override string ToString()
        {

            string cargoInfo = name + " " + Weight;

            return cargoInfo;
        }

    }
}
