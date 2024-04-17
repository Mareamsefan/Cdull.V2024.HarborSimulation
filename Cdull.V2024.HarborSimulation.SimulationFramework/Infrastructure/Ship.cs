using Cdull.V2024.HarborSimulation.SimulationFramework.ContainerOperations;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure
{
    /// <summary>
    /// Represents a ship in the harbor simulation.
    /// </summary>
    public class Ship
    {
        public string Name { get; set; }
        public Model Model { get; set; }
        public Size Size { get; set; }
        internal bool HasDocked { get; set; }
        public List<Container> Containers { get; } = new List<Container>();
        internal List<string> History { get; } = new List<string>();
        internal string DockedAtTime { get; set; }
        internal string SailedAtTime { get; set; }
        public bool IsSailing { get; set; }
        public bool IsReadyToSail { get; set; }
        internal bool IsWaitingForSailing { get; set; }
        internal float Speed { get; private set; }
        internal Dock? DockedAt { get; set; }
        internal int CurrentLocation { get; set; }
        internal bool HasReachedDestination { get; set; }
        internal bool IsLoadingCompleted { get; set; }
        internal bool IsUnloadingCompleted { get; set; }

        public List<ScheduledContainerHandling> ScheduledContainerHandlings { get; set; } = new List<ScheduledContainerHandling>();


        /// <summary>
        /// Initializes a new instance of the Ship class.
        /// </summary>
        /// <param name="shipName">The name of the ship.</param>
        /// <param name="shipModel">The model of the ship.</param>
        /// <param name="shipSize">The size of the ship.</param>
        public Ship(string shipName, Model shipModel, Size shipSize, int shipCurrentLocation)
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
            CurrentLocation = shipCurrentLocation;
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
            return (int)model;
        }

        /// <summary>
        /// Initializes container on the ship.
        /// </summary>
        /// <param name="number">The number of container units to initialize.</param>
        /// <param name="size">The size of each container unit. Default is set to CargoSize.Large.</param>
        public void InitializeContainers(int number, ContainerSize size = ContainerSize.Large)
        {
            if (Model != Model.ContainerShip)
            {
                throw new InvalidOperationException("Only Container Ships can carry container.");
            }

            for (int i = 0; i < number; i++)
            {
                Container container = new($"container{i}", size);
                Containers.Add(container);
            }

        }


        /// <summary>
        /// Retrieves the container carried by the ship.
        /// </summary>
        /// <returns>A list of Containers objects representing the ship's container.</returns>
        public List<Container> GetShipCargo()
        {
            return Containers;
        }





        /// <summary>
        /// Returns a string representation of the ship with only its name and model.
        /// </summary>
        /// <returns>A string representing the ship's name and model.</returns>
        public override string ToString()
        {
            return $"Ship Name: {Name}, Model: {Model}";
        }




    }
}
