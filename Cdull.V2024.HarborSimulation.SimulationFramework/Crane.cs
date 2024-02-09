namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Crane
    {
        private string name;
        private bool isCraneAvalible;
        private bool isCraneOutOfFuntion;

        public Crane(string craneName)
        {
            name = craneName;
            isCraneAvalible = true;
            isCraneOutOfFuntion = false;
        }

        /// <summary>
        /// A method to returns information about the crane.
        /// </summary>
        /// <returns>The name of the crane.</returns>
        public override string ToString()
        {
            return $"Crane Name: {name}";
        }
    }
}
