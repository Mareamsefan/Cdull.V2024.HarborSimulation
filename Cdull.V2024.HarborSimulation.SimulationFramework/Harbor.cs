
using System.ComponentModel.Design;
using System.Xml.Schema;
using static Cdull.V2024.HarborSimulation.SimulationFramework.Enums;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Harbor
    {

        private string Name { get; }
        private Watch Watch { get; }
        private List<Dock> Docks { get; } = new List<Dock>();
        private List<Ship> Ships { get; } = new List<Ship>();
        private List<Ship> DockedShips { get; } = new List<Ship>();
        private Queue<Ship> WaitingShips { get; } = new Queue<Ship>();
        private List<Crane> Cranes { get; } = new List<Crane>();

        private CargoStorage CargoStorage { get; } = new CargoStorage("CargoStorage");


        public Harbor(string harborName, CargoStorage harborCargoStorage)
        {
            this.Name = harborName;
            this.CargoStorage = harborCargoStorage; 
          
        }
        public List<Crane> GetCraneList()
        {
            return Cranes;
        }

        public void InitializeCranes(int numberOfCranes)
        {
            for (int i = 0; i <= numberOfCranes; i++)
            {
                Crane crane = new($"crane{i}");
                this.Cranes.Add(crane);
            }
        }

        public void InitializeDocks(int numberOfDock, DockType type,  DockSize size)
        {
            for (int i = 0; i <= numberOfDock; i++)
            {

                Dock dock = new($"dock{i}", size, type, this.Cranes[i]);
                this.Docks.Add(dock);
                

            }

        }

        public void InitializeShips(int numberOfShips, ShipSize shipSize, ShipType shipType,  int numberOfCargos, int CargoWeight = 10)
        {
            for (int i = 0; i <= numberOfShips; i++)
            {
                Ship ship = new($"ship{i}", shipType, shipSize);
                ship.InitializeCargos(numberOfCargos);
                this.Ships.Add(ship);
            }

        }

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

        public void QueueShipsToDock()
        {
            foreach (Ship ship in this.Ships)
            {
                this.WaitingShips.Enqueue(ship);
            }
        }

        public void DockShips()
        {   //kjører så lenge det er noen ship som venter på å docke 
            while (this.WaitingShips.Count < 0)
            {   // henter ut første skip i køen 
                Ship ship = this.WaitingShips.Peek();
                Dock availableDock = GetAvailableDockOfSize(ship.Size);

                if (availableDock is not null)
                {
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
