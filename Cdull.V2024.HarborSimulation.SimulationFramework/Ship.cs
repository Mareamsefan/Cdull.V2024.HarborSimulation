using System;
using static Cdull.V2024.HarborSimulation.SimulationFramework.Enums;
// spør marius om det er ok at man henter enums slik og at klassen med enums er public

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Ship
    {
        //kanskje internal? 
        public string Name { get; }
        //GJort public
        internal Model Model { get; }
        internal Size Size { get; }
        internal bool HasDocked { get; set; }
        internal List<Cargo> Cargo { get; } = new List<Cargo>();
        internal string DockedAtTime { get; set; }
        internal string SailedAtTime { get; set; }
        public bool IsSailing { get; set; }
        public bool IsReadyToSail{ get; set; }
        internal bool IsWaitingForSailing { get; set; }
        internal int Speed { get; private set; }
        internal Dock? DockedAt { get; set; }
        public int CurrentLocation { get; set; }
        public int DestinationLocation { get; set; }



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
            Speed = 100;
            IsReadyToSail = false;
            CurrentLocation = GenerateRandomDestination(10, 100);
        }

        public int GenerateRandomDestination(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max + 1);
        }
        /// <summary>
        /// A method to create cargo in the simulation.
        /// </summary>
        /// <param name="number">The amount of cargo you want to create.</param>
        /// <param name="weight">The weight of each cargo you're creating.</param>
        public void InitializeCargo(int number, double weight = 10)
        {
            for (int i = 0; i < number; i++)
            {
                Cargo cargo = new($"cargo{i}", weight);
                Cargo.Add(cargo);
            }

        }


        public int CalculateDistanceToDock(Dock dock)
        {
            return Math.Abs(CurrentLocation - dock.Location);
        }


        // Metode for å sette destinasjonen fra havnen til skipet
        public void SetDestinationFromHarbor(int harborLocation, int destination)
        {
            DestinationLocation = Math.Abs(destination - harborLocation);
        }

      

        public List<Cargo> GetCargo()
        {
            return Cargo;
        }


        public string GetDockedAtTime()
        {
            return DockedAtTime; 

        }


        /// <summary>
        /// A method that returns the name of the ship.
        /// </summary>
        /// <returns>Name of the ship.</returns>
        public override String ToString()
        {
            return "Name: " + Name + " Model: " + Model + " Size: " + Size + " Has docked: " + HasDocked;
        }


    }
}
