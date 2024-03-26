using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    internal class Driver
    {

        internal int CurrentLocation { get; set; }
        internal bool HasReachedDestination { get; set; }
        internal bool agvHasReachedDestination { get; set; }
        internal int agvSpeed { get; set; }
        internal float move {  get; set; }




        public Driver()
        {

            CurrentLocation = 0;
            HasReachedDestination = false;
            agvHasReachedDestination = false;
            agvSpeed = 7;
           

        }


        internal bool AgvMove(int agvLocation, int moveToLocation)
        {
            float speed = (agvSpeed / 3.6f);
            double calcDistance = Math.Pow(moveToLocation - agvLocation, 2);
            double distance = Math.Sqrt(calcDistance);
            
            if (agvHasReachedDestination)
            {
                return false;
            }

            if (move < distance)
            {
                if (distance > move)
                {
                    move += speed;
                    Console.WriteLine("Move: " + move);
                    Console.WriteLine("speed: " + speed);
                }
            }

            if (move == distance)
            {
                agvHasReachedDestination = true;
            }

            return !agvHasReachedDestination; 
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



            if (CurrentLocation >= range)
            {
                HasReachedDestination = true;
  
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
