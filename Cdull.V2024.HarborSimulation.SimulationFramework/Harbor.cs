﻿using System.Collections;
using System.Text;
using System.Xml.Linq;
using static Cdull.V2024.HarborSimulation.SimulationFramework.Enums;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Harbor
    {
        internal string name;
        internal DateTime CurrentTime { get; set; }
        internal int Location { get; set; }
        internal List<Dock> Docks { get; } = new List<Dock>();
        internal List<Ship> Ships { get; } = new List<Ship>();
        internal List<Ship> DockedShips { get; } = new List<Ship>();
        internal List<Ship> SailingShips { get; } = new List<Ship>();
        internal Queue<Ship> WaitingShips { get; set; } = new Queue<Ship>();
        internal Dictionary<DateTime, Harbor> HarborHistory { get; } = new Dictionary<DateTime, Harbor>();

        internal CargoStorage harborCargoStorage { get; set; }

        /// <summary>
        /// A method to create the harbor for the simulation.
        /// </summary>
        /// <param name="harborName">Navnet på Havnen</param>
        /// <param name="harborCargoStorage">Antall lagringsplasser ment for cargo</param>
        public Harbor(string harborName, CargoStorage harborCargoStorage)
        {
            name = harborName;
            Location = 0; 
            this.harborCargoStorage = harborCargoStorage;
        }
      

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

        //Kilde hentet ut : 09/02/2023: https://learn.microsoft.com/en-us/dotnet/api/system.object.memberwiseclone?view=net-8.0
        private Harbor Clone()
        {
            return (Harbor)MemberwiseClone();
        }
        public Harbor DeepClone()
        {
            Harbor clonedHarbor = new Harbor(name, harborCargoStorage);

            clonedHarbor.CurrentTime = CurrentTime;
            clonedHarbor.Location = Location;

            // Kopier lister og køer
            clonedHarbor.Docks.AddRange(Docks.Select(dock => new Dock(dock.Name, dock.Size, dock.Model)));
            clonedHarbor.Ships.AddRange(Ships.Select(ship => new Ship(ship.Name, ship.Model, ship.Size)));
            clonedHarbor.DockedShips.AddRange(DockedShips.Select(ship => new Ship(ship.Name, ship.Model, ship.Size)));
            clonedHarbor.SailingShips.AddRange(SailingShips.Select(ship => new Ship(ship.Name, ship.Model, ship.Size)));
            clonedHarbor.WaitingShips = new Queue<Ship>(WaitingShips);

            // Kopier historikk
            foreach (var entry in HarborHistory)
            {
                clonedHarbor.HarborHistory[entry.Key] = entry.Value.DeepClone();
            }

            return clonedHarbor;
        }



        public void SaveHarborHistory(DateTime timestamp)
        {
            HarborHistory[timestamp] = DeepClone();
        }
        

        /// <summary>
        /// A method to create docks in the harbor.
        /// </summary>
        /// <param name="numberOfDock">The amount of docks you want to create</param>
        /// <param name="type">The type of dock you want to create</param>
        /// <param name="size">The size of the dock you're creating</param>
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
        /// A method to create ships for the simulation. Were using Enum for the ships size and type.
        /// </summary>
        /// <param name="numberOfShips">The amount of ships you want to create</param>
        /// <param name="shipSize">The size of the ships you're creating</param>
        /// <param name="shipType">The type of ship you want to create</param>
        /// <param name="numberOfCargos">The amount of cargo on the ship</param>
        /// <param name="CargoWeight">The weight of all the cargo on the ship</param>
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
        /// A method to get available docks for the ships that want to dock.
        /// </summary>
        /// <param name="shipSize">The size of the ship</param>
        /// <returns>A dock that has the same size as the ship and is available, if nothing is available, returns null.</returns>
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


        private bool IsShipInQueue(Ship ship)
        {
            foreach(Ship ship_2 in WaitingShips)
            {
                if(ship.Name == ship_2.Name)
                {
                    return true; 
                }
            }
            return false; 
        }

        /// <summary>
        /// A method that puts ALL the ships in the simulation into the waiting queue.
        /// </summary>
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
                        Console.WriteLine(DockedShips.Count + "D-q");
                        Console.WriteLine(WaitingShips.Count + "W-q");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error queuing ships to dock.", e);
            }
        }


        // inkluderer sikring av dock 

        /// <summary>
        /// A method to dock the ships in the harbor. Docks all the ships that has a dock available
        /// to them (Checks size of the available docks and then docks the ship)
        /// </summary>

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

                        if (ship.ReachedDestination)
                        {
                            ship.HasDocked = true;
                            ship.DockedAt = availableDock;
                            availableDock.IsAvailable = false;
                            availableDock.OccupiedBy = ship;
                            ship.DockedAtTime = CurrentTime.ToString();
                            DockedShips.Add(ship);
                            WaitingShips.Dequeue();
                            Console.WriteLine(DockedShips.Count + "D-d");
                            Console.WriteLine(WaitingShips.Count + "W-d");
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
        /// A method to move cargo from a ship to the harbor.
        /// </summary>
        public void AddCargoToStorage()
        {
            try
            {
                List<Ship> dockedShipsCopy = new List<Ship>(DockedShips);

                foreach (Ship ship in dockedShipsCopy)
                {
                    List<Cargo> shipCargoCopy = new List<Cargo>(ship.Cargo);

                    foreach (Cargo cargo in shipCargoCopy)
                    {
                        if(harborCargoStorage.GetOccupiedSpace() > 0)
                        {
                            harborCargoStorage.AddCargo(cargo);
                            ship.Cargo.Remove(cargo);

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
        /// A method to move cargo from the harbor onto a ship.
        /// </summary>
        /// <param name="numberOfCargo">The amount of cargo you want to move</param>
        /// <param name="ship">The ship you want to move the cargo to</param>
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
                                cargo.History.Add($"{cargo.name} loaded at {CurrentTime} on {ship.Name}");
                            }

                            else
                            {
                                break; // No more cargo available in the harbor
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

        public bool RemoveShipFromDock(Ship ship)
        {

            if (ship.DockedAt != null)
            {
                Dock dock = ship.DockedAt;

                // Fjern skipet fra dokken
                dock.OccupiedBy = null;

                // Sett dokken til ledig igjen
                dock.IsAvailable = true;

                // Fjern skipet fra listen over dockede skip
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


        public void Sailing(DateTime sailingStartTime, int numberOfDays)
        {
            try
            {
                foreach (Ship ship in Ships)
                {
                    DateTime sailTimeIsOver = sailingStartTime.AddDays(numberOfDays);

                    if (ship.IsReadyToSail && CurrentTime.CompareTo(sailingStartTime) == 0 && !ship.IsSailing)
                    {
                        Console.WriteLine("TID FOR SEILING KOMMER");

                        // Hvis skipet kan fjernes fra dokken
                        if (RemoveShipFromDock(ship))
                        {
                            ship.SailedAtTime = CurrentTime.ToString();
                            ship.IsSailing = true;
                            SailingShips.Add(ship);
                        }
                    }
                    // Sjekk om det er tid for skipet å stoppe seilingen
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

        private int CalculateDistanceToDestination(Ship ship)
        {
            return Math.Abs(ship.DestinationLocation - ship.CurrentLocation);
        }


        public void RecurringSailing(DateTime startSailing, int numberOfDaysAtSailing, RecurringType dailyOrWeekly)
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
                var weekly = startSailing.AddDays(7);
               
                if (weekly.Date == CurrentTime.Date && weekly.Hour == CurrentTime.Hour && weekly.Minute == CurrentTime.Minute)
                {
                    Sailing(weekly, numberOfDaysAtSailing);
                }
            }
        }


        

        public string GetName()
        {
            return name;
        }

        public DateTime GetCurrentTime()
        {
            return CurrentTime; 
        }

        public DateTime GetCurrentTimeCopy()
        {
            return new DateTime(CurrentTime.Ticks);
        }


        public void SetCurrentTime(DateTime currentTime)
        {
            CurrentTime = currentTime; 
        }
        public List<Dock> GetDocks()
        {
            return Docks;
        }

        public List<Ship> GetShips()
        {
            return Ships;
        }

      

        public List<Ship> GetDockedShips()
        {
            return DockedShips;
        }

        public List<Ship> GetSailingShips()
        {
            return SailingShips;
        }

        public Queue<Ship> GetWaitingShips()
        {
            return WaitingShips;
        }

      
        public CargoStorage GetCargoStorage()
        {
            return harborCargoStorage;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Harbor Name: {name}" + $" CurrentTime: {CurrentTime}");
            sb.AppendLine($"Total Ships: {Ships.Count}");
            sb.AppendLine($"Docked Ships: {DockedShips.Count}");
            sb.AppendLine($"Sailing Ships: {SailingShips.Count}");
            sb.AppendLine($"Waiting Ships: {WaitingShips.Count}");
            sb.AppendLine($"Cargo Storage: Capacity: {harborCargoStorage.Capacity}, Occupied Space: {harborCargoStorage.GetOccupiedSpace()}\n");
            return sb.ToString();
        }
    }
    
    
}


