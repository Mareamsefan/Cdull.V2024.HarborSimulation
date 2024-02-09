namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class CargoStorage
    {
        private string name;
        // kanksje internal? 
        public List<Cargo> Cargo { get; set; } = new List<Cargo>();
        private bool IsAvailable { get; set; }
        public int Capacity { get; }

        public CargoStorage(string cargoStorageName, int cargoStoragecapacity)
        {

            name = cargoStorageName;
            IsAvailable = true;
            Capacity = cargoStoragecapacity;
        }

        /// <summary>
        /// A method to add cargo to the cargo storage in the harbor.
        /// </summary>
        /// <param name="cargo">The cargo object thats being added</param>
        public void AddCargo(Cargo cargo)
        {
            Cargo.Add(cargo);
        }

        /// <summary>
        /// A method to remove the cargor from the cargo storage in the harbor.
        /// </summary>
        /// <param name="cargo">The cargo object thats being removed</param>
        public void RemoveCargo(Cargo cargo)
        {
            Cargo.Remove(cargo);
        }


        public double GetOccupiedSpace()
        {
            double occupiedSpace = 0;
            foreach (var cargo in Cargo)
            {
                occupiedSpace += cargo.Weight; // Antar at lastens vekt representerer størrelsen
            }
            return occupiedSpace;
        }
    }
}
