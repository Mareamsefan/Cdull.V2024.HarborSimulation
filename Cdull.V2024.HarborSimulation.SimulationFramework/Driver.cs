using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    /// <summary>
    /// Represents a driver responsible for controlling the movement of vehicles/boats in and out of the harbor.
    /// </summary>
    internal class Driver
    {
        internal int CurrentLocation { get; set; }
        internal bool HasReachedDestination { get; set; }
        internal bool AgvHasReachedDestination { get; set; }
        internal int AgvSpeed { get; set; }
        internal float MoveDistance {  get; set; }


        /// <summary>
        /// Initializes a new instance of the Driver class with default values.
        /// </summary>
        public Driver()
        {

            CurrentLocation = 0;
            HasReachedDestination = false;
            AgvHasReachedDestination = false;
            AgvSpeed = 7;
           

        }


        /// <summary>
        /// Moves the AGV towards its destination.
        /// </summary>
        /// <param name="agvLocation">The current location of the AGV.</param>
        /// <param name="moveToLocation">The destination location to Move to.</param>
        /// <returns>True if the AGV has not yet reached its destination, otherwise false.</returns>
        internal bool AgvMove(int agvLocation, int moveToLocation)
        {
            float speed = (AgvSpeed / 3.6f);
            double calcDistance = Math.Pow(moveToLocation - agvLocation, 2);
            double distance = Math.Sqrt(calcDistance);
            
            if (AgvHasReachedDestination)
            {
                return false;
            }

            if (MoveDistance < distance)
            {
                if (distance > MoveDistance)
                {
                    MoveDistance += speed;
                }
            }

            if (MoveDistance == distance)
            {
                AgvHasReachedDestination = true;
            }

            return !AgvHasReachedDestination; 
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
                return false; 
            }

            if (CurrentLocation >= range)
            {
                HasReachedDestination = true;
  
            }
            else
            {
                
                CurrentLocation += (int)(speed / 3.6f);
            }

            return !HasReachedDestination;
        }


    }
}
