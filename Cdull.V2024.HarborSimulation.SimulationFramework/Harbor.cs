
namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
     public class Harbor
    {
        private string name { get;  }
        private List<Dock> dockList { get;  } = new List<Dock>();
        private List<Ship> shipList { get; } = new List<Ship>();
        private Queue<Ship> shipQueue { get; } = new Queue<Ship>();
        private List<Crane> craneList {  get; } = new List<Crane>();
        private List<CargoStorage> cargoStorageList { get; } = new List<CargoStorage>();

        public Harbor(string harborName, List<Dock> harborDockList, List<Ship> harborShipList,
            Queue<Ship> harborShipQueue, List<Crane> harborCraneList, List<CargoStorage> harborCargoStorageList)
        {
            this.name = harborName;
            this.dockList = harborDockList;
            this.shipList = harborShipList;
            this.shipQueue = harborShipQueue;
            this.craneList = harborCraneList;
            this.cargoStorageList = harborCargoStorageList;

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
                        $"Crane List: {string.Join(", ", craneList.Select(crane => crane.ToString()))}\n" +
                        $"Cargo Storage List: {string.Join(", ", cargoStorageList.Select(storage => storage.ToString()))}";

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


    }

     

   

}

