using System.Collections;
using System.Text;
using System.Xml.Linq;
using static Cdull.V2024.HarborSimulation.SimulationFramework.Enums;

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
        internal Dictionary<DateTime, Harbor> HarborHistory { get; set; } = new Dictionary<DateTime, Harbor>();
        internal CargoStorage harborCargoStorage { get; set; }


        /// <summary>
        /// Initializes a new instance of the Harbor class.
        /// </summary>
        /// <param name="harborName">The name of the harbor.</param>
        /// <param name="harborCargoStorage">The cargo storage capacity for the harbor.</param>
        public Harbor(string harborName, CargoStorage harborCargoStorage)
        {
            Name = harborName;
            Location = 0; 
            this.harborCargoStorage = harborCargoStorage;
        }

        /// <summary>
        /// Creates a deep clone of the harbor, including all its properties and nested objects.
        /// </summary>
        /// <returns>A deep clone of the harbor.</returns>
        //(Manole, 2021) (https://paulsebastian.codes/a-solution-to-deep-cloning-in-csharp)
        public Harbor DeepClone()
        {
            Harbor clonedHarbor = new Harbor(Name, harborCargoStorage);

            clonedHarbor.CurrentTime = CurrentTime;
            clonedHarbor.Location = Location;
          
            clonedHarbor.Docks.AddRange(Docks.Select(dock => new Dock(dock.Name, dock.Size, dock.Model)));
            clonedHarbor.Ships.AddRange(Ships.Select(ship => new Ship(ship.Name, ship.Model, ship.Size)));
            clonedHarbor.DockedShips.AddRange(DockedShips.Select(ship => new Ship(ship.Name, ship.Model, ship.Size)));
            clonedHarbor.SailingShips.AddRange(SailingShips.Select(ship => new Ship(ship.Name, ship.Model, ship.Size)));
            clonedHarbor.WaitingShips = new Queue<Ship>(WaitingShips);
         
            foreach (var entry in HarborHistory)
            {
                clonedHarbor.HarborHistory[entry.Key] = entry.Value.DeepClone();
            }

            return clonedHarbor;
        }

        /// <summary>
        /// Saves the current state of the harbor to its history at the specified timestamp.
        /// </summary>
        /// <param name="date">The date which represents the moment when the state of harbor was saved.</param>
        //(Microsoft, 2022) https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2?view=net-8.0
        public void SaveHarborHistory(DateTime date)
        {
            HarborHistory[date] = DeepClone();
        }

        /// <summary>
        /// Retrieves the harbor state history for the specified date.
        /// </summary>
        /// <param name="fromDate">The date for which to retrieve the harbor state history.</param>
        /// <returns>The harbor state at the specified date.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the harbor history for the specified date is not found.</exception>
        //(Microsoft, 2022) https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2?view=net-8.0
        public Harbor GetHarborHistory(DateTime fromDate)
        {
            if (HarborHistory.ContainsKey(fromDate.Date))
            {
                return HarborHistory[fromDate.Date].DeepClone();
            }
            else
            {
                throw new KeyNotFoundException("Harbor history not found for the given date.");
            }
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
                    ship.InitializeCargo(numberOfCargo);
                    ships.Add(ship);
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
                    if (dock.Size.Equals(shipSize) && dock.IsAvailable)
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
        /// Docks the ships in the harbor.
        /// </summary>
        /// <remarks>
        /// This method docks all ships that have an available dock of appropriate size. It checks the size of the available docks,
        /// and then docks the ship if conditions are met.
        /// </remarks>
        public void DockShips()
        {
            try
            {
                while (WaitingShips.Count > 0)
                {
                    Ship ship = WaitingShips.Peek();
                    Dock availableDock = AvailableDockOfSize(ship.Size);

                    if (availableDock is not null && ship.IsSailing == false)
                    { 
                        ship.Move();

                        if (ship.HasReachedDestination)
                        {
                            ship.HasDocked = true;
                            ship.DockedAt = availableDock;
                            availableDock.IsAvailable = false;
                            availableDock.OccupiedBy = ship;
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
            catch (Exception e)
            {
                Console.WriteLine("Error docking ships.", e);
            }
        }


        /// <summary>
        /// Moves cargo from docked ships to the harbor's cargo storage.
        /// </summary>
        /// <remarks>
        /// This method iterates through all docked ships and their cargo. It attempts to add each cargo item to the harbor's cargo storage if there is available space.
        /// If the harbor's cargo storage is full, it marks the storage as unavailable.
        /// </remarks>
        public void AddCargoToStorage()
        {
            try
            {
                foreach (Ship ship in DockedShips.ToList())
                {
                    foreach (Cargo cargo in ship.Cargo.ToList())
                    {
                        if(harborCargoStorage.GetOccupiedSpace() > 0)
                        {
                            harborCargoStorage.AddCargo(cargo);
                            ship.Cargo.Remove(cargo);
                        }
                        else
                        {
                            harborCargoStorage.IsAvailable = false; 
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error adding cargo to storage.", e);
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
                            if (harborCargoStorage.Cargo.Count > 0)
                            {
                                Cargo cargo = harborCargoStorage.Cargo.First();
                                ship.Cargo.Add(cargo);
                                harborCargoStorage.RemoveCargo(cargo);
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

        /// <summary>
        /// Initiates the sailing process for ships.
        /// </summary>
        /// <param name="sailingStartTime">The starting time for sailing.</param>
        /// <param name="numberOfDays">The duration of sailing in days.</param>
        /// <remarks>
        /// This method iterates through all ships in the harbor. If a ship is ready to sail and the current time matches
        /// the specified sailing start time, the ship is removed from its dock and marked as sailing. If the current time
        /// matches the end of the sailing period, the ship is marked as not sailing, and it's queued to dock again.
        /// If the ship is not ready to sail or the current time is between the start and end of the sailing period,
        /// the ship is marked as waiting for sailing.
        /// </remarks>
        public void Sailing(DateTime sailingStartTime, int numberOfDays)
        {
            try
            {
                foreach (Ship ship in Ships)
                {
                    DateTime sailTimeIsOver = sailingStartTime.AddDays(numberOfDays);
                    if (ship.IsReadyToSail && CurrentTime.CompareTo(sailingStartTime) == 0 && !ship.IsSailing)
                    {
                        if (RemoveShipFromDock(ship))
                        {
                            ship.SailedAtTime = CurrentTime.ToString();
                            ship.IsSailing = true;
                            SailingShips.Add(ship);
                        }
                    }
                    else if (CurrentTime.CompareTo(sailTimeIsOver) == 0)
                    {
                        ship.IsSailing = false;
                        SailingShips.Remove(ship);
                        QueueShipsToDock();
                    }
                    else
                    {
                        ship.IsWaitingForSailing = true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in Sailing method: " + e.Message);
            }
        }

        /// <summary>
        /// Initiates recurring sailing operations based on the specified schedule.
        /// </summary>
        /// <param name="startSailing">The starting time for the first sailing operation.</param>
        /// <param name="endTime">The end time for the recurring sailing operations.</param>
        /// <param name="numberOfDaysAtSailing">The duration of sailing in days for each operation.</param>
        /// <param name="dailyOrWeekly">Specifies whether the sailing should occur daily or weekly.</param>
        /// <remarks>
        /// This method schedules recurring sailing operations based on the specified parameters.
        /// If <paramref name="dailyOrWeekly"/> is set to Enums.RecurringType.Daily/>,
        /// the method checks if the current time matches the <paramref name="startSailing"/> time and initiates sailing accordingly.
        /// If <paramref name="dailyOrWeekly"/> is set to Enums.RecurringType.Weekly/>,
        /// the method schedules sailing operations weekly and iterates until the <paramref name="endTime"/>.
        /// It calculates the weekly sailing times and initiates sailing accordingly.
        /// </remarks>
        public void RecurringSailing(DateTime startSailing, DateTime endTime, int numberOfDaysAtSailing, RecurringType dailyOrWeekly)
        {
            try
            {
                if (dailyOrWeekly.Equals(Enums.RecurringType.Daily))
                {
                    if (startSailing.Date == CurrentTime.Date && startSailing.Hour == CurrentTime.Hour && startSailing.Minute == CurrentTime.Minute)
                    {
                        Sailing(startSailing, numberOfDaysAtSailing);
                    }
                }
                else if (dailyOrWeekly.Equals(Enums.RecurringType.Weekly))
                {
                    var weekly = startSailing;
                    int weekCounter = 1;

                    while (CurrentTime <= endTime && weekly <= endTime)
                    {
                        weekly = weekly.AddDays(7 * weekCounter);

                        if (weekly.Date == CurrentTime.Date && weekly.Hour == CurrentTime.Hour && weekly.Minute == CurrentTime.Minute)
                        {
                            Sailing(weekly, numberOfDaysAtSailing);
                            weekCounter++;
                            Console.WriteLine(weekCounter);
                            Console.WriteLine(weekly);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in RecurringSailing method: " + e.Message);
            }
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
            stringBuilder.AppendLine($"Cargo Storage: Capacity: {harborCargoStorage.Capacity}, Occupied Space: {harborCargoStorage.GetOccupiedSpace()}\n");
            return stringBuilder.ToString();
        }

    }


}


