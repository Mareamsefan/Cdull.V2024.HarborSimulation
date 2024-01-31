using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Watch
    {
        public DateTime StartTime {  get; set; }
        public DateTime StopTime { get; set; }
        private bool IsCounting { get; set; }   
        public Watch(DateTime watchStartTime, DateTime watchStopTime) {
            this.StartTime = watchStartTime;
            this.StopTime = watchStopTime;
            this.IsCounting = false; 
        }

        public void StartCountingTime()
        {
            if (!IsCounting)
            {
                StartTime = DateTime.Now;
                IsCounting = true; 
            }
            else
            {
                Console.WriteLine("time is already counting");
            }
        }

        public void StopCountingTime()
        {
            if (IsCounting)
            {
                StopTime = DateTime.Now;
                IsCounting = false;
            }
            else
            {
                Console.WriteLine("time is currently not counting");
            }
        }

        public TimeSpan MeasureTimeElapsed()
        {
            TimeSpan elapsedTime = this.StopTime - this.StartTime;
            return elapsedTime; 
        }
    }
}
