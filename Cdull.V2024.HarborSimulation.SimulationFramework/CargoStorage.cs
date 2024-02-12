namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    /// <summary>
    /// Represents a storage facility for cargo items.
    /// </summary>
    public class CargoStorage
    {
        private string name;

        internal List<Cargo> Cargo { get; set; } = new List<Cargo>();
        internal bool IsAvailable { get; set; }
        internal int Capacity { get; set; }


        /// <summary>
        /// Initializes a new instance of the CargoStorage class with the specified name and capacity.
        /// </summary>
        /// <param name="cargoStorageName">The name of the cargo storage.</param>
        /// <param name="cargoStorageCapacity">The maximum capacity of the cargo storage.</param>
        public CargoStorage(string cargoStorageName, int cargoStorageCapacity)
        {

            name = cargoStorageName;
            IsAvailable = true;
            Capacity = cargoStorageCapacity;
        }

        /// <summary>
        /// A method to add cargo to the cargo storage in the harbor simulation.
        /// </summary>
        /// <param name="cargo">The cargo object thats being added</param>
        public void AddCargo(Cargo cargo)
        {
            Cargo.Add(cargo);
        }

        /// <summary>
        /// A method to remove the cargo from the cargo storage in the harbor simulation.
        /// </summary>
        /// <param name="cargo">The cargo object thats being removed</param>
        public void RemoveCargo(Cargo cargo)
        {
            Cargo.Remove(cargo);
        }

        /// <summary>
        /// Calculates the occupied space in the cargo storage based on the weight of the stored cargo.
        /// </summary>
        /// <returns>The total weight of the cargo stored in the cargo storage.</returns>
        public double GetOccupiedSpace()
        {
            double occupiedSpace = 0;
            foreach (var cargo in Cargo)
            {
                occupiedSpace += cargo.Weight;
            }
            return occupiedSpace;
        }
    }
}
