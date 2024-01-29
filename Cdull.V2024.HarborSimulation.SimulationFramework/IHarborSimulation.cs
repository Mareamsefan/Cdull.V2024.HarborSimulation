using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    internal interface IHarborSimulation
    {
        // made 
        public void Harbor(String name, List<Dock> DockList, List<Ship> ShipList, Queue<Cargo> ShipQueue,
            List<Crane> CraneList, List<CargoStorage> cargoStorageFacility);

        // made 
        public void Dock(String name, String size, String dockType, bool IsAvalible = true);

        // made 
        public void InitializeDocks(int number, String docktype, Crane crane);



        public void AddDock(Dock dock);


        public void AddCrane(Crane crane);


        public void Crane(bool IsCraneAvalible = true, bool IsCraneOutOfFuntion = false);

        // made 
        public void InitializingCranes(int number, List<Dock> DockList);

        //made 
        public void Cargo(String name, double weight);



        // made 
        public void Dock(String name, String size, String dockType = "CargohandlingDock");

        //made 
        void InitiateShipQueue(Queue<Ship> ShipQueue);


        public void Dock(String name, String size, String dockType, Crane crane, bool IsAvalible = true);



        public void Dock(String name, String size, String dockType, Crane crane, bool IsAvalible = true, Ship IsOccupiedBy = null);

        public void Ship(String name, String model, String size, bool HasDocked, List<Cargo> CargoList, String dockname);



        void AddShips(Ship ship, List<Ship> ShipList);



        public void CargoStorageFacility(int number, bool IsAvailable = true);


        void AddCargoToStorage(List<CargoStorage> cargoStorageFacility, List<Cargo> CargoList, Crane crane);

        void MoveCargoToShip(Cargo Cargo, SaveCargoHistory saveCargoHistory);



        
        public void Ship(String name, String model, String size, bool HasDocked, List<Cargo> CargoList,
            String dockname, bool isSailing);

        void Sailing(UnicodeEncoding id, DataSetDateTime datatime, Ship ship, Watch watch);


        void RecurringSailing(Boolean IsWeekly, Boolean IsDaily);

        delegate void CheckIfDockAvailable(bool IsAvailable, String size);


        void DockAt(Dock dock, DataSetDateTime datatime, Watch watch, CheckIfDockAvailable
            checkIfDockAvailable);


        delegate void SaveShipHistory();

        public void Ship(String name, String model, String size, bool HasDocked, List<Cargo> CargoList, List<String> HistoryList, String dockname);


        delegate void SaveCargoHistory();

        public void Cargo(String name, double weight, List<String> HistoryList);

        void AddToQueue(Ship ship, Queue<Ship> ShipQueue);


    
        public void Watch(DataSetDateTime startTime, DataSetDateTime stopTime, bool IsCounting);

        void StartCountingTime();

        void StopCountingTime();

        DateTime MeasureTimeElapsed();

    }
}
