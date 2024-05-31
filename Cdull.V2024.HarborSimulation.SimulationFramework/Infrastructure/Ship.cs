using Cdull.V2024.HarborSimulation.SimulationFramework.ContainerOperations;
using Cdull.V2024.HarborSimulation.SimulationFramework.ShipOperations;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure
{
    /// <summary>
    /// Represents a ship in the harbor simulation.
    /// </summary>
    public class Ship
    {
        private string _name;

        private Model _model;

        private Size _size;

        private bool _hasDocked; 

        private List<Container> _containers = new List<Container>();

        private List<string> _history  = new List<string>();

        private string _dockedAtTime;

        private string _sailedAtTime;

        private bool _isSailing;

        private bool _isReadyToSail;

        private float _speed;

        private Dock? _dockedAt;

        private int _currentLocation;

        private bool _hasReachedDestination;

        private ShipSailingState _sailingState; 

        internal List<ScheduledContainerHandling> ScheduledContainerHandlings { get; set; } = new List<ScheduledContainerHandling>();
        internal List<Sailing> ScheduledSailings { get; set; } = new List<Sailing>();

        /// <summary>
        /// Initializes a new instance of the Ship class.
        /// </summary>
        /// <param name="shipName">The name of the ship.</param>
        /// <param name="shipModel">The model of the ship.</param>
        /// <param name="shipSize">The size of the ship.</param>
        public Ship(string shipName, Model shipModel, Size shipSize, int shipCurrentLocation)
        {
            _name = shipName;
            _model = shipModel;
            _size = shipSize;
            _hasDocked = false;
            _isSailing = false;
            _dockedAt = null;
            _dockedAtTime = "";
            _sailedAtTime = "";
            _speed = GetSpeedFromModel(shipModel);
            _isReadyToSail = false;
            _currentLocation = shipCurrentLocation;
        }
       

        /// <summary>
        /// Gets the speed of the ship based on its model.
        /// </summary>
        /// <param name="model">The model of the ship.</param>
        /// <returns>The speed of the ship.</returns>
        private int GetSpeedFromModel(Model model)
        {
            return (int)model;
        }

        /// <summary>
        /// Initializes container on the ship.
        /// </summary>
        /// <param name="number">The number of container units to initialize.</param>
        /// <param name="size">The size of each container unit. Default is set to CargoSize.Large.</param>
        public void InitializeContainers(int number, ContainerSize size = ContainerSize.Large)
        {
            if (_model != Model.ContainerShip)
            {
                throw new InvalidOperationException("Only _container Ships can carry container.");
            }

            for (int i = 0; i < number; i++)
            {
                Container container = new($"container{i}", size);
                _containers.Add(container);
            }

        }


        /// <summary>
        /// Retrieves the container carried by the ship.
        /// </summary>
        /// <returns>A list of Containers objects representing the ship's container.</returns>
        public List<Container> GetShipContainers()
        {
            return _containers;
        }



        /// <summary>
        /// Returns a string representation of the ship with only its name and model.
        /// </summary>
        /// <returns>A string representing the ship's name and model.</returns>
        public override string ToString()
        {
            return $"_name: {_name}, _model: {_model}";
        }

        /// <summary>
        /// Gets the name of the ship.
        /// </summary>
        public string GetName => _name;

        /// <summary>
        /// Gets the model of the ship.
        /// </summary>
        public Model GetModel => _model;

        /// <summary>
        /// Gets the size of the ship.
        /// </summary>
        public Size GetSize => _size;

        /// <summary>
        /// Gets whether the ship has docked.
        /// </summary>
        public bool GetHasDocked => _hasDocked;

        /// <summary>
        /// Sets whether the ship has docked.
        /// </summary>
        public void SetHasDocked(bool hasDocked) => _hasDocked = hasDocked;

        /// <summary>
        /// Gets the list of containers on the ship.
        /// </summary>
        public List<Container> GetContainers => _containers;

        /// <summary>
        /// Gets the history of the ship.
        /// </summary>
        public List<string> GetHistory => _history;

        /// <summary>
        /// Gets the time when the ship docked.
        /// </summary>
        public string GetDockedAtTime => _dockedAtTime;

        /// <summary>
        /// Sets the time when the ship docked.
        /// </summary>
        /// <param name="dockedAtTime">The time when the ship docked.</param>
        public void SetDockedAtTime(string dockedAtTime) => _dockedAtTime = dockedAtTime;

        /// <summary>
        /// Gets the time when the ship sailed.
        /// </summary>
        public string GetSailedAtTime => _sailedAtTime;

        /// <summary>
        /// Sets the time when the ship sailed.
        /// </summary>
        /// <param name="sailedAtTime">The time when the ship sailed.</param>
        public void SetSailedAtTime(string sailedAtTime) => _sailedAtTime = sailedAtTime;

        /// <summary>
        /// Gets whether the ship is sailing.
        /// </summary>
        public bool GetIsSailing => _isSailing;

        /// <summary>
        /// Sets whether the ship is sailing.
        /// </summary>
        /// <param name="isSailing">True if the ship is sailing; otherwise, false.</param>
        public void SetIsSailing(bool isSailing) => _isSailing = isSailing;

        /// <summary>
        /// Gets whether the ship is ready to sail.
        /// </summary>
        public bool GetIsReadyToSail => _isReadyToSail;

        /// <summary>
        /// Sets whether the ship is ready to sail.
        /// </summary>
        /// <param name="isReadyTosail">True if the ship is ready to sail; otherwise, false.</param>
        public void SetIsReadyToSail(bool isReadyTosail) => _isReadyToSail = isReadyTosail;

        /// <summary>
        /// Gets the speed of the ship.
        /// </summary>
        public float GetSpeed => _speed;

        /// <summary>
        /// Gets the dock where the ship is docked.
        /// </summary>
        public Dock GetDockedAt => _dockedAt;

        /// <summary>
        /// Sets the dock where the ship is docked.
        /// </summary>
        /// <param name="dockedAt">The dock where the ship is docked.</param>
        public void SetDockedAt(Dock dockedAt) => _dockedAt = dockedAt;

        /// <summary>
        /// Gets the current location of the ship.
        /// </summary>
        public int GetCurrentLocation => _currentLocation;

        /// <summary>
        /// Sets the current location of the ship.
        /// </summary>
        /// <param name="currentLocation">The current location of the ship.</param>
        public void SetCurrentLocation(int currentLocation) => _currentLocation = currentLocation;

        /// <summary>
        /// Gets whether the ship has reached its destination.
        /// </summary>
        public bool GetHasReachedDestination => _hasReachedDestination;

        /// <summary>
        /// Sets whether the ship has reached its destination.
        /// </summary>
        /// <param name="hasReachedDestination">True if the ship has reached its destination; otherwise, false.</param>

        public void SetHasReachedDestination(bool hasReachedDestination)=> _hasReachedDestination = hasReachedDestination;

        /// <summary>
        /// Sets the sailing state of the ship.
        /// </summary>
        /// <param name="sailingState">The sailing state of the ship.</param>
        
        internal ShipSailingState GetSailingState => _sailingState;
        /// <summary>
        /// Gets the sailing state of the ship.
        /// </summary>
        internal void SetSailingState(ShipSailingState sailingState) => _sailingState = sailingState;

    }
}
