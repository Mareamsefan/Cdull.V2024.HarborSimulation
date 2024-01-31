using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Ship
    {
        private string Name {  get; }
        //private string Model {  get; }
        public string Size { get; }
        public bool HasDocked { get; set; }
        private List<Cargo> Cargo { get; } = new List<Cargo>();
        public List<String> History { get; } = new List<String>();
        public Dock? DockedBy { get; set; }
        public Ship(string shipName, /*string shipModel*/ string shipSize 
            /*List<Cargo> shipCargoList*/) {
            this.Name = shipName;   
            //this.Model = shipModel;
            this.Size = shipSize;
            this.HasDocked = false;
            //this.Cargo = shipCargoList;
            this.DockedBy = null; 
        }


        public void InitializeCargos(int number, double weight = 10)
        {
            for (int i = 0; i < number; i++)
            {
                Cargo cargo = new($"cargo{i}", weight);
                this.Cargo.Add(cargo);
            }

        }

        public void override()


    }
}
