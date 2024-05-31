
using Cdull.V2024.HarborSimulation.SimulationFramework.ContainerOperations;
using Cdull.V2024.HarborSimulation.SimulationFramework.ShipOperations;
using System.Text;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure
{
    /// <summary>
    /// Represents a harbor in the simulation.
    /// </summary>
    public class Harbor
    {
        private readonly string _name;

        /// <summary>
        /// Represents the maximum number of ships per day that each dock in the harbor can accommodate.
        /// </summary>
        /// <remarks>
        /// This value determines the maximum capacity of each dock in terms of the number of ships it can handle per day.
        /// It influences the scheduling and allocation of ships to docks within the harbor simulation.
        /// </remarks>

        private int _maxNumberOfShipsPerDay; 

        private DateTime _currentTime;

        private readonly Range _locationRange;

        private List<Dock> _docks = new List<Dock>();

        private List<Ship> _ships = new List<Ship>();

        private List<Ship> _dockedShips = new List<Ship>();

        private List<Ship> _sailingShips = new List<Ship>();

        private Queue<Ship> _waitingShips = new Queue<Ship>();

        private List<AGV> _agvs = new List<AGV>();

        private ContainerStorage _containerStorage;


        /// <summary>
        /// Event raised when a ship departs from the harbor.
        /// </summary>
        public event EventHandler<ShipDepartureEventArgs> DepartedShip;
        /// <summary>
        /// Event raised when a ship arrives at the harbor.
        /// </summary>
        public event EventHandler<ShipArrivalEventArgs> ArrivedShip;
        /// <summary>
        /// Event raised when a ship completes unloading at the harbor.
        /// </summary>
        public event EventHandler<ShipUnloadingEventArgs> CompletedUnloadingShip;
        /// <summary>
        /// Event raised when a ship completes Loading at the harbor.
        /// </summary>
        public event EventHandler<ShipLoadingEventArgs> CompletedloadingShip;


        /// <summary>
        /// Initializes a new instance of the Harbor class with the specified name, location range, cargo storage, and maximum number of ships per day.
        /// </summary>
        /// <param name="harborName">The name of the harbor.</param>
        /// <param name="locationRange">The range of locations for the harbor, starting from 0.</param>
        /// <param name="harborContainerStorage">The cargo storage capacity for the harbor.</param>
        /// <param name="harborMaxNumberOfShipsPerDay">The maximum number of ships that each dock in the harbor can handle per day.</param>
        /// <example>
        /// This example demonstrates how to create a Harbor instance:
        /// <code>
        /// class Program
        /// {
        ///     static void Main(string[] args)
        ///     {
        ///         // Create a ContainerStorage instance for the harbor
        ///         ContainerStorage harborContainerStorage = new ContainerStorage(1000);
        ///   
        ///         // Create a new instance of Harbor with the specified name, location range, cargo storage, and maximum number of ships per day
        ///         Harbor harbor = new Harbor("Oslo Harbor", 1000, harborContainerStorage, 10);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Harbor(string harborName, int locationRange, ContainerStorage harborContainerStorage, int harborMaxNumberOfShipsPerDay)
        {
            _name = harborName;
            _locationRange = new Range(0, locationRange);
            _containerStorage = harborContainerStorage;
            _maxNumberOfShipsPerDay = harborMaxNumberOfShipsPerDay; 
        }

        /// <summary>
        /// Initializes a list of docks for the harbor simulation with the specified parameters.
        /// </summary>
        /// <param name="numberOfDock">The number of docks to create.</param>
        /// <param name="dockSize">The size of the docks to create.</param>
        /// <param name="numberOfCranes">The number of cranes to add to each dock.</param>
        /// <returns>A list of created docks.</returns>
        /// <remarks>
        /// This method creates a list of docks based on the specified parameters.
        /// It ensures that the number of docks is greater than zero and creates each dock with a unique name.
        /// If the number of cranes is greater than zero, it adds the specified number of cranes to each dock.
        /// </remarks>
        /// <example>
        /// This example shows how to use the InitializeDocks method to create a list of docks.
        /// <code>
        ///     List<Dock> docks = harbor.InitializeDocks(20, Size.Large, 2);
        /// </code>
        /// </example>
        public List<Dock> InitializeDocks(int numberOfDock, Size dockSize, int numberOfCranes)
        {
            List<Dock> docks = new List<Dock>();

            if (numberOfDock <= 0)
            {
                throw new ArgumentException("Number of docks should be greater than zero.");
            }

            for (int i = 0; i < numberOfDock; i++)
            {
                Dock dock = new(i, dockSize);
                docks.Add(dock);

                if (numberOfCranes > 0)
                {
                    for (int z = 0; z < numberOfCranes; z++)
                    {
                        Crane crane = new(z);
                        dock.GetCranes.Add(crane);
                    }
                }
            }

            return docks;
        }


        /// <summary>
        /// Initializes a list of ships based on the provided parameters.
        /// </summary>
        /// <param name="shipCurrentLocation">The current location of the ships.</param>
        /// <param name="numberOfShips">The number of ships to initialize.</param>
        /// <param name="shipModel">The model of the ships to initialize.</param>
        /// <param name="shipSize">The size of the ships to initialize. Default is set to Size.Small.</param>
        /// <param name="numberOfContainers">The number of container units to initialize for each ship. Default is set to 0.</param>
        /// <param name="containerSize">The size of each cargo unit. Default is set to ContainerSize.Large.</param>
        /// <returns>A list of initialized Ship objects.</returns>
        /// <remarks>
        /// This method initializes a list of ships based on the provided parameters.
        /// If the ship model is a ContainerShip, it initializes the specified number of cargo units for each ship.
        /// The ship size and number of cargo units parameters are optional, with default values assigned if not provided.
        /// </remarks>
        /// <example>
        /// This example shows how to use the InitializeShips method to create a list of ships.
        /// <code> 
        ///     List<Ship> ships = harbor.InitializeShips(2000, 5, _model.LNGCarrier, Size.Medium);
        /// </code>
        /// </example>

        public List<Ship> InitializeShips(int shipCurrentLocation, int numberOfShips, Model shipModel, Size shipSize = Size.Small,
            int numberOfContainers = 0, ContainerSize containerSize = ContainerSize.Large)
        {
            List<Ship> ships = new List<Ship>();

            if (numberOfShips <= 0)
            {
                throw new ArgumentException("Number of ships should be greater than zero.");
            }
            for (int i = 0; i < numberOfShips; i++)
            {
                Ship ship = new($"{shipModel}ship-{i}", shipModel, shipSize, shipCurrentLocation);
                if (!ships.Any(s => s.GetName == ship.GetName))
                {
                    ships.Add(ship);

                    if (shipModel == Model.ContainerShip)
                    {
                        ship.InitializeContainers(numberOfContainers, containerSize);
                    }
                }
            }
            return ships;
        }


        /// <summary>
        /// Initializes a specified number of Automated Guided Vehicles (AGVs) at a given location.
        /// </summary>
        /// <param name="numberOfAGV">The number of AGVs to initialize.</param>
        /// <param name="agvLocation">The location where the AGVs should be initialized.</param>
        /// <returns>A list of initialized AGVs.</returns>
        /// <exception cref="ArgumentException">Thrown when the number of AGVs is less than or equal to zero.</exception>
        /// <example>
        /// This example shows how to use the InitializeAGVs method to create a list of AGVs.
        /// <code>
        ///     List<AGV> agvs = harbor.InitializeAGVs(20, 1000);
        /// </code>
        /// </example> 
        public List<AGV> InitializeAGVs(int numberOfAGV, int agvLocation)
        {
            List<AGV> agvs = new List<AGV>();

            if (numberOfAGV <= 0)
            {
                throw new ArgumentException("Number of AGVs should be greater than zero.");
            }

            for (int i = 0; i < numberOfAGV; i++)
            {
                AGV agv = new(agvLocation);
                agvs.Add(agv);
            }

            return agvs;
        }

        /// <summary>
        /// Initializes the storage columns in the harbor.
        /// </summary>
        /// <param name="longColumnLocations">The locations for long columns.</param>
        /// <param name="shortColumnLocations">The locations for short columns.</param>
        /// <param name="longColumnLength">The length of long columns.</param>
        /// <param name="shortColumnLength">The length of short columns.</param>
        /// <param name="numberOfLongColumns">The number of long columns at each location.</param>
        /// <param name="numberOfShortColumns">The number of short columns at each location.</param>
        /// <param name="columnWidth">The width of the columns.</param>
        /// <param name="columnHeight">The height of the columns.</param>
        /// <returns>A list of initialized storage columns.</returns>
        /// <exception cref="ArgumentException">Thrown when parameters are invalid.</exception>
        /// <example>
        /// This example shows how to use the InitializeStorageColumns method to create a list of storage columns.
        /// <code>
        ///     List<int> longColumnLocations = new List<int> { 37, 111, 185, 259, 333, 407 };
        ///     List<int> shortColumnLocations = new List<int> { 30, 74, 148, 222, 292, 270, 444 };
        ///     int longColumnLength = 18;
        ///     int shortColumnLength = 15;
        ///     int numberOfLongColumns = 4;
        ///     int numberOfShortColumns = 1;
        ///     int columnWidth = 6;
        ///     int columnHeight = 4;
        ///     
        ///     List<StorageColumn> storageColumns = harbor.InitializeStorageColumns(
        ///         longColumnLocations, shortColumnLocations, longColumnLength, shortColumnLength, 
        ///         numberOfLongColumns, numberOfShortColumns, columnWidth, columnHeight);
        /// </code>
        /// </example>
        public List<StorageColumn> InitializeStorageColumns(List<int> longColumnLocations, List<int> shortColumnLocations, int longColumnLength,
            int shortColumnLength, int numberOfLongColumns, int numberOfShortColumns, int columnWidth, int columnHeight)
        {
            if (longColumnLocations == null || longColumnLocations.Count == 0 ||
                shortColumnLocations == null || shortColumnLocations.Count == 0 ||
                numberOfLongColumns <= 0 || numberOfShortColumns <= 0)
            {
                throw new ArgumentException("Invalid parameters. Locations and numbers of columns should be provided.");
            }

            List<StorageColumn> columns = new List<StorageColumn>();

            AddColumns(columns, longColumnLocations, longColumnLength, numberOfLongColumns, columnWidth, columnHeight);
            AddColumns(columns, shortColumnLocations, shortColumnLength, numberOfShortColumns, columnWidth, columnHeight);

            return columns;
        }




        private void AddColumns(List<StorageColumn> columns, List<int> locations, int columnLength, int numberOfColumns, int columnWidth, int columnHeight)
        {
            foreach (int location in locations)
            {
                for (int i = 0; i < numberOfColumns; i++)
                {
                    StorageColumn column = new StorageColumn(location, i, columnLength, columnWidth, columnHeight);
                    columns.Add(column);
                }
            }
        }

        /// <summary>
        /// Gets an available AGV from the list of AGVs.
        /// </summary>
        /// <returns>The available AGV.</returns>
        internal AGV GetAvailableAGV()
        {
            foreach (AGV agv in _agvs)
            {
                if (agv.GetIsAvailable)
                {
                    return agv;
                }
            }
            return null;

        }

        /// <summary>
        /// Finds an available dock of the specified size for docking a ship.
        /// </summary>
        /// <param name="shipSize">The size of the ship seeking a dock.</param>
        /// <returns>
        /// The available dock of the specified size if found; otherwise, null.
        /// </returns>
        /// <remarks>
        /// This method iterates through the docks in the harbor to find a dock that matches the specified size criteria.
        /// It considers the availability of the dock and whether it can accommodate ships of the specified size.
        /// If a suitable dock is found, it is returned; otherwise, null is returned.
        /// </remarks>
        internal Dock AvailableDockOfSize(Size shipSize)
        {
            if (_docks.Count == 0)
            {
                throw new ArgumentNullException(nameof(Dock), "Harbor cannot have Zero Docks.");
            }

            foreach (Dock dock in _docks)
            {
                if (shipSize < Size.Medium && dock.GetIsAvailable)
                {
                    return dock;
                }
                else if (shipSize.Equals(Size.Medium) && !dock.GetSize.Equals(Size.Small) && dock.GetIsAvailable)
                {
                    return dock;
                }
                else if (shipSize.Equals(Size.Large) && dock.GetSize.Equals(Size.Large) && dock.GetIsAvailable)
                {
                    return dock;
                }
            }

            return null;
        }


        /// <summary>
        /// Checks if a ship is already in the waiting queue for docking.
        /// </summary>
        /// <param name="ship">The ship to check for in the waiting queue.</param>
        /// <returns>True if the ship is found in the waiting queue; otherwise, false.</returns>
        /// <remarks>
        /// This method iterates through all ships in the waiting queue and compares their names with the name of the provided ship.
        /// If a ship with the same name is found in the waiting queue, the method returns true; otherwise, it returns false.
        /// </remarks>
        internal bool IsShipInQueue(Ship ship)
        {
            foreach (Ship otherShip in _waitingShips)
            {
                if (ship.GetName == otherShip.GetName)
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// Puts ships in the simulation into the waiting queue for docking if they are not currently sailing, docked, or already in the queue.
        /// </summary>
        /// <returns>True if at least one ship is added to the waiting queue; otherwise, false.</returns>
        /// <remarks>
        /// This method checks if the harbor has any ships. If there are no ships, it throws an exception.
        /// It then iterates through all ships in the harbor. For each ship, it checks if the ship is not currently sailing,
        /// not already docked, and not already in the waiting queue. If these conditions are met, the ship is added to the waiting queue,
        /// and the ship arrived event is raised. The method returns true if at least one ship is added to the waiting queue; otherwise, false.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when the list of ships is empty.</exception>
        internal bool QueueShipsToDock()
        {
            if (_ships.Count == 0)
            {
                throw new ArgumentNullException(nameof(_ships), "Harbor cannot have Zero Ships.");
            }

            foreach (Ship ship in _ships)
            {
                if (!ship.GetIsSailing && !ship.GetHasDocked && !IsShipInQueue(ship))
                {
                    _waitingShips.Enqueue(ship);
                    RaiseShipArrived(ship);
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// Removes the specified ship from its dock.
        /// </summary>
        /// <param name="ship">The ship to be removed from its dock.</param>
        /// <returns>True if the ship was successfully removed from its dock; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the ship parameter is null.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the ship is not currently docked,
        /// or when the ship's dock reference is null,
        /// or when the ship fails to be removed from the list of docked ships.
        /// </exception>
        internal bool RemoveShipFromDock(Ship ship)
        {
            if (ship.GetDockedAt != null)
            {
                if (ship == null)
                {
                    throw new ArgumentNullException(nameof(ship), "Ship parameter cannot be null.");
                }

                if (ship.GetDockedAt == null)
                {
                    throw new InvalidOperationException("The ship is not currently docked.");
                }

                Dock dock = ship.GetDockedAt;
                if (dock == null)
                {
                    throw new InvalidOperationException("The ship's dock reference is null.");
                }
                dock.SetOccpuiedBy(null);
                dock.SetIsAvailable(true);
                bool removed = _dockedShips.Remove(ship);

                if (!removed)
                {
                    throw new InvalidOperationException("Failed to remove ship from docked ships list.");
                }
                ship.SetHasDocked(false);
                ship.SetDockedAt(null);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Raises the DepartedShip event.
        /// </summary>
        /// <param name="departedShip">The ship that has departed.</param>
        internal void RaiseShipDeparted(Ship departedShip)
        {
            DepartedShip?.Invoke(this, new ShipDepartureEventArgs(departedShip));
        }

        /// <summary>
        /// Raises the ArrivedShip event.
        /// </summary>
        /// <param name="arrivedShip">The ship that has arrived.</param>
        internal void RaiseShipArrived(Ship arrivedShip)
        {
            ArrivedShip?.Invoke(this, new ShipArrivalEventArgs(arrivedShip));
        }

        /// <summary>
        /// Raises the CompletedUnloadingShip event.
        /// </summary>
        /// <param name="completedUnloadingShip">The ship that has completed unloading.</param>
        internal void RaiseShipCompletedUnloading(Ship completedUnloadingShip)
        {
            CompletedUnloadingShip?.Invoke(this, new ShipUnloadingEventArgs(completedUnloadingShip));
        }

        /// <summary>
        /// Raises the CompletedLoadingShip event.
        /// </summary>
        /// <param name="completedLoadingShip">The ship that has completed Loading.</param>
        internal void RaiseShipCompletedLoading(Ship completedLoadingShip)
        {
            CompletedloadingShip?.Invoke(this, new ShipLoadingEventArgs(completedLoadingShip));
        }


        /// <summary>
        /// Gets the list of docks in the harbor.
        /// </summary>
        /// <returns>The list of docks.</returns>
        public List<Dock> GetDocks => _docks;


        /// <summary>
        /// Gets the range of locations within the harbor.
        /// </summary>
        /// <returns>The range of locations within the harbor.</returns>
        public Range GetLocationRange => _locationRange;


        /// <summary>
        /// Gets the list of ships in the harbor.
        /// </summary>
        /// <returns>The list of ships.</returns>
        public List<Ship> GetShips => _ships;

        /// <summary>
        /// Gets the list of ships currently docked in the harbor.
        /// </summary>
        /// <returns>The list of docked ships.</returns>
        public List<Ship> GetDockedShips => _dockedShips;

        /// <summary>
        /// Gets the list of ships currently sailing from the harbor.
        /// </summary>
        /// <returns>The list of sailing ships.</returns>
        public List<Ship> GetSailingShips => _sailingShips; 
        

        /// <summary>
        /// Gets the queue of ships waiting to dock in the harbor.
        /// </summary>
        /// <returns>The queue of waiting ships.</returns>
        public Queue<Ship> GetWaitingShips => _waitingShips;

      

        /// <summary>
        /// Retrieves the current time of the harbor simulation.
        /// </summary>
        /// <returns>The current time of the harbor simulation.</returns>
        public DateTime GetCurrentTime => _currentTime; 
       

        /// <summary>
        /// Sets the current time of the harbor simulation to the specified value.
        /// </summary>
        /// <param name="currentTime">The new current time to set for the harbor simulation.</param>
        public void SetCurrentTime(DateTime currentTime) => _currentTime = currentTime;


        /// <summary>
        /// Retrieves the maximum number of ships that each dock in the harbor can handle per day.
        /// </summary>
        /// <returns>The maximum number of ships per day.</returns>
        public int GetMaxNumberOfShipsPerDay => _maxNumberOfShipsPerDay;


        /// <summary>
        /// Retrieves the container storage capacity for the harbor.
        /// </summary>
        /// <returns>The container storage capacity for the harbor.</returns>
        public ContainerStorage GetContainerStorage => _containerStorage;


        /// <summary>
        /// Retrieves the list of Automated Guided Vehicles (AGVs) used in the harbor.
        /// </summary>
        /// <returns>The list of AGVs used in the harbor.</returns>
        public List<AGV> GetAGVs => _agvs; 

        /// <summary>
        /// Returns a string representation of the harbor simulation.
        /// </summary>
        /// <returns>A string containing information about the harbor simulation, including harbor name, current time, ship counts, and cargo storage details.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Harbor _Id: {_name}" + $" CurrentTime: {_currentTime}");
            stringBuilder.AppendLine($"Total Ships: {_ships.Count}");
            stringBuilder.AppendLine($"Docked Ships: {_dockedShips.Count}");
            stringBuilder.AppendLine($"Sailing Ships: {_sailingShips.Count}");
            stringBuilder.AppendLine($"Waiting Ships: {_waitingShips.Count}");
            stringBuilder.AppendLine($"Containers Storage: Capacity: {_containerStorage.GetCapacity}, Occupied Space: {_containerStorage.GetOccupiedSpace()}\n");
            return stringBuilder.ToString();
        }

    }


}


