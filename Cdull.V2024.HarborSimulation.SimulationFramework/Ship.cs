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

        //Gjort public
        public string Name {  get; }
        //GJort public
        public ShipType Type {  get; }
        public Size Size { get; }
        public bool HasDocked { get; set; }
        public List<Cargo> Cargo { get; } = new List<Cargo>();
        public List<String> History { get; } = new List<String>();
        public Dock? DockedBy { get; set; }
        public Ship(string shipName, ShipType shipType, Size shipSize) {
            this.Name = shipName;   
            this.Type = shipType;
            this.Size = shipSize;
            this.HasDocked = false;
            this.DockedBy = null; 
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
                this.Cargo.Add(cargo);
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
