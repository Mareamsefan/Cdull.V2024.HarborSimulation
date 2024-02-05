
using System.ComponentModel.Design;
using System.Text;
using System.Xml.Schema;
using static Cdull.V2024.HarborSimulation.SimulationFramework.Enums;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Harbor
    {

        private string name; 
        public Watch Watch { get; }
        public List<Dock> Docks { get; } = new List<Dock>();
        public List<Ship> Ships { get; } = new List<Ship>();
        public List<Ship> DockedShips { get; } = new List<Ship>();
        public List<Ship> SailingShips { get; } = new List<Ship>();
        public Queue<Ship> WaitingShips { get; } = new Queue<Ship>();

        internal CargoStorage cargoStorage  = new CargoStorage("CargoStorage");

        /// <summary>
        /// A method to create the harbor for the simulation.
        /// </summary>
        /// <param name="harborName">Navnet på Havnen</param>
        /// <param name="harborCargoStorage">Antall lagringsplasser ment for cargo</param>
        public Harbor(string harborName)
        {
            name = harborName;

        }
        
        /// <summary>
        /// A method for the user to decide at what time they want to start and stop the simulation.
        /// </summary>
        /// <param name="startWatch">The date and time you want the simulation to start.</param>
        /// <param name="stopWatch">The date and time you want the simulation to stop.</param>
        /*public void SetUpWatch(DateTime startWatch, DateTime stopWatch) 
        {
            Watch = new Watch(startWatch, stopWatch);
        }*/

  

        /// <summary>
        /// A method to create docks in the harbor.
        /// </summary>
        /// <param name="numberOfDock">The amount of docks you want to create</param>
        /// <param name="type">The type of dock you want to create</param>
        /// <param name="size">The size of the dock you're creating</param>
        public void InitializeDocks(int numberOfDock, Model dockModel,  Size dockSize)
        {
            for (int i = 0; i < numberOfDock; i++)
            {

                Dock dock = new($"dock{i}", dockSize, dockModel, new Crane($"crane{i}"));
                Docks.Add(dock);
                

            }

        }

        /// <summary>
        /// A method to create ships for the simulation. Were using Enum for the ships size and type.
        /// </summary>
        /// <param name="numberOfShips">The amount of ships you want to create</param>
        /// <param name="shipSize">The size of the ships you're creating</param>
        /// <param name="shipType">The type of ship you want to create</param>
        /// <param name="numberOfCargos">The amount of cargo on the ship</param>
        /// <param name="CargoWeight">The weight of all the cargo on the ship</param>
        public void InitializeShips(int numberOfShips, Size shipSize, Model shipModel,  int numberOfCargo, int CargoWeight = 10)
        {
            for (int i = 0; i < numberOfShips; i++)
            {
                Ship ship = new($"ship{i}", shipModel, shipSize);
                ship.InitializeCargo(numberOfCargo);
                Ships.Add(ship);
            }

        }

        /// <summary>
        /// A method to get available docks for the ships that want to dock.
        /// </summary>
        /// <param name="shipSize">The size of the ship</param>
        /// <returns>A dock that has the same size as the ship and is available, if nothing is available, returns null.</returns>
        public Dock AvailableDockOfSize(Size shipSize)
        {
            foreach (Dock dock in Docks)
            {
                if (dock.Size.Equals(shipSize) && dock.IsAvailable)
                {
                    return dock;
                }

            }
            return null;
        }

        /// <summary>
        /// A method that puts ALL the ships in the simulation into the waiting queue.
        /// </summary>
        public void QueueShipsToDock()
        {
            foreach (Ship ship in Ships)
            {
                if (!ship.IsSailing && !ship.HasDocked)
                {
                    WaitingShips.Enqueue(ship);
                }
               
            }
        }
        // inkluderer sikring av dock 

        /// <summary>
        /// A method to dock the ships in the harbor. Docks all the ships that has a dock available
        /// to them (Checks size of the available docks and then docks the ship)
        /// </summary>

        public void DockShips(DateTime currentTime)
        {
            while (WaitingShips.Count > 0)
            {
                /*bool AllShipsDockedPrinted = false; 
                if (WaitingShips.Count == 0 && !AllShipsDockedPrinted)
                {
                    Console.WriteLine("All ships docked successfully.");
                    AllShipsDockedPrinted = true;  // Sett indikatoren til true for å unngå flere utskrifter
                }*/

                Ship ship = WaitingShips.Peek();
                Dock availableDock = AvailableDockOfSize(ship.Size);
                

                if (availableDock is not null && ship.IsSailing == false)
                {
      
                    ship.HasDocked = true;
                    ship.DockedAt = availableDock;
                    availableDock.IsAvailable = false;
                    availableDock.OccupiedBy = ship;
                    ship.History.Add($"{ship.Name} docked At: {currentTime} at {availableDock.Name}");
                    DockedShips.Add(ship);
                    WaitingShips.Dequeue();
                }
                else
                {
                    
                    break;
                }
            }
           
        }

        /// <summary>
        /// A method to move cargo from a ship to the harbor.
        /// </summary>
        public void AddCargoToStorage()
        {
            List<Ship> dockedShipsCopy = new List<Ship>(DockedShips);

            foreach (Ship ship in dockedShipsCopy)
            {
                List<Cargo> shipCargoCopy = new List<Cargo>(ship.Cargo);

                foreach (Cargo cargo in shipCargoCopy)
                {
                    cargoStorage.AddCargo(cargo);
                    ship.Cargo.Remove(cargo);
                }
            }

        }


        /// <summary>
        /// A method to move cargo from the harbor onto a ship.
        /// </summary>
        /// <param name="numberOfCargo">The amount of cargo you want to move</param>
        /// <param name="ship">The ship you want to move the cargo to</param>
        public void AddCargoToShip(int numberOfCargo)
        {
            for (int i = 0; i < numberOfCargo; i++)
            {
                foreach (Cargo cargo in cargoStorage.Cargo)
                {
                    foreach(Ship ship in DockedShips)
                    {
                        if (ship.Cargo == null)
                        {
                            ship.Cargo.Add(cargo);
                            cargoStorage.RemoveCargo(cargo);
                        }
                    }
                    
                }
            }
        }

        //NY
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

        /// <summary>
        /// A method to simulate a sailing for a ship.
        /// </summary>
        /// <param name="ship">The ship that is sailing</param>
        /// <param name="sailingStartTime">The time of the departure</param>
        //trenger vi sailingTimeStop????
        public void Sailing(Ship ship, DateTime currentTime, DateTime sailingStartTime, int numberOfDays)
        {
            if (currentTime == sailingStartTime) { 

                if (RemoveShipFromDock(ship))
                {
                   
                    ship.History.Add($"Started sailing At: {sailingStartTime}");
                    ship.IsSailing = true;
                    SailingShips.Add(ship);
                }
                

            }
            else if (currentTime == sailingStartTime.AddDays(numberOfDays))
            {
                ship.IsSailing = false; 
                SailingShips.Remove(ship);
                WaitingShips.Enqueue(ship); 
               
            }
            else
            {
                ship.IsWaitingForSailing = true;
            }

        }

        public void RecurringSailing(Ship ship,DateTime currentTime, DateTime sailingStartTime, RecurringType recurringType, DateTime CurrentTime)
        {
            TimeSpan weeklySailing = TimeSpan.FromDays(7);
            TimeSpan dailySailing = TimeSpan.FromDays(1);

            if (ship.DockedAt != null)
            {
                if (RemoveShipFromDock(ship))
                {

                }

                while (Watch.CurrentTime < sailingStartTime.AddDays(1))  // Repeat until the next day
                {
                    // Sjekk om det er daglig eller ukentlig seiling og legg til riktig tidsintervall
                    if (recurringType == RecurringType.Daily)
                    {
                        RemoveShipFromDock(ship);
                        ship.IsSailing = true;
                        Watch.AddTime(dailySailing);
                        Console.WriteLine("Daily sailing! \n");
                        Thread.Sleep(2000);
                        QueueShipsToDock();
                        DockShips(CurrentTime);
                        Console.WriteLine("Ship has returned from the sailing and is now docked!");
                    }
                    else if (recurringType == RecurringType.Weekly)
                    {
                        RemoveShipFromDock(ship);
                        ship.IsSailing = true;
                        Watch.AddTime(weeklySailing);
                        Console.WriteLine("Weekly Sailing! \n");
                        Thread.Sleep(2000);
                        QueueShipsToDock();
                        DockShips(CurrentTime);
                        Console.WriteLine("Ship has returned from the sailing and is now docked!");
                    }

                    ship.IsSailing = true;
                    Console.WriteLine(Watch.MeasureTimeElapsed());
                    Console.WriteLine(ship.IsSailing);
                }
            }
            else
            {
                Console.WriteLine("Ship is not docked");
            }
        }

        /// <summary>
        /// Prints out information about the harbor, a list of all the docks, ships docked, 
        /// ships that are waiting in the ship queue, cranes and a list of the cargo storage.
        /// </summary>
        /// <returns>A string with all the info</returns>
        public override string ToString()
        {

            string harborInfo = $"Harbor Name: {name}\n" +
                       $"Dock List: {string.Join(", ", Docks.Select(dock => dock.ToString()))}\n" +
                       $"Ship List: {string.Join(", ", Ships.Select(ship => ship.ToString()))}\n" +
                       $"Ship Queue: {string.Join(", ", WaitingShips.Select(ship => ship.ToString()))}\n" +
                    
                       $"Cargo Storage List: {string.Join(", ", cargoStorage.ToString())}";

            return harborInfo;
        }


    }


    
    
}
