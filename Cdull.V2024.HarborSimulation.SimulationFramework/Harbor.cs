
using System.ComponentModel.Design;
using System.Xml.Schema;
using static Cdull.V2024.HarborSimulation.SimulationFramework.Enums;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Harbor
    {

        private string Name { get; }
        public Watch Watch { get; set; }
        private List<Dock> Docks { get; } = new List<Dock>();
        private List<Ship> Ships { get; } = new List<Ship>();
        private List<Ship> DockedShips { get; } = new List<Ship>();
        private Queue<Ship> WaitingShips { get; } = new Queue<Ship>();
        private List<Crane> Cranes { get; } = new List<Crane>();

        private CargoStorage CargoStorage { get; } = new CargoStorage("CargoStorage");

        /// <summary>
        /// A method to create the harbor for the simulation.
        /// </summary>
        /// <param name="harborName">Navnet på Havnen</param>
        /// <param name="harborCargoStorage">Antall lagringsplasser ment for cargo</param>
        public Harbor(string harborName, CargoStorage harborCargoStorage)
        {
            this.Name = harborName;
            this.CargoStorage = harborCargoStorage; 
          
        }

        /// <summary>
        /// A method for the user to decide at what time they want to start and stop the simulation.
        /// </summary>
        /// <param name="startWatch">The date and time you want the simulation to start.</param>
        /// <param name="stopWatch">The date and time you want the simulation to stop.</param>
        public void SetUpWatch(DateTime startWatch, DateTime stopWatch) 
        {
            this.Watch = new Watch(startWatch, stopWatch);
        }

        /// <summary>
        /// A method to create cranes in the harbor, you choose how many with an int number
        /// </summary>
        /// <param name="numberOfCranes">The amount of cranes you want to make</param>
        public void InitializeCranes(int numberOfCranes)
        {
            for (int i = 0; i <= numberOfCranes; i++)
            {
                Crane crane = new($"crane{i}");
                this.Cranes.Add(crane);
            }
        }

        /// <summary>
        /// A method to create docks in the harbor.
        /// </summary>
        /// <param name="numberOfDock">The amount of docks you want to create</param>
        /// <param name="type">The type of dock you want to create</param>
        /// <param name="size">The size of the dock you're creating</param>
        public void InitializeDocks(int numberOfDock, DockType type,  DockSize size)
        {
            for (int i = 0; i <= numberOfDock; i++)
            {

                Dock dock = new($"dock{i}", size, type, this.Cranes[i]);
                this.Docks.Add(dock);
                

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
        public void InitializeShips(int numberOfShips, ShipSize shipSize, ShipType shipType,  int numberOfCargos, int CargoWeight = 10)
        {
            for (int i = 0; i <= numberOfShips; i++)
            {
                Ship ship = new($"ship{i}", shipType, shipSize);
                ship.InitializeCargo(numberOfCargos);
                this.Ships.Add(ship);
            }

        }

        /// <summary>
        /// A method to get available docks for the ships that want to dock.
        /// </summary>
        /// <param name="shipSize">The size of the ship</param>
        /// <returns>A dock that has the same size as the ship and is available, if nothing is available, returns null.</returns>
        public Dock GetAvailableDockOfSize(ShipSize shipSize)
        {
            foreach (Dock dock in this.Docks)
            {
                if (dock.Size.Equals(shipSize) && dock.IsAvalible)
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
            foreach (Ship ship in this.Ships)
            {
                this.WaitingShips.Enqueue(ship);
            }
        }
        // inkluderer sikring av dock 

        /// <summary>
        /// A method to dock the ships in the harbor. Docks all the ships that has a dock available
        /// to them (Checks size of the available docks and then docks the ship)
        /// </summary>
        public void DockShips()
        {   //kjører så lenge det er noen ship som venter på å docke 
            while (this.WaitingShips.Count < 0)
            {   // henter ut første skip i køen 
                Ship ship = this.WaitingShips.Peek();
                Dock availableDock = GetAvailableDockOfSize(ship.Size);

                if (availableDock is not null)
                {
                    if (ship.Size.Equals("Small")) {
                        this.Watch.StartTime.AddSeconds(150);
                    }
                    else if (ship.Size.Equals("Medium"))
                    {
                        this.Watch.StartTime.AddSeconds(250);
                    }
                    // ca 16 min for docking 
                    else
                    {
                        this.Watch.StartTime.AddSeconds(350);
                    }
                    ship.HasDocked = true;
                    ship.DockedBy = availableDock;
                    availableDock.IsAvalible = false;
                    ship.History.Add($"{DateTime.Now} + {availableDock.Name}");
                    this.DockedShips.Add(ship);
                    this.WaitingShips.Dequeue();
                }
                else
                {   //avslutter løkken dersom det ikke er noen docker for dette skipet 
                    break;
                }
            }
        }
        
        /// <summary>
        /// A method to move cargo from a ship to the harbor.
        /// </summary>
        public void AddCargoToStorage()
        {
            foreach (Ship ship in this.Ships)
            {
                foreach (Cargo cargo in ship.Cargo)
                {
                    CargoStorage.AddCargo(cargo);
                    ship.Cargo.Remove(cargo);
                }
            }

        }


        /// <summary>
        /// A method to move cargo from the harbor onto a ship.
        /// </summary>
        /// <param name="numberOfCargo">The amount of cargo you want to move</param>
        /// <param name="ship">The ship you want to move the cargo to</param>
        public void AddCargoToShip(int numberOfCargo, Ship ship)
        {
            for (int i = 0; i <= numberOfCargo; i++)
            {
                foreach (Cargo cargo in CargoStorage.Cargo)
                {
                    ship.Cargo.Add(cargo);
                    CargoStorage.RemoveCargo(cargo);
                }
            }
        }

        /// <summary>
        /// Prints out information about the harbor, a list of all the docks, ships docked, 
        /// ships that are waiting in the ship queue, cranes and a list of the cargo storage.
        /// </summary>
        /// <returns>A string with all the info</returns>
        public override string ToString()
        {

            string harborInfo = $"Harbor Name: {Name}\n" +
                       $"Dock List: {string.Join(", ", Docks.Select(dock => dock.ToString()))}\n" +
                       $"Ship List: {string.Join(", ", Ships.Select(ship => ship.ToString()))}\n" +
                       $"Ship Queue: {string.Join(", ", WaitingShips.Select(ship => ship.ToString()))}\n" +
                       $"Crane List: {string.Join(", ", Cranes.Select(crane => crane.ToString()))}\n" +
                       $"Cargo Storage List: {string.Join(", ", CargoStorage.ToString())}";

            return harborInfo;
        }
    }
    
    
}
