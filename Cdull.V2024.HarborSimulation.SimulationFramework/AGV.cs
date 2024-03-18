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
        internal List<String> AVGHistory { get; set; } = new List<String>();

        public AVG(string Id, double speed)
        {
            Id = AVGId;
            speed = AVGSpeed;
        }


        public List<string> GetAVGHistory()
        {
            return AVGHistory;
        }


    }
