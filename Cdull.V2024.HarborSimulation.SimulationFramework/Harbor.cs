
using System.ComponentModel.Design;
using System.Xml.Schema;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Harbor
    {

        private string Name { get; }
        private List<Dock> Docks { get; } = new List<Dock>();
        private List<Ship> Ships { get; } = new List<Ship>();
        private Queue<Ship> WaitingShips { get; } = new Queue<Ship>();
        private List<Crane> Cranes { get; } = new List<Crane>();

        private CargoStorage CargoStorage { get; }


        public Harbor(string harborName, /* List<Dock> harborDockList, List<Ship> harborShipList,
            Queue<Ship> harborShipQueue, List<Crane> harborCraneList,*/ CargoStorage harborCargoStorage)
        {
            this.Name = harborName;
            /* this.Docks = harborDockList;
             this.Ships = harborShipList;
             this.WaitingShips = harborShipQueue;
             this.Cranes = harborCraneList;*/
            this.CargoStorage = harborCargoStorage; 

        }
        public List<Crane> GetCraneList()
        {
            return Cranes;
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

        public void InitializeCranes(int numberOfCranes)
        {
            for (int i = 0; i < numberOfCranes; i++)
            {
                Crane crane = new($"crane{i}");
                this.Cranes.Add(crane);
            }
        }

        public void InitializeDocks(int numberOfDock, String type, String size, List<Crane> cranes)
        {
            for (int i = 0; i < numberOfDock; i++)
            {
                Dock dock = new($"dock{i}", size, type, cranes[i]);
                
                this.Docks.Add(dock);

            }

        }

        public void InitializeShips(int numberOfShips, string shipSize, int numberOfCargos, int CargoWeight = 10)
        {
            for (int i = 0; i < numberOfShips; i++)
            {
                Ship ship = new($"ship{i}", shipSize);
                ship.InitializeCargos(numberOfCargos);
                this.Ships.Add(ship);
            }

        }

        public Dock GetAvailableDockOfSize(string shipSize)
        {
            foreach(Dock dock in this.Docks)
            {
                if(dock.Size == shipSize && dock.IsAvalible)
                {
                    return dock; 
                }
               
            }
            return null; 
        }

        public void QueueShipsToDock()
        {
            foreach(Ship ship in this.Ships)
            {
                this.WaitingShips.Enqueue(ship);
            }
        }

        public void DockShips()
        {
            foreach(Ship ship in this.Ships)
            { 
                Dock availableDock = GetAvailableDockOfSize(ship.Size); 

                if (availableDock is not null){
                    ship.HasDocked = true;
                    ship.DockedBy = availableDock;
                    availableDock.IsAvalible = false;
                    ship.History.Add($"{DateTime.Now} + {availableDock.Name}");
                   
                }

            }
           
        }
    }
        
       

}

     

   



