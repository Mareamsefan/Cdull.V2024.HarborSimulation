
namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Harbor
    {
        private int id { get; set; }
        private string name { get; }
        private List<Dock> dockList { get; } = new List<Dock>();
        private List<Ship> shipList { get; } = new List<Ship>();
        private Queue<Ship> shipQueue { get; } = new Queue<Ship>();
        private List<Crane> craneList { get; } = new List<Crane>();

        private CargoStorage cargoStorage { get; } 

        public Harbor(string harborName, List<Dock> harborDockList, List<Ship> harborShipList,
            Queue<Ship> harborShipQueue, List<Crane> harborCraneList, CargoStorage harborCargoStorage)
        {
            this.name = harborName;
            this.dockList = harborDockList;
            this.shipList = harborShipList;
            this.shipQueue = harborShipQueue;
            this.craneList = harborCraneList;
            this.cargoStorage = harborCargoStorage;

        }
        public List<Crane> GetCraneList()
        {
            return craneList;
        }
        public override string ToString()
        {

            string harborInfo = $"Harbor Name: {name}\n" +
                       $"Dock List: {string.Join(", ", dockList.Select(dock => dock.ToString()))}\n" +
                       $"Ship List: {string.Join(", ", shipList.Select(ship => ship.ToString()))}\n" +
                       $"Ship Queue: {string.Join(", ", shipQueue.Select(ship => ship.ToString()))}\n" +
                       $"Crane List: {string.Join(", ", craneList.Select(crane => crane.ToString()))}\n";
                   

            return harborInfo;
        }

        public void InitializeCranes(int number)
        {
            List<Crane> cranes = new List<Crane>();
            for (int i = 0; i < number; i++)
            {
                Crane crane = new($"crane{i}");
                this.craneList.Add(crane);
            }
        }

        public void InitializeDocks(int number, String type, String size, List<Crane> cranes)
        {
            for (int i = 0; i < number; i++)
            {
                Dock dock = new($"dock{i}", size, type, cranes[i]);
                this.dockList.Add(dock);

            }

        }

        public void InitializeCargosStorage()
        {

        }

        public void AddCargoToStorage( Ship ship, Dock dock)
        {
          
            foreach (var cargo in ship.cargoList)
            {
                cargoStorage.AddCargoToList(cargo);
            }           
            

        }





    }
}

