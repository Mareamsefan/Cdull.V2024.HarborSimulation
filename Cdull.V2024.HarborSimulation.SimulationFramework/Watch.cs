using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Watch
    {
        public DateTime StartTime {  get; set; }
        public DateTime EndTime { get; set; }
        private bool IsCounting { get; set; }

        //NY
        public DateTime CurrentTime { get; private set; }

        public Watch(DateTime watchStartTime, DateTime watchEndTime) {
            this.StartTime = watchStartTime;
            this.EndTime = watchEndTime;
            this.IsCounting = false; 
        }

        public void StartCountingTime(DateTime specificTime)
        {
            if (!IsCounting)
            {
                StartTime = specificTime;
                IsCounting = true;
            }
            else
            {
                Console.WriteLine("time is already counting");
            }
        }



        //Ny (se på etterpå)
        public void AddTime(TimeSpan timeToAdd)
        {
            if (IsCounting)
            {
                // Legg til tid i StartTime
                CurrentTime = StartTime.Add(timeToAdd);

                // Legg til tid i EndTime hvis den ikke er null
                if (EndTime != DateTime.MinValue)
                {
                    EndTime = EndTime.Add(timeToAdd);
                }

                Console.WriteLine($"{timeToAdd} has passed from the start time. Current time is: {CurrentTime}");
            }
            else
            {
                Console.WriteLine("Timer is not running. Start the timer first.");
            }
        }

     

        public DateTime StopCountingTime()
        {
            if (IsCounting)
            {
                EndTime = CurrentTime;
                IsCounting = false;
               
            }
            else
            {
                Console.WriteLine("time is currently not counting");
            }
            return EndTime;
        }

        public TimeSpan MeasureTimeElapsed()
        {
            TimeSpan elapsedTime = this.EndTime - this.StartTime;
            return elapsedTime; 
        }
    }
}
