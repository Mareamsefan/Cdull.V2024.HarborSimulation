
namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    internal class Harbor
    {
        private string name { get;  }
        private List<Dock> dockList { get;  } = new List<Dock>();
        private List<Ship> shipList { get; } = new List<Ship>();
        private Queue<Ship> shipQueue { get; } = new Queue<Ship>();
        private List<Crane> craneList { get; } = new List<Crane>();
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

     

    
    }


}

