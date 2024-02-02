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
        private string Name {  get; }
        private ShipType Type {  get; }
        public ShipSize Size { get; }
        public bool HasDocked { get; set; }
        public List<Cargo> Cargo { get; } = new List<Cargo>();
        public List<String> History { get; } = new List<String>();
        public Dock? DockedBy { get; set; }
        public Ship(string shipName, ShipType shipType, ShipSize shipSize) {
            this.Name = shipName;   
            this.Type = shipType;
            this.Size = shipSize;
            this.HasDocked = false;
            this.DockedBy = null; 
        }


        public void InitializeCargos(int number, double weight = 10)
        {
            for (int i = 0; i <= number; i++)
            {
                Cargo cargo = new($"cargo{i}", weight);
                this.Cargo.Add(cargo);
            }

        }

        public override String ToString()
        {
            return Name;
        }


    }
}
