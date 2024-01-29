using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Ship
    {
        private int id {  get; set; }
        private string name {  get; }
        private string model {  get; }
        private string size { get; }
        private bool hasDocked { get; set; }
        private List<Cargo> cargoList { get; } = new List<Cargo>();
        private List<String> historyList { get; set; } = new List<String>();
        private Dock? dockedBy { get; set; }
        public Ship(string shipName, string shipModel, string shipSize, 
            List<Cargo> shipCargoList) {
            this.name = shipName;   
            this.model = shipModel;
            this.size = shipSize;
            this.hasDocked = false;
            this.cargoList = shipCargoList;
            this.dockedBy = null; 
        }


        public void InitializeCargos(int number, double weight)
        {
            for (int i = 0; i < number; i++)
            {
                Cargo cargo = new($"name{i}", weight);
                this.cargoList.Add(cargo);
            }

        }

    }
}
