using Cdull.V2024.HarborSimulation.SimulationFramework.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class ScheduledContainerHandling
    {
        internal DateTime HandlingTime {  get; set; } 
        internal int StartColumnId {  get; set; }
        internal int EndColumnId { get; set; }
        internal int NumberOfContainers { get; set; }

        internal LoadingType LoadingType { get; set; }


        public ScheduledContainerHandling(DateTime handlingDateTime, int startColumnId, int endColumnId, int numberOfContainers, 
            LoadingType handlingLoadingType)
        {
            HandlingTime = handlingDateTime;
            StartColumnId = startColumnId;
            EndColumnId = endColumnId;
            NumberOfContainers = numberOfContainers;
            LoadingType = handlingLoadingType;
        }
    }
}
