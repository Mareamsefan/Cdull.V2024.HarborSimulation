using static Cdull.V2024.HarborSimulation.SimulationFramework.Enums;
// spør marius om det er ok at man henter enums slik og at klassen med enums er public

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Ship
    {
        //kanskje internal? 
        public string Name { get; }
        //GJort public
        internal Model Model { get; }
        internal Size Size { get; }
        internal bool HasDocked { get; set; }
        internal List<Cargo> Cargo { get; } = new List<Cargo>();
        internal string DockedAtTime { get; set; }
        internal string SailedAtTime { get; set; }
        internal bool IsSailing { get; set; }
        internal bool IsReadyToSail{ get; set; }
        internal bool IsWaitingForSailing { get; set; }
        internal int ShipSpeed { get; private set; }
        internal Dock? DockedAt { get; set; }



        public Ship (Harbor shipharbor, string shipName, Model shipModel, Size shipSize)
        {
            Harbor = shipharbor; 
            Name = shipName;
            Model = shipModel;
            Size = shipSize;
            HasDocked = false;
            IsSailing = false;
            DockedAt = null; 
            IsWaitingForSailing = false;
            DockedAtTime = "";
            SailedAtTime = "";
            ShipSpeed = 100;
            IsReadyToSail = false;
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





        /// <summary>
        /// A method that returns the name of the ship.
        /// </summary>
        /// <returns>Name of the ship.</returns>
        public override String ToString()
        {
            return "Name: " + Name + " Model: " + Model + " Size: " + Size + " Has docked: " + HasDocked;
        }


    }
}
