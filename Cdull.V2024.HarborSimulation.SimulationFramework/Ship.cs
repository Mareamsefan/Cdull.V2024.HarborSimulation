using System;
using System.ComponentModel.Design;


namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    /// <summary>
    /// Represents a ship in the harbor simulation.
    /// </summary>
    public class Ship
    {
        internal string Name { get; set; }
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
        internal int CurrentLocation { get; set; }
        internal int DestinationLocation { get; set; }
        internal bool HasReachedDestination { get; set; }

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
            CurrentLocation =  GenerateRandomDestination(100, 1000); 
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
        public void InitializeCargo(int number, double weight = 10)
        {
            for (int i = 0; i < number; i++)
            {
                Cargo cargo = new($"cargo{i}", weight);
                Cargo.Add(cargo);
            }

        }

        /// <summary>
        /// Sets the destination location of the ship relative to a specified location.
        /// </summary>
        /// <param name="Location">The current location, such as harbor, dock, or sailing point.</param>
        /// <param name="destination">The desired destination location.</param>
        /// <remarks>
        /// This method calculates the distance from the current location to the destination location and sets it as the ship's new destination.
        /// The current location and destination are provided as parameters, and the absolute difference between them determines the distance.
        /// </remarks>
        public void SetDestinationLocationFrom (int Location, int destination)
        {
            DestinationLocation = Math.Abs(destination - Location);
        }

        /// <summary>
        /// Retrieves the history of the ship.
        /// </summary>
        /// <returns>A list of strings representing the ship's history.</returns>
        public List<string> GetShipHistory()
        {
            return History;
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
        /// Moves the ship towards its destination.
        /// </summary>
        public void Move()
        {    
            int newLocation = CurrentLocation + ((Speed * 1000) / 60); 

            CurrentLocation = Math.Min(DestinationLocation, newLocation);

            if (CurrentLocation >= DestinationLocation)
            {
                HasReachedDestination = true; 
            }
        }


    }
}
