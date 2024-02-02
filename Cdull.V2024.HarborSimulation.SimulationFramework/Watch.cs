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
        public DateTime EndTime { get; set; }
        private bool IsCounting { get; set; }   
        public Watch(DateTime watchStartTime, DateTime watchEndTime) {
            this.StartTime = watchStartTime;
            this.EndTime = watchEndTime;
            this.IsCounting = false; 
        }

        /// <summary>
        /// A method that starts a timer.
        /// </summary>
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

        /// <summary>
        /// A method that stops the timer if its already been started, if a timer never was started nothing happens.
        /// </summary>
        public void StopCountingTime()
        {
            if (IsCounting)
            {
                EndTime = DateTime.Now;
                IsCounting = false;
            }
            else
            {
                Console.WriteLine("time is currently not counting");
            }
        }

        /// <summary>
        /// A method to calculate how much time has gone by between when you started the timer and when it stopped.
        /// </summary>
        /// <returns>Returns time</returns>
        public TimeSpan MeasureTimeElapsed()
        {
            TimeSpan elapsedTime = this.EndTime - this.StartTime;
            return elapsedTime; 
        }
    }
}
