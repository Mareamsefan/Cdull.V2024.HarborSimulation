using System;
using System.ComponentModel.Design;
using System.Text;
using Cdull.V2024.HarborSimulation.SimulationFramework.Enums;


namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    /// <summary>
    /// Represents a ship in the harbor simulation.
    /// </summary>
    public class Ship
    {
        public string Name { get; set; }
        internal Model Model { get; set; }
        internal Size Size { get; set; }
        internal bool HasDocked { get; set; }
        internal List<Cargo> Cargo { get; } = new List<Cargo>(); 
        internal List<string> History { get; } = new List<String>();
        internal string DockedAtTime { get; set; }
        internal string SailedAtTime { get; set; }
        public bool IsSailing { get; set; }
        public bool IsReadyToSail{ get; set; }
        internal bool IsWaitingForSailing { get; set; }
        internal int Speed { get; private set; }
        internal Dock? DockedAt { get; set; }
        internal int CurrentLocation {  get; set; }
        internal bool HasReachedDestination { get; set; }
        internal bool IsLoadingCompleted { get; set; }
        internal bool IsUnloadingCompleted { get; set; }

        /// <summary>
        /// Initializes a new instance of the Ship class.
        /// </summary>
        /// <param name="shipName">The name of the ship.</param>
        /// <param name="shipModel">The model of the ship.</param>
        /// <param name="shipSize">The size of the ship.</param>
        public Ship (string shipName, Model shipModel, Size shipSize)
        {
            Name = shipName;
            Model = shipModel;
            Size = shipSize;
            HasDocked = false;
            IsSailing = false;
            DockedAt = null; 
            IsWaitingForSailing = false;
            DockedAtTime = "";
            SailedAtTime = "";
            Speed = GetSpeedFromModel(shipModel);
            IsReadyToSail = false;
            CurrentLocation =  GenerateRandomDestination(1000, 10000); 
        }
        /// <summary>
        /// Generates a random destination for the ship.
        /// </summary>
        /// <param name="min">The minimum destination value.</param>
        /// <param name="max">The maximum destination value.</param>
        /// <returns>The randomly generated destination.</returns>
        public int GenerateRandomDestination(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max + 1);
        }

        /// <summary>
        /// Gets the speed of the ship based on its model.
        /// </summary>
        /// <param name="model">The model of the ship.</param>
        /// <returns>The speed of the ship.</returns>
        private int GetSpeedFromModel(Model model)
        {
            switch (model)
            {
                case Model.ContainerShip:
                    return 30; 
                case Model.Bulker:
                    return 40; 
                case Model.Tanker:
                    return 50; 
                case Model.LNGCarrier:
                    return 45; 
                case Model.RoRo:
                    return 35;
                default: 
                    return 0; 
            }
        }

        /// <summary>
        /// Initializes cargo on the ship.
        /// </summary>
        /// <param name="number">The number of cargo to initialize.</param>
        /// <param name="weight">The weight of each cargo.</param>
        internal void InitializeCargo(int number, double weight = 10, Size size = Size.Large)
        {
            for (int i = 0; i < number; i++)
            {
                Cargo cargo = new($"cargo{i}", weight, size);
                Cargo.Add(cargo);
            }

        }

     
        /// <summary>
        /// Retrieves the cargo carried by the ship.
        /// </summary>
        /// <returns>A list of Cargo objects representing the ship's cargo.</returns>
        public List<Cargo> GetShipCargo()
        {
            return Cargo;
        }



     

        /// <summary>
        /// Returns a string representation of the ship with only its name and model.
        /// </summary>
        /// <returns>A string representing the ship's name and model.</returns>
        public override string ToString()
        {
            return $"Ship Name: {Name}, Model: {Model}";
        }


        private int CalculateMaxCargoCapacity(Model model)
        {
            int maxCapacity = 0;

            switch (model)
            {
                case Model.ContainerShip:
                    maxCapacity = 200000;
                    break;
                case Model.Bulker:
                    maxCapacity = 1000;
                    break;
                case Model.Tanker:
                    maxCapacity = 100000;
                    break;
                case Model.LNGCarrier:
                    maxCapacity = 200;
                    break;
                case Model.RoRo:
                    maxCapacity = 300000;
                    break;
                default:
                    maxCapacity = 0;
                    break;
            }

            int smallCargoCount = 0;
            int largeCargoCount = 0;

            foreach (var cargo in Cargo)
            {
                if (cargo.Size == Size.Small)
                {
                    smallCargoCount++;
                }
                else
                {
                    largeCargoCount++;
                }
            }

            maxCapacity -= largeCargoCount;
            maxCapacity -= smallCargoCount / 2;

            return maxCapacity;
        }

    }
}
