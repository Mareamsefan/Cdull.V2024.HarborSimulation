using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    internal class StorageColumns
    {
        public int ColumnId;
        internal int Height { get; private set; }
        internal int Length { get; private set; }
        internal int Width { get; private set; }
        public int Capacity { get; set; }
        internal List<Cargo> Cargo {  get; private set; }
        internal double OccupiedSpace { get; set; }
        internal bool IsAvailable { get; set; }

        public StorageColumns(int columnId, int columnLength, int columnWidth, int columnHeight)
        {
            ColumnId = columnId;
            Length = columnLength;
            Width = columnWidth;
            Height = columnHeight;
            IsAvailable = true;

            // Calculate total capacity based on length, width, and height
            Capacity = Length * Width * Height;
        }

        /// <summary>
        /// Adds the specified cargo to the cargo storage.
        /// </summary>
        /// <param name="cargo">The cargo object to be added.</param>
        /// <remarks>
        /// This method adds the specified cargo to the cargo storage if it is not already present.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when the cargo parameter is null.</exception>
        internal void AddCargo(Cargo cargo)
        {
            if (cargo == null)
            {
                throw new ArgumentNullException(nameof(cargo), "Cargo parameter cannot be null.");
            }

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
        /// <exception cref="ArgumentNullException">Thrown when the cargo parameter is null.</exception>
        internal void RemoveCargo(Cargo cargo)
        {
            if (cargo == null)
            {
                throw new ArgumentNullException(nameof(cargo), "Cargo parameter cannot be null.");
            }

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
        /// <exception cref="InvalidOperationException">Thrown when the cargo storage capacity is exceeded.</exception>
        internal void OccupySpace(Cargo cargo)
        {
            if (OccupiedSpace < Capacity)
            {
                OccupiedSpace += 1;
            }
            else
            {
                IsAvailable = false;
                throw new InvalidOperationException("Cargo storage capacity exceeded.");
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
        /// <exception cref="InvalidOperationException">Thrown when the occupied space or capacity is invalid.</exception>
        internal void deOccupySpace(Cargo cargo)
        {
            if (OccupiedSpace <= Capacity && OccupiedSpace > 0)
            {
                OccupiedSpace -= 1;
                IsAvailable = true;
            }
            else
            {
                throw new InvalidOperationException("Invalid occupied space or capacity.");
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
