
namespace Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure
{
    /// <summary>
    /// Represents container that can be loaded onto ships in the harbor simulation.
    /// </summary>
    public class Container
    {
        private static int lastId = 0;

        public int Id;
        public string Name { get; set; }
        public List<string> History { get; } = new List<string>();
        public ContainerSize Size { get; set; }
        public int numberOfDaysInStorage { get; set; }

        /// <summary>
        /// Initializes a new instance of Container class with the specified name and weight.
        /// </summary>
        /// <param name="cargoName">The name of the cargo.</param>
        public Container(string containerName, ContainerSize containerSize)
        {
            Id = ++lastId;
            Name = containerName;
            Size = containerSize;
            numberOfDaysInStorage = 0;

        }

        /// <summary>
        /// Retrieves the history of the container.
        /// </summary>
        /// <returns>A list of strings representing the c's history.</returns>
        public List<string> GetContainerHistory()
        {
            return History;
        }


    }
}
