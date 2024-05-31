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

        public List<string> GetHistory => _history; 

        public string GetDockedAtTime => _dockedAtTime;

        public void SetDockedAtTime(string dockedAtTime) => _dockedAtTime = dockedAtTime;

        public string GetSailedAtTime => _sailedAtTime;

        public void SetSailedAtTime(string sailedAtTime) => _sailedAtTime = sailedAtTime;

        public bool GetIsSailing => _isSailing;

        public void SetIsSailing(bool isSailing) => _isSailing = isSailing;

        public bool GetIsReadyToSail => _isReadyToSail;

        public void SetIsReadyToSail(bool isReadyTosail) => _isReadyToSail = isReadyTosail;

        public float GetSpeed => _speed;

        public Dock GetDockedAt => _dockedAt;

        public void SetDockedAt(Dock dockedAt) => _dockedAt = dockedAt;

        public int GetCurrentLocation => _currentLocation;

        public void SetCurrentLocation(int currentLocation) => _currentLocation = currentLocation;

        public bool GetHasReachedDestination => _hasReachedDestination;

        internal ShipSailingState GetSailingState => _sailingState; 

        internal void SetSailingState(ShipSailingState sailingState) => _sailingState = sailingState;

    }
}
