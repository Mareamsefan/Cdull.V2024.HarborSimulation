﻿using System;
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

        public TimeSpan MeasureTimeElapsed()
        {
            TimeSpan elapsedTime = this.EndTime - this.StartTime;
            return elapsedTime; 
        }
    }
}
