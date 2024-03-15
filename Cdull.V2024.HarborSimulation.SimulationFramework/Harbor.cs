﻿using System.Collections;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;
using Cdull.V2024.HarborSimulation.SimulationFramework.Enums;
using Cdull.V2024.HarborSimulation.SimulationFramework.Events;
using Cdull.V2024.HarborSimulation.SimulationFramework.Exceptions;
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
        /// Event raised when a ship completes loading at the harbor.
        /// </summary>
        public event EventHandler<ShipLoadingEventArgs> CompletedloadingShip;



        internal CargoStorage CargoStorage { get; set; }


        /// <summary>
        /// Initializes a new instance of the Harbor class with the specified name and cargo storage.
        /// </summary>
        /// <param name="harborName">The name of the harbor.</param>
        /// <param name="harborCargoStorage">The cargo storage capacity for the harbor.</param>
        /// <remarks>
        /// This constructor initializes a harbor with the given name and cargo storage.
        /// It sets the harbor's location to the default value of 0.
        /// </remarks>

        public Harbor(string harborName, CargoStorage harborCargoStorage)
        {
            Name = harborName;
            Location = 0; 
            CargoStorage = harborCargoStorage;
        }

        /// <summary>
        /// Initializes a list of docks for the harbor simulation with the specified parameters.
        /// </summary>
        /// <param name="numberOfDock">The number of docks to create.</param>
        /// <param name="dockModel">The model of the docks to create.</param>
        /// <param name="dockSize">The size of the docks to create.</param>
        /// <param name="numberOfCranes">The number of cranes to add to each dock.</param>
        /// <returns>A list of created docks.</returns>
        /// <remarks>
        /// This method creates a list of docks based on the specified parameters.
        /// It ensures that the number of docks is greater than zero and creates each dock with a unique name.
        /// If the number of cranes is greater than zero, it adds the specified number of cranes to each dock.
        /// </remarks>
        public List<Dock> InitializeDocks(int numberOfDock, Model dockModel, Size dockSize, int numberOfCranes)
        {
            List<Dock> docks = new List<Dock>();

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

            return docks;
        }


        /// <summary>
        /// Initializes a list of ships for the simulation with the specified parameters.
        /// </summary>
        /// <param name="numberOfShips">The number of ships to create.</param>
        /// <param name="shipModel">The model of the ships to create.</param>
        /// <param name="shipSize">The size of the ships to create.</param>
        /// <param name="numberOfCargo">The number of cargos on each ship.</param>
        /// <param name="CargoWeight">The weight of all cargo on each ship (default is 10).</param>
        /// <returns>A list of created ships.</returns>
        /// <remarks>
        /// This method creates a list of ships based on the specified parameters.
        /// It ensures that the number of ships is greater than zero and creates each ship with a unique name.
        /// Each ship is initialized with the specified number of cargo items and cargo weight.
        /// </remarks>
        public List<Ship> InitializeShips(int numberOfShips, Model shipModel, Size shipSize, int numberOfCargo, int CargoWeight = 10)
        {
            List<Ship> ships = new List<Ship>();

            if (numberOfShips <= 0)
            {
                    throw new ArgumentException("Number of ships should be greater than zero.");
            }
            for (int i = 0; i < numberOfShips; i++)
            {
                Ship ship = new($"{shipModel}ship-{i}", shipModel, shipSize);  
                if (!ships.Any(s => s.Name == ship.Name))
                {
                    ships.Add(ship);
                    ship.InitializeCargo(numberOfCargo);
                }   
            }
            return ships;
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
            if (Docks.Count == 0)
            {
                throw new ArgumentNullException(nameof(Dock), "Harbor cannot have Zero Docks.");
            }

            foreach (Dock dock in Docks)
            {
                if (shipSize < Size.Medium && dock.IsAvailable)
                {
                    return dock;
                }
                else if (shipSize.Equals(Size.Medium) && !dock.Size.Equals(Size.Small) && dock.IsAvailable)
                {
                    return dock;
                }
                else if (shipSize.Equals(Size.Large) && dock.Size.Equals(Size.Large) && dock.IsAvailable)
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
            if (Ships.Count == 0)
            {
                throw new ArgumentNullException(nameof(Ships), "Harbor cannot have Zero Ships.");
            }
            
            foreach (Ship ship in Ships)
            {
                if (!ship.IsSailing && !ship.HasDocked && !IsShipInQueue(ship))
                {   
                    WaitingShips.Enqueue(ship);
                    RaiseShipArrived(ship);
                    return true; 
                }
            }
            return false; 
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
        /// <exception cref="InvalidOperationException">Thrown when there are no waiting ships in the queue.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the ship's docked status cannot be updated.</exception>
        /// <exception cref="InvalidOperationException">Thrown when an event cannot be added to the ship's history.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the ship cannot be added to the list of docked ships.</exception>
        /// <exception cref="InvalidOperationException">Thrown when a ship cannot be dequeued from the waiting queue.</exception>
        internal void DockShips()
        {
            HistoryHandler historyHandler = HistoryHandler.GetInstance();

            if (!WaitingShips.Any())
            {
                throw new InvalidOperationException("No waiting ships in the queue.");
            }

            while (WaitingShips.Any())
            {
                Ship ship = WaitingShips.Peek();
                Dock availableDock = AvailableDockOfSize(ship.Size);

                if (availableDock is not null && !ship.IsSailing)
                {
                    ship.SetDestinationLocationFrom(ship.CurrentLocation, Location);
                    ship.CalculateMovement(this);

                    if (ship.HasReachedDestination)
                    {
                        ship.HasDocked = true;
                        ship.DockedAt = availableDock;
                        availableDock.IsAvailable = false;
                        availableDock.OccupiedBy = ship;
                        ship.DockedAtTime = CurrentTime.ToString();

                        try
                        {
                            historyHandler.AddEventToShipHistory(ship, $"{ship.Name} Docked at {ship.DockedAtTime} on {ship.DockedAt.Name} ship cargo: {ship.Cargo.Count}");
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException("Failed to add event to ship's history.", ex);
                        }

                        DockedShips.Add(ship);

                        try
                        {
                            WaitingShips.Dequeue();
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException("Failed to dequeue ship from waiting ships queue.", ex);
                        }
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
            if (ship.DockedAt != null)
            {
                if (ship == null)
                {
                    throw new ArgumentNullException(nameof(ship), "Ship parameter cannot be null.");
                }

                if (ship.DockedAt == null)
                {
                    throw new InvalidOperationException("The ship is not currently docked.");
                }

                Dock dock = ship.DockedAt;
                if (dock == null)
                {
                    throw new InvalidOperationException("The ship's dock reference is null.");
                }
                dock.OccupiedBy = null;
                dock.IsAvailable = true;
                bool removed = DockedShips.Remove(ship);

                if (!removed)
                {
                    throw new InvalidOperationException("Failed to remove ship from docked ships list.");
                }
                ship.HasDocked = false;
                ship.DockedAt = null;
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
        /// <param name="completedLoadingShip">The ship that has completed loading.</param>
        internal void RaiseShipCompletedLoading(Ship completedLoadingShip)
        {
            CompletedloadingShip?.Invoke(this, new ShipLoadingEventArgs(completedLoadingShip));
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


