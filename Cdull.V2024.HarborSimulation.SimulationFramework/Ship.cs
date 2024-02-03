using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Cdull.V2024.HarborSimulation.SimulationFramework.Enums;
// spør marius om det er ok at man henter enums slik og at klassen med enums er public

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Ship
    {

        //kanskje internal? 
        internal string Name {  get; }
        //GJort public
        internal ShipType Type {  get; }
        internal Size Size { get; }
        internal bool HasDocked { get; set; }
        internal List<Cargo> Cargo { get; } = new List<Cargo>();
        internal List<String> History { get; } = new List<String>();
        internal bool isSailing { get; set; }
        internal Dock? DockedBy { get; set; }

        public Ship(string shipName, ShipType shipType, Size shipSize) {
            Name = shipName;   
            Type = shipType;
            Size = shipSize;
            HasDocked = false;
            DockedBy = null;
            isSailing = false;
        }

        /// <summary>
        /// A method to create cargo in the simulation.
        /// </summary>
        /// <param name="number">The amount of cargo you want to create.</param>
        /// <param name="weight">The weight of each cargo you're creating.</param>
        public void InitializeCargo(int number, double weight = 10)
        {
            for (int i = 0; i <= number; i++)
            {
                Cargo cargo = new($"cargo{i}", weight);
                Cargo.Add(cargo);
            }

        }

        /// <summary>
        /// A method that returns the name of the ship.
        /// </summary>
        /// <returns>Name of the ship.</returns>
        public override String ToString()
        {
            return Name;
        }


    }
}
