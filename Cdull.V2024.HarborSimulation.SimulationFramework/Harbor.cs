using System.Collections;
using System.Text;
using System.Xml.Linq;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    /// <summary>
    /// Represents a harbor in the simulation.
    /// </summary>
    public class Harbor
    {
        internal string Name { get; set; }
        internal DateTime CurrentTime { get; set; }
        internal int Location { get; set; }
        internal List<Dock> Docks { get; set; } = new List<Dock>();
        internal List<Ship> Ships { get; set; } = new List<Ship>();
        internal List<Ship> DockedShips { get; set; } = new List<Ship>();
        internal List<Ship> SailingShips { get; set; } = new List<Ship>();
        internal Queue<Ship> WaitingShips { get; set; } = new Queue<Ship>();


        public event EventHandler<ShipDepartureEventArgs> ShipDeparted;

        internal CargoStorage CargoStorage { get; set; }


        /// <summary>
        /// Initializes a new instance of the Harbor class.
        /// </summary>
        /// <param name="harborName">The name of the harbor.</param>
        /// <param name="harborCargoStorage">The cargo storage capacity for the harbor.</param>
        public Harbor(string harborName, CargoStorage harborCargoStorage)
        {
            Name = harborName;
            Location = 0; 
            CargoStorage = harborCargoStorage;
        }

        /// <summary>
        /// Creates docks in the harbor.
        /// </summary>
        /// <param name="numberOfDock">The number of docks to create.</param>
        /// <param name="dockModel">The model of the docks to create.</param>
        /// <param name="dockSize">The size of the docks to create.</param>
        /// <param name="numberOfCranes">The number of cranes to add to each dock.</param>
        /// <returns>A list of created docks.</returns>
        public List<Dock> InitializeDocks(int numberOfDock, Model dockModel, Size dockSize, int numberOfCranes)
        {
            List<Dock> docks = new List<Dock>();
            try
            {            
                if (numberOfDock <= 0)
                {
                   throw new ArgumentException("Number of docks should be greater than zero.");       
                }
                for (int i = 0; i < numberOfDock; i++)
                {
                    Dock dock = new($"{dockModel}dock-{i}", dockSize, dockModel);
                    docks.Add(dock);
                    
                    if (numberOfCranes > 0)
                    {
                        for (int z = 0; z < numberOfCranes; z++)
                        {
                            Crane crane = new($"Crane{z}");
                            dock.Cranes.Add(crane);
                         
                        }         
                    }           
                } 
            }
            catch (Exception e)
            {
                Console.WriteLine("Error initializing docks.", e);
            }
            return docks;
        }

        /// <summary>
        /// Creates ships for the simulation using Enum for ship size and type.
        /// </summary>
        /// <param name="numberOfShips">The number of ships to create.</param>
        /// <param name="shipModel">The model of the ships to create.</param>
        /// <param name="shipSize">The size of the ships to create.</param>
        /// <param name="numberOfCargo">The number of cargos on each ship.</param>
        /// <param name="CargoWeight">The weight of all cargo on each ship (default is 10).</param>
        /// <returns>A list of created ships.</returns>
        public List<Ship> InitializeShips(int numberOfShips, Model shipModel, Size shipSize, int numberOfCargo, int CargoWeight = 10)
        {
            List<Ship> ships = new List<Ship>();
            try
            { 
                if (numberOfShips <= 0)
                {
                    throw new ArgumentException("Number of ships should be greater than zero.");

                }
                for (int i = 0; i < numberOfShips; i++)
                {
                    Ship ship = new($"{shipModel}ship-{i}", shipModel, shipSize);

                    // Sjekk om skipet allerede finnes før du legger det til
                    if (!ships.Any(s => s.Name == ship.Name))
                    {
                        ships.Add(ship);
                        ship.InitializeCargo(numberOfCargo);
                    }
                }    
            }
            catch (Exception e)
            {
                Console.WriteLine("Error initializing ships.", e);
            }
            return ships;
        }

        /// <summary>
        /// Finds an available dock of the specified size.
        /// </summary>
        /// <param name="shipSize">The size of the ship seeking a dock.</param>
        /// <returns>
        /// The available dock of the specified size if found; otherwise, null.
        /// </returns>
        public Dock AvailableDockOfSize(Size shipSize)
        {
            if (Docks.Count == 0)
            {
                throw new ArgumentNullException(nameof(Dock), "Harbor cannot have Zero Dockes.");
            }
            try
            {
                foreach (Dock dock in Docks)
                {
                    if(shipSize < Size.Medium && dock.IsAvailable)
                    {
                        return dock; 
                    }

                    else if (shipSize.Equals(Size.Medium) && !dock.Size.Equals(Size.Small) && dock.IsAvailable)
                    {
                        return dock;
                    }

                    else if(shipSize.Equals(Size.Large) && dock.Size.Equals(Size.Large) && dock.IsAvailable)
                    {
                        return dock;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error finding available dock.", e);
            }
            return null;
        }


        /// <summary>
        /// Checks if a ship is already in the waiting queue.
        /// </summary>
        /// <param name="ship">The ship to check for in the queue.</param>
        /// <returns>
        /// True if the ship is found in the waiting queue; otherwise, false.
        /// </returns>
        private bool IsShipInQueue(Ship ship)
        {
            foreach(Ship otherShip in WaitingShips)
            {
                if(ship.Name == otherShip.Name)
                {
                    return true; 
                }
            }
            return false; 
        }

        /// <summary>
        /// Puts all ships in the simulation into the waiting queue for docking.
        /// </summary>
        /// <remarks>
        /// This method adds all ships that are not currently sailing, not already docked, 
        /// and not already in the waiting queue to the waiting queue for docking.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when the list of ships is empty.</exception>
     
        public void QueueShipsToDock()
        {
            if (Ships.Count == 0)
            {
                throw new ArgumentNullException(nameof(Ships), "Harbor cannot have Zero Ships.");
            }
            try
            {
                foreach (Ship ship in Ships)
                {
                    if (!ship.IsSailing && !ship.HasDocked && !IsShipInQueue(ship))
                    {
                        WaitingShips.Enqueue(ship);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error queuing ships to dock.", e);
            }
        }

        /// <summary>
        /// Attempts to dock ships waiting in the queue to available docks in the harbor.
        /// </summary>
        /// <remarks>
        /// This method iterates through the ships in the waiting queue and attempts to dock them to available docks in the harbor. 
        /// It checks if there is an available dock of the appropriate size for each ship and if the ship is not currently sailing.
        /// If a ship successfully docks, it is removed from the queue and added to the list of docked ships.
        /// If there are no available docks or the ship is currently sailing, it remains in the queue.
        /// </remarks>
        public void DockShips()
        {
            HistoryHandler historyHandler = HistoryHandler.GetInstance();
            while (WaitingShips.Count > 0)
            {
                Ship ship = WaitingShips.Peek();
                Dock availableDock = AvailableDockOfSize(ship.Size);

                if (availableDock is not null && ship.IsSailing == false)
                {
                    ship.SetDestinationLocationFrom(ship.CurrentLocation, Location); 
                    ship.Move();

                    if (ship.HasReachedDestination)
                    {
                        ship.HasDocked = true;
                        ship.DockedAt = availableDock;
                        availableDock.IsAvailable = false;
                        availableDock.OccupiedBy = ship;
                        historyHandler.AddEventToShipHistory(ship, $"{ship.Name} Docked at {CurrentTime} on {ship.DockedAt.Name}");
                        ship.DockedAtTime = CurrentTime.ToString();
                        DockedShips.Add(ship);
                        WaitingShips.Dequeue();
                    }  
                }
                else
                {
                    WaitingShips.Enqueue(WaitingShips.Dequeue());
                    break; 
                }
            }
         
     
        }


        /// <summary>
        /// Moves cargo <cargo cref="Cargo"></Cargo>from docked ships to the harbor's cargo storage.
        /// </summary>
        /// <remarks>
        /// This method iterates through all docked ships in the harbor and their respective cargo items.
        /// It attempts to add each cargo item to the harbor's cargo storage if there is available space.
        /// If the harbor's cargo storage is full, it marks the storage as unavailable by throwing an exception.
        /// </remarks>
        /// <exception cref="AddCargoToStorageException">Thrown when there is not enough space in CargoStorage to add all cargo from the ship.</exception>
        public void AddCargoToStorage()
        {
            foreach (Ship ship in DockedShips)
            {
                while (ship.Cargo.Any())
                {
                    Cargo cargo = ship.Cargo.First();

                    if (CargoStorage.Capacity > ship.Cargo.Count)
                    {
                        CargoStorage.AddCargo(cargo);
                        CargoStorage.OccupySpace(cargo);
                        ship.Cargo.Remove(cargo);
                    }
                    else
                    {
                        throw new AddCargoToStorageException("Not enough space in CargoStorage to add all cargo from the ship.");
                    }

                }

            }
        }

        /// <summary>
        /// Adds cargo from the harbor's storage to docked ships.
        /// </summary>
        /// <param name="numberOfCargo">The maximum number of cargo items to add to each ship.</param>
        /// <remarks>
        /// This method iterates through all docked ships and attempts to add cargo to each ship up to the specified limit.
        /// It removes cargo items from the harbor's storage and adds them to the ship's cargo list.
        /// The method also updates cargo history with loading information.
        /// </remarks>
        public void AddCargoToShips(int numberOfCargo)
        {
            try
            {
                foreach (Ship ship in DockedShips)
                {
                    if (ship.Cargo.Count <= numberOfCargo)
                    {
                        for (int i = 0; i < numberOfCargo; i++)
                        {
                            if (CargoStorage.Cargo.Count > 0)
                            {
                                Cargo cargo = CargoStorage.Cargo.First();
                                ship.Cargo.Add(cargo);
                                CargoStorage.RemoveCargo(cargo);
                                CargoStorage.deOccupySpace(cargo); 
                                cargo.History.Add($"{cargo.Name} loaded at {CurrentTime} on {ship.Name}");
                            }
                            else
                            {
                                break; 
                            }
                        }
                        ship.IsReadyToSail = true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Removes a ship from its dock and updates dock and ship properties accordingly.
        /// </summary>
        /// <param name="ship">The ship to be removed from its dock.</param>
        /// <returns>True if the ship was successfully removed from its dock; otherwise, false.</returns>
        /// <remarks>
        /// This method checks if the ship is docked at a valid dock. If the ship is docked, it removes the ship from the dock,
        /// updates the dock's occupied status, removes the ship from the list of docked ships, and resets ship properties
        /// related to docking. If the ship is not docked, it returns false.
        /// </remarks>
        public bool RemoveShipFromDock(Ship ship)
        {
            if (ship.DockedAt != null)
            {
                Dock dock = ship.DockedAt;
                dock.OccupiedBy = null;
                dock.IsAvailable = true;
                DockedShips.Remove(ship);
                ship.HasDocked = false;
                ship.DockedAt = null;
                return true;
            }
            else
            {
                return false;
            }
        }

     
        internal void RaiseShipDeparted(Ship departedShip)
        {
            ShipDeparted?.Invoke(this, new ShipDepartureEventArgs(departedShip));
        }


        /// <summary>
        /// Gets the list of docks in the harbor.
        /// </summary>
        /// <returns>The list of docks.</returns>
        public List<Dock> GetDocks()
        {
            return Docks;
        }

        /// <summary>
        /// Gets the list of ships in the harbor.
        /// </summary>
        /// <returns>The list of ships.</returns>
        public List<Ship> GetShips()
        {
            return Ships;
        }

        /// <summary>
        /// Gets the list of ships currently docked in the harbor.
        /// </summary>
        /// <returns>The list of docked ships.</returns>
        public List<Ship> GetDockedShips()
        {
            return DockedShips;
        }

        /// <summary>
        /// Gets the list of ships currently sailing from the harbor.
        /// </summary>
        /// <returns>The list of sailing ships.</returns>
        public List<Ship> GetSailingShips()
        {
            return SailingShips;
        }

        /// <summary>
        /// Gets the queue of ships waiting to dock in the harbor.
        /// </summary>
        /// <returns>The queue of waiting ships.</returns>
        public Queue<Ship> GetWaitingShips()
        {
            return WaitingShips;
        }

        /// <summary>
        /// Retrieves the current time of the harbor simulation.
        /// </summary>
        /// <returns>The current time of the harbor simulation.</returns>
        public DateTime GetCurrentTime()
        {
            return CurrentTime;
        }

        /// <summary>
        /// Sets the current time of the harbor simulation to the specified value.
        /// </summary>
        /// <param name="currentTime">The new current time to set for the harbor simulation.</param>
        public void SetCurrentTime(DateTime currentTime)
        {
            CurrentTime = currentTime;
        }

        /// <summary>
        /// Returns a string representation of the harbor simulation.
        /// </summary>
        /// <returns>A string containing information about the harbor simulation, including harbor name, current time, ship counts, and cargo storage details.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Harbor Name: {Name}" + $" CurrentTime: {CurrentTime}");
            stringBuilder.AppendLine($"Total Ships: {Ships.Count}");
            stringBuilder.AppendLine($"Docked Ships: {DockedShips.Count}");
            stringBuilder.AppendLine($"Sailing Ships: {SailingShips.Count}");
            stringBuilder.AppendLine($"Waiting Ships: {WaitingShips.Count}");
            stringBuilder.AppendLine($"Cargo Storage: Capacity: {CargoStorage.Capacity}, Occupied Space: {CargoStorage.GetOccupiedSpace()}\n");
            return stringBuilder.ToString();
        }

    }


}


