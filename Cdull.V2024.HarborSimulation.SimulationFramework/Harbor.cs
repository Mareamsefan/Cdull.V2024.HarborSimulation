
using System.ComponentModel.Design;
using System.Text;
using System.Xml.Schema;
using static Cdull.V2024.HarborSimulation.SimulationFramework.Enums;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Harbor
    {

        private string name; 
        public List<Dock> Docks { get; } = new List<Dock>();
        public List<Ship> Ships { get; } = new List<Ship>();
        internal List<Ship> DockedShips { get;  } = new List<Ship>();
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
        /// A method to create docks in the harbor.
        /// </summary>
        /// <param name="numberOfDock">The amount of docks you want to create</param>
        /// <param name="type">The type of dock you want to create</param>
        /// <param name="size">The size of the dock you're creating</param>
        public void InitializeDocks(int numberOfDock, Model dockModel,  Size dockSize)
        {
            try
            {
                for (int i = 0; i < numberOfDock; i++)
                {

                    Dock dock = new($"dock{i}", dockSize, dockModel, new Crane($"crane{i}"));
                    Docks.Add(dock);


                }



            }
            catch (Exception e){
                throw new Exception("Error initializing docks.", e);
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
            try
            {
                for (int i = 0; i < numberOfShips; i++)
                {
                    Ship ship = new($"ship{i}", shipModel, shipSize);
                    ship.InitializeCargo(numberOfCargo);
                    Ships.Add(ship);
                }
            }
            
            catch (Exception e)
            {
                throw new Exception("Error initializing ships.", e);
            }

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
                throw new Exception("Error finding available dock.", e);
            }

            return null;

        }
        /// <summary>
        /// A method that puts ALL the ships in the simulation into the waiting queue.
        /// </summary>
        public void QueueShipsToDock()
        {
            if(Ships.Count == 0)
            {
                throw new ArgumentNullException(nameof(Ships), "Harbor cannot have Zero Ships.");
            }

            try
            {
                foreach (Ship ship in Ships)
                {
                    if (!ship.IsSailing && !ship.HasDocked)
                    {
                        WaitingShips.Enqueue(ship);
                    }

                }

            }
            catch (Exception e)
            {
                throw new Exception("Error queuing ships to dock.", e);
            }


        }
        // inkluderer sikring av dock 

        /// <summary>
        /// A method to dock the ships in the harbor. Docks all the ships that has a dock available
        /// to them (Checks size of the available docks and then docks the ship)
        /// </summary>

        public void DockShips(DateTime currentTime)
        {
            try
            {
                while (WaitingShips.Count > 0)
                {

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

            catch (Exception e)
            {
                throw new Exception("Error docking ships.", e);
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
                        cargoStorage.AddCargo(cargo);
                        ship.Cargo.Remove(cargo);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error adding cargo to storage.", e); 
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
                for (int i = 0; i < numberOfCargo; i++)
                {
                    foreach (Cargo cargo in cargoStorage.Cargo)
                    {
                        foreach (Ship ship in DockedShips)
                        {
                            if (ship.Cargo.Count == 0)
                            {
                                ship.Cargo.Add(cargo);
                                cargoStorage.RemoveCargo(cargo);
                            }

                        }

                    }
                }
            }

            catch (Exception e)
            {
                throw new Exception("Error adding cargo to ships.", e);
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
