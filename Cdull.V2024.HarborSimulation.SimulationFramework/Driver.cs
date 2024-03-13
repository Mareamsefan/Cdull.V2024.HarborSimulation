using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    internal class Driver
    {

        private float Speed;
        internal int CurrentLocation { get; set; }
        internal int DestinationLocation { get; set; }
        internal bool HasReachedDestination { get; set; }
        internal int tid { get; set; }


        public Driver()
        {

            CurrentLocation = 0;

        }


        /// <summary>
        /// Moves the ship towards its destination.
        /// </summary>
        /// <remarks>
        /// This method calculates the new location of the ship based on its speed and updates its current location.
        /// If the ship reaches its destination, the HasReachedDestination property is set to true.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown when the speed of the ship is negative.</exception>
        internal bool Move(int range, float speed)
        {
            if (HasReachedDestination)
            {
                return false; // Stopper metoden hvis destinasjonen allerede er nådd
            }

            var distance = range - CurrentLocation;


            tid++; // Øker tiden med ett sekund for simulering

            if (CurrentLocation >= range)
            {
                HasReachedDestination = true;
                tid = 0;
            }
            else
            {
                // Oppdaterer posisjonen basert på hastighet
                // Her bruker vi tid i sekunder
                CurrentLocation += (int)(speed / 3.6f);
            }

            return !HasReachedDestination; // Fortsetter simuleringen hvis destinasjonen ikke er nådd ennå
        }


    }
}
