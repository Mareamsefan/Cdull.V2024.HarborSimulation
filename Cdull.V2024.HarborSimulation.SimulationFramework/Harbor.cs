
using System.ComponentModel.Design;
using System.Xml.Schema;
using static Cdull.V2024.HarborSimulation.SimulationFramework.Enums;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Harbor
    {

        private string Name { get; }
        public Watch Watch { get; set; }
        public List<Dock> Docks { get; } = new List<Dock>();
        public List<Ship> Ships { get; } = new List<Ship>();
        public List<Ship> DockedShips { get; } = new List<Ship>();
        public Queue<Ship> WaitingShips { get; } = new Queue<Ship>();
        public List<Crane> Cranes { get; } = new List<Crane>();

        private CargoStorage CargoStorage { get; } = new CargoStorage("CargoStorage");


        public Harbor(string harborName, CargoStorage harborCargoStorage)
        {
            this.Name = harborName;
            this.CargoStorage = harborCargoStorage; 
          
        }

        public void SetUpWatch(DateTime startWatch, DateTime stopWatch) 
        {
            this.Watch = new Watch(startWatch, stopWatch);
        }

        public void InitializeCranes(int numberOfCranes)
        {
            for (int i = 0; i <= numberOfCranes; i++)
            {
                Crane crane = new($"crane{i}");
                this.Cranes.Add(crane);
            }
        }

        public void InitializeDocks(int numberOfDock, DockType type,  Size size)
        {
            for (int i = 0; i <= numberOfDock; i++)
            {

                Dock dock = new($"dock{i}", size, type, this.Cranes[i]);
                this.Docks.Add(dock);
                

            }

        }

        public void InitializeShips(int numberOfShips, Size shipSize, ShipType shipType,  int numberOfCargos, int CargoWeight = 10)
        {
            for (int i = 0; i <= numberOfShips; i++)
            {
                Ship ship = new($"ship{i}", shipType, shipSize);
                ship.InitializeCargos(numberOfCargos);
                this.Ships.Add(ship);
            }

        }

        public Dock AvailableDockOfSize(Size shipSize)
        {
            foreach (Dock dock in this.Docks)
            {
                if (dock.Size.Equals(shipSize) && dock.IsAvailable)
                {
                    return dock;
                }

            }
            return null;
        }

        public void QueueShipsToDock()
        {
            foreach (Ship ship in this.Ships)
            {
                this.WaitingShips.Enqueue(ship);
            }
        }
        // inkluderer sikring av dock 
        
        public String DockShips()
        {   //kjører så lenge det er noen ship som venter på å docke 
            while (this.WaitingShips.Count > 0)
            {   // henter ut første skip i køen 
                Ship ship = this.WaitingShips.Peek();
                Dock availableDock = AvailableDockOfSize(ship.Size);

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
                    availableDock.IsAvailable = false;
                    availableDock.OccupiedBy = ship; 
                    ship.History.Add($"{DateTime.Now} + {availableDock.Name}");
                    this.DockedShips.Add(ship);
                    this.WaitingShips.Dequeue();
                    return "docket :)";
                }
                else
                {   //avslutter løkken dersom det ikke er noen docker for dette skipet 
                    break;
                    return "docket ikke pga..";
                }
            
            }
            return "koden kjørte ikke";
        }
        

   


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

        //NY
        public void RemoveShipFromDock(Dock dock)
        {
            if (dock != null && dock.OccupiedBy != null)
            {
                Ship ship = (Ship)dock.OccupiedBy;

                // Fjern skipet fra dokken
                dock.OccupiedBy = null;

                // Sett dokken til ledig igjen
                dock.IsAvailable = true;

                // Fjern skipet fra listen over dockede skip
                DockedShips.Remove(ship);

                Console.WriteLine($"Ship '{ship.Name}' has been removed from dock '{dock.Name}' and dock is now available.");
            }
            else if (dock == null)
            {
                Console.WriteLine("Dock parameter is null.");
            }
            else
            {
                Console.WriteLine($"Dock '{dock.Name}' is already available or no ship is docked in it.");
            }
        }


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
