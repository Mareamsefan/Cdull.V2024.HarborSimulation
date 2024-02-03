using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using static Cdull.V2024.HarborSimulation.SimulationFramework.Enums;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Watch
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        private bool IsCounting { get; set; }
        public DateTime CurrentTime { get; private set; }

        public Watch(DateTime watchStartTime, DateTime watchEndTime)
        {
            this.StartTime = watchStartTime;
            this.EndTime = watchEndTime;
            this.IsCounting = false;
            this.CurrentTime = watchStartTime; // Start med StartTime
        }

        public void AddTime(TimeSpan timeToAdd)
        {
            if (IsCounting)
            {
                // Legg til tid i CurrentTime
                CurrentTime = CurrentTime.Add(timeToAdd);

                // Legg til tid i EndTime hvis den ikke er null
                if (CurrentTime >= EndTime)
                {
                    Console.WriteLine("The simulation has ended.");
                    IsCounting = false;
                }

                Console.WriteLine($"{timeToAdd} has passed from the start time. Current time is: {CurrentTime}");
            }
            else
            {
                Console.WriteLine("Timer is not running. Start the timer first.");
            }
        }

        public void StartCountingTime()
        {
            if (!IsCounting)
            {
                IsCounting = true;

                Task.Run(async () =>
                {
                    while (IsCounting)
                    {
                        await Task.Delay(1000);
                        AddTime(TimeSpan.FromSeconds(1)); // Legg til 1 sekund i gangen
                        Console.WriteLine($"Current time: {CurrentTime}");
                    }
                });
            }
            else
            {
                Console.WriteLine("Timer is already counting.");
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
                Console.WriteLine("Time is currently not counting.");
            }
            return EndTime;
        }

        public TimeSpan MeasureTimeElapsed()
        {
            return CurrentTime - StartTime;
        }

        public void TimeBasedOnSize(Size size)
        {
            if (size == Enums.Size.Small)
            {
                TimeSpan minutesToAdd = TimeSpan.FromMinutes(2);
                AddTime(minutesToAdd);
             
            }
            else if (size == Enums.Size.Medium)
            {
                TimeSpan minutesToAdd = TimeSpan.FromMinutes(5);
                AddTime(minutesToAdd);
          
            }
            else
            {
                TimeSpan minutesToAdd = TimeSpan.FromMinutes(8);
                AddTime(minutesToAdd);
          
            }
        }
    }

}
