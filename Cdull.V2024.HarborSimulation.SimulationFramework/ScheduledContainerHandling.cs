﻿using Cdull.V2024.HarborSimulation.SimulationFramework.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{

    /// <summary>
    /// Represents a scheduled container handling event in the harbor simulation.
    /// </summary>
    public class ScheduledContainerHandling
    {
        internal DateTime HandlingTime {  get; set; } 
        internal int StartColumnId {  get; set; }
        internal int EndColumnId { get; set; }
        internal int NumberOfContainers { get; set; }

        internal LoadingType LoadingType { get; set; }



        /// <summary>
        /// Initializes a new instance of the ScheduledContainerHandling class with the specified parameters.
        /// </summary>
        /// <param name="handlingDateTime">The date and time when the handling is scheduled.</param>
        /// <param name="startColumnId">The ID of the start column for the handling.</param>
        /// <param name="endColumnId">The ID of the end column for the handling.</param>
        /// <param name="numberOfContainers">The number of containers involved in the handling.</param>
        /// <param name="handlingLoadingType">The type of loading for the handling.</param>
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
