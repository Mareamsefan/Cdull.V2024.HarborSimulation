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
        internal double OccupiedSpace{ get; set;  }


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
        /// Adds the specified cargo to the cargo storage.
        /// </summary>
        /// <param name="cargo">The cargo object to be added.</param>
        /// <remarks>
        /// This method adds the specified cargo to the cargo storage if it is not already present.
        /// </remarks>
        internal void AddCargo(Cargo cargo)
        {
            if (!Cargo.Contains(cargo))
            {
                Cargo.Add(cargo);
            }
               
        }


        /// <summary>
        /// Removes the specified cargo from the cargo storage.
        /// </summary>
        /// <param name="cargo">The cargo object to be removed.</param>
        /// <remarks>
        /// This method removes the specified cargo from the cargo storage if it exists.
        /// </remarks>
        internal void RemoveCargo(Cargo cargo)
        {
            if (Cargo.Contains(cargo))
            {
                Cargo.Remove(cargo);
            }
               
        }

        /// <summary>
        /// Occupies space in the cargo storage by adding the weight of the provided cargo.
        /// </summary>
        /// <param name="cargo">The cargo to occupy space for.</param>
        /// <remarks>
        /// This method adds the weight of the provided cargo to the occupied space of the cargo storage.
        /// If the occupied space exceeds the capacity of the cargo storage, it marks the storage as unavailable.
        /// </remarks>

        internal void OccupySpace(Cargo cargo)
        {
            if(OccupiedSpace < Capacity)
            {
                OccupiedSpace += 1;
            }
            else
            {
                IsAvailable = false; 
            }    
            
        }

        /// <summary>
        /// Frees up space in the cargo storage by subtracting the weight of the provided cargo.
        /// </summary>
        /// <param name="cargo">The cargo to free up space for.</param>
        /// <remarks>
        /// This method subtracts the weight of the provided cargo from the occupied space of the cargo storage.
        /// If the occupied space becomes less than or equal to the capacity of the cargo storage, it marks the storage as available.
        /// </remarks>

        internal void deOccupySpace(Cargo cargo)
        {
            if (OccupiedSpace <= Capacity && OccupiedSpace > 0)
            {
                OccupiedSpace -= 1;
                IsAvailable = true;
            }

        }
        /// <summary>
        /// Returns the occupied space in the cargo storage based on the weight of the stored cargo.
        /// </summary>
        /// <returns>The total weight of the cargo stored in the cargo storage.</returns>
        public double GetOccupiedSpace()
        {       
            return OccupiedSpace;
        }
    }
}
