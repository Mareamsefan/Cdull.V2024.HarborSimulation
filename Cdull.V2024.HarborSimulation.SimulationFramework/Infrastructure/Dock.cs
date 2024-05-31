

namespace Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure
{
    /// <summary>
    /// Represents a dock in the harbor.
    /// </summary>
    public class Dock
    {
        private readonly string _name;

        private readonly Size _size;

        private readonly int _numberOfCranes;

        private readonly int _Id;

        private bool _isAvailable; 

        private readonly List<Crane> _cranes = new List<Crane>();

        private Ship? _occupiedBy; 
       
        private readonly int _numberOfShipsPerDay; 



        /// <summary>
        /// Initializes a new instance of the Dock class with the specified _Id, size, and model.
        /// </summary>
        /// <param name="dockId">The _Id of the dock.</param>
        /// <param name="dockSize">The size of the dock.</param>
        /// <param name="dockCranes">Optional list of cranes available at the dock.</param>
        /// <param name="dockCrane">Optional single crane available at the dock.</param>
        /// <example>
        /// This example shows how to use the Dock constructor to create a new dock instance with optional cranes.
        /// <code>
        ///     int dockId = 1;
        ///     Size _size = Size.Large;
        ///     List<Crane> dockCranes = new List<Crane> { new Crane(1), new Crane(2) };
        ///     Dock dock = new Dock(_name, _size, dockCranes);
        /// </code>
        /// </example>
        /// 
        internal Dock(int dockId, Size dockSize)
        {
            _Id = dockId;
            _size = dockSize;
            _isAvailable = true;
            _occupiedBy = null;
            _numberOfShipsPerDay = 0;
        }


        /// <summary>
        /// Initializes a new instance of the Dock class with the specified Id, size, and list of cranes.
        /// </summary>
        /// <param name="dockId">The Id of the dock.</param>
        /// <param name="dockSize">The size of the dock.</param>
        /// <param name="dockCranes">The list of cranes at the dock.</param>

        public Dock(int dockId, Size dockSize, List<Crane> dockCranes)
        {
            _Id = dockId;
            _size = dockSize;
            _isAvailable = true;
            _occupiedBy = null;
            _numberOfShipsPerDay = 0;
            _numberOfCranes = _cranes.Count; 
            _cranes = dockCranes;
        }


        /// <summary>
        /// Initializes a new instance of the Dock class with the specified Id, size, and crane.
        /// </summary>
        /// <param name="dockId">The Id of the dock.</param>
        /// <param name="dockSize">The size of the dock.</param>
        /// <param name="dockCrane">The single crane at the dock.</param>
        public Dock(int dockId, Size dockSize, Crane dockCrane)
        {
            _Id = dockId;
            _size = dockSize;
            _isAvailable = true;
            _occupiedBy = null;
            _numberOfShipsPerDay = 0;
            _numberOfCranes = _cranes.Count; 
            _cranes.Add(dockCrane);      

        }


        /// <summary>
        /// Checks if the specified crane is available.
        /// </summary>
        /// <param name="crane">The crane to check.</param>
        /// <returns>True if the crane is available, otherwise false.</returns>
        internal bool GetAvailableCrane(Crane crane)
        {
            if (crane != null && _isAvailable)
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Gets the Name of the dock.
        /// </summary>
        public string GetName => _name; 

        /// <summary>
        /// Gets the Id of the dock.
        /// </summary>
        public int GetId => _Id;

        /// <summary>
        /// Gets the size of the dock.
        /// </summary>
        public Size GetSize => _size;


        /// <summary>
        /// Gets a value indicating whether the AGV is available for use.
        /// </summary>
        public bool GetIsAvailable => _isAvailable;

        /// <summary>
        /// Sets the availability status of the AGV.
        /// </summary>
        public void SetIsAvailable(bool isAvailable) => _isAvailable = isAvailable; 

        /// <summary>
        /// Gets the list of cranes available at the dock.
        /// </summary>
        public List<Crane> GetCranes => _cranes;

        /// <summary>
        /// Gets the number of cranes available at the dock.
        /// </summary>
        public int GetNumberOfCranes => _numberOfCranes;

        /// <summary>
        /// Gets the number of ships per day.
        /// </summary>
        public int GetNumberOfShipsPerDay => _numberOfShipsPerDay;

        /// <summary>
        /// Updates the number of ships per day.
        /// </summary>
        /// <param name="numberOfShipsPerDay">The new number of ships per day.</param>
        public void UpdateNumberOfShipsPerDay(int numberOfShipsPerDay) => numberOfShipsPerDay = _numberOfShipsPerDay;


        /// <summary>
        /// Gets the ship that the dock is occpied by. 
        /// </summary>
        public Ship GetOccupiedBy => _occupiedBy;

        /// <summary>
        /// Sets the ship currently occupied by the dock.
        /// </summary>
        /// <param name="occupiedBy">The ship currently occupying the dock.</param>
        public void SetOccpuiedBy(Ship occupiedBy) => _occupiedBy = occupiedBy;

    }
}
