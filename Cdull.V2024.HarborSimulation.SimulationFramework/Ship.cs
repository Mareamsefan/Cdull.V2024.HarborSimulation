using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cdull.V2024.HarborSimulation.SimulationFramework.Enums;
// spør marius om det er ok at man henter enums slik og at klassen med enums er public

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Ship 
    {

        //kanskje internal? 
        internal string Name {  get; }
        //GJort public
        internal Model Model {  get; }
        internal Size Size { get; }
        internal bool HasDocked { get; set; }
        internal List<Cargo> Cargo { get; } = new List<Cargo>();
        internal DateTime DockedAtTime { get; set; }
        internal DateTime SailedAtTime { get; set; }
        internal bool IsSailing { get; set; }
        internal bool IsWaitingForSailing { get; set; }
        internal int ShipSpeed { get; private set; }
        internal Dock? DockedAt { get; set; }

        internal Harbor Harbor { get; set; }


        public Ship(string shipName, Model shipModel, Size shipSize) {
            Name = shipName;   
            Model = shipModel;
            Size = shipSize;
            HasDocked = false;
            DockedAt = null;
            IsSailing = false;
            IsWaitingForSailing = false;
            ShipSpeed = 100; 
        }

        /// <summary>
        /// A method to create cargo in the simulation.
        /// </summary>
        /// <param name="number">The amount of cargo you want to create.</param>
        /// <param name="weight">The weight of each cargo you're creating.</param>
        public void InitializeCargo(int number, double weight = 10)
        {
            for (int i = 0; i < number; i++)
            {
                Cargo cargo = new($"cargo{i}", weight);
                Cargo.Add(cargo);
            }

        }

        public void AddShipToHarbor(Harbor shipHarbor)
        {
            if (shipHarbor == null)
            {
                throw new ArgumentNullException(nameof(shipHarbor), "Harbor cannot be null.");
            }
            if (HasDocked || DockedAt != null)
            {
                throw new InvalidOperationException($"Ship {Name} is already docked or has a dock assigned.");
            }

            shipHarbor.Ships.Add(this);
            Harbor = shipHarbor;
        }



        //NY
        public bool RemoveShipFromDock(Ship ship)
        {
            if (Harbor == null)
            {
                throw new ArgumentNullException(nameof(Harbor), "Harbor cannot be null.");
            }

            if (ship.DockedAt != null)
            {
                Dock dock = ship.DockedAt;

                // Fjern skipet fra dokken
                dock.OccupiedBy = null;

                // Sett dokken til ledig igjen
                dock.IsAvailable = true;

                // Fjern skipet fra listen over dockede skip
                Harbor.DockedShips.Remove(ship);

                ship.HasDocked = false;
                ship.DockedAt = null;

                return true;
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// A method to simulate a sailing for a ship.
        /// </summary>
        /// <param name="ship">The ship that is sailing</param>
        /// <param name="sailingStartTime">The time of the departure</param>
        //trenger vi sailingTimeStop????
        public void Sailing(Ship ship, DateTime currentTime, DateTime sailingStartTime, int numberOfDays)
        {
            if (Harbor == null)
            {
                Console.WriteLine("Harbor cannot be null.");
            }

            if (currentTime == sailingStartTime)
            {

                if (RemoveShipFromDock(ship))
                {

                    ship.SailedAtTime = currentTime; 
                    ship.IsSailing = true;
                    Harbor.SailingShips.Add(ship);
                }


            }
            else if (currentTime == sailingStartTime.AddDays(numberOfDays))
            {
                ship.IsSailing = false;
                Harbor.SailingShips.Remove(ship);
                Harbor.WaitingShips.Enqueue(ship);

            }
            else
            {
                ship.IsWaitingForSailing = true;
            }

        }


        public void RecurringSailing(Ship ship, DateTime currentTime, DateTime endTime, DateTime sailingStartTime, int numberOfDays, RecurringType recurringType)
        {
            if (Harbor == null)
            {
                throw new ArgumentNullException(nameof(Harbor), "Harbor cannot be null.");
            }

            if (currentTime < endTime)
            {
                if (recurringType == Enums.RecurringType.Daily)
                {

                    if (currentTime >= sailingStartTime.AddDays(1))
                    {

                        if (RemoveShipFromDock(ship))
                        {

                            ship.SailedAtTime = currentTime;
                            ship.IsSailing = true;
                            Harbor.SailingShips.Add(ship);
                        }


                    }
                    else if (currentTime >= sailingStartTime.AddDays(numberOfDays))
                    {
                        ship.IsSailing = false;
                        Harbor.SailingShips.Remove(ship);
                        Harbor.WaitingShips.Enqueue(ship);

                    }
                    else
                    {
                        ship.IsWaitingForSailing = true;
                    }

                }

                if (recurringType == Enums.RecurringType.Weekly)
                {

                    if (currentTime >= sailingStartTime.AddDays(7))
                    {

                        if (RemoveShipFromDock(ship))
                        {

                            ship.SailedAtTime = currentTime;
                            ship.IsSailing = true;
                            Harbor.SailingShips.Add(ship);
                        }


                    }
                    else if (currentTime >= sailingStartTime.AddDays(numberOfDays))
                    {
                        ship.IsSailing = false;
                        Harbor.SailingShips.Remove(ship);
                        Harbor.WaitingShips.Enqueue(ship);

                    }
                    else
                    {
                        ship.IsWaitingForSailing = true;
                    }

                }


            }
        }

        public void AddCargoToStorage(Harbor harbor)
        {
            if (harbor == null)
            {
                throw new ArgumentNullException(nameof(harbor), "Harbor cannot be null.");
            }

            foreach (Cargo cargo in Cargo)
            {
                harbor.cargoStorage.AddCargo(cargo);
            }

            Cargo.Clear();
        }
        
        public void AddCargoToShip(int numberOfCargo, Harbor harbor)
        {
            if (harbor == null)
            {
                throw new ArgumentNullException(nameof(harbor), "Harbor cannot be null.");
            }

            for (int i = 0; i < numberOfCargo; i++)
            {
                if (harbor.cargoStorage.Cargo.Count > 0)
                {
                    Cargo cargo = harbor.cargoStorage.Cargo.First();
                    Cargo.Add(cargo);
                    harbor.cargoStorage.RemoveCargo(cargo);
                }
                else
                {
                    break; // No more cargo available in the harbor
                }
            }
        }


        /// <summary>
        /// A method that returns the name of the ship.
        /// </summary>
        /// <returns>Name of the ship.</returns>
        public override String ToString()
        {
            return "Name: " + Name + " Model: "+ Model + " Size: " + Size + " Has docked: " + HasDocked; 
        }


    }
}
