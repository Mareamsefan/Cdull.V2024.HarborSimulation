using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class AGV
    {
        internal string Id { get; set; }
        internal double speed { get; set; }
        internal List<String> AGVHistory { get; set; } = new List<String>();


        public AGV(string agvId, double agvSpeed)
        {
            Id = agvId;
            speed = agvSpeed;
        }


        public List<string> GetAGVHistory()
        {
            return AGVHistory;
        }


    }
}
