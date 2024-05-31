
namespace Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure
{
    /// <summary>
    /// Represents container that can be loaded onto ships in the harbor simulation.
    /// </summary>
    public class Container
    {
        private static int s_lastId = 0;

        private int _id;

        private string _name;
        
        private List<string> _history = new List<string>();

        private ContainerSize _size;

        private int _numberOfDaysInStorage;


        /// <summary>
        /// Initializes a new instance of _container class with the specified name and size.
        /// </summary>
        /// <param name="containerName">The name of the container.</param>
        /// <param name="containerSize">The size of the container.</param>
        public Container(string containerName, ContainerSize containerSize)
        {
            _id = ++s_lastId;
            _name = containerName;
            _size = containerSize;
            _numberOfDaysInStorage = 0;

        }

   
        /// <summary>
        /// Gets the unique identifier of the container.
        /// </summary>
        public int GetId => _id;

        /// <summary>
        /// Gets the name of the container.
        /// </summary>
        public string GetName => _name;

        /// <summary>
        /// Gets the history of the container.
        /// </summary>
        public List<string> GetHistory => new List<string>(_history);

        /// <summary>
        /// Gets the size of the container.
        /// </summary>
        public ContainerSize GetSize => _size;

        /// <summary>
        /// Gets the number of days the container has been in storage.
        /// </summary>
        public int GetNumberOfDaysInStorage => _numberOfDaysInStorage;

        internal int UpdateNumberofDaysInStorage(int daysInStorage) => _numberOfDaysInStorage = daysInStorage;  

        /// <summary>
        /// Adds a record to the container's history.
        /// </summary>
        /// <param name="record">The history record to add.</param>
        public void AddToHistory(string record) => _history.Add(record);
        

    }
}
