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


        // made
        public void AddDock(Dock dock);

        // made
        public void AddCrane(Crane crane);

        // made
        public void Crane(bool IsCraneAvalible = true, bool IsCraneOutOfFuntion = false);

        // made 
        public void InitializingCranes(int number, List<Dock> DockList);

        //made 
        public void Cargo(String name, double weight);



        // made 
        public void Dock(String name, String size, String dockType = "CargohandlingDock");

        //made 
        void InitiateShipQueue(Queue<Ship> ShipQueue);

        // made
        public void Dock(String name, String size, String dockType, Crane crane, bool IsAvalible = true);


        // made
        public void Dock(String name, String size, String dockType, Crane crane, bool IsAvalible = true, Ship IsOccupiedBy = null);
        // made

        public void Ship(String name, String model, String size, bool HasDocked, List<Cargo> CargoList, String dockname);


        // made
        void AddShips(Ship ship, List<Ship> ShipList);


        // made
        public void CargoStorage(int number, bool IsAvailable = true);

        // made
        void AddCargoToStorage(List<CargoStorage> cargoStorageFacility, List<Cargo> CargoList, Crane crane);

        // made
        void AddCargoToShip(int numberOfCargo, Ship ship);


        
        public void Ship(String name, String model, String size, bool HasDocked, List<Cargo> CargoList,
            String dockname, bool isSailing);


        void Sailing(UnicodeEncoding id, DataSetDateTime datatime, Ship ship, Watch watch);


        void RecurringSailing(Boolean IsWeekly, Boolean IsDaily);

        // made
        delegate void GetAvailableDockofSize(bool IsAvailable, String size);

        // made
        void DockShips();

        // made ish (ligger i dockships)
        delegate void SaveShipHistory();

        // made
        public void Ship(String name, String model, String size, bool HasDocked, List<Cargo> CargoList, List<String> HistoryList, Dock DockedBy);


        delegate void SaveCargoHistory();

        // made
        public void Cargo(String name, double weight, List<String> HistoryList);

        // made ish (need fix)
        void AddToQueue(Ship ship, Queue<Ship> ShipQueue);

        // made
        public void Watch(DataSetDateTime startTime, DataSetDateTime stopTime, bool IsCounting);

        // made
        void StartCountingTime();

        // made
        void StopCountingTime();

        //made
        DateTime MeasureTimeElapsed();

    }
}
