namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    /// <summary>
    /// Represents a crane used in harbor to move containers from/to ships/harbor.
    /// </summary>
    public class Crane
    {
        private string Name { get; set; }
        internal int Speed { get; set; }
        private bool IsCraneAvalible {  get; set; }
        private bool IsCraneOutOfFuntion { get; set; }

        /// <summary>
        /// Initializes a new instance of the Crane class with the specified name.
        /// </summary>
        /// <param name="craneName">The name of the crane.</param>
        public Crane(string craneName)
        {
            Name = craneName;
            IsCraneAvalible = true;
            IsCraneOutOfFuntion = false;
            Speed = 100; 
        }

    }
}
