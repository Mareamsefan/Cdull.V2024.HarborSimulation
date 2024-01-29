using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    internal class Watch
    {
        private DataSetDateTime startTime {  get; set; }
        private DataSetDateTime stopTime { get; set; }
        private bool IsCounting { get; set; }   
        public Watch(DataSetDateTime watchStartTime, DataSetDateTime watchStopTime) {
            this.startTime = watchStartTime;
            this.stopTime = watchStopTime;
            this.IsCounting = false; 
        }
    }
}
