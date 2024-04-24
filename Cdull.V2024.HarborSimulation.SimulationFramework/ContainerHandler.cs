using Cdull.V2024.HarborSimulation.SimulationFramework.Cdull.HarborSimulation.Infastructure;
using Cdull.V2024.HarborSimulation.SimulationFramework.Cdull.HarborSimulation.Infrastructure;
using Cdull.V2024.HarborSimulation.SimulationFramework.Enums;
using Cdull.V2024.HarborSimulation.SimulationFramework.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{

    /// <summary>
    /// Handles the movement and manipulation of containers within the harbor.
    /// </summary>
    public class ContainerHandler
    {
        private static readonly ContainerHandler instance = new ContainerHandler();


        /// <summary>
        /// Private constructor to prevent external instantiation.
        /// </summary>
        private ContainerHandler() { }

        /// <summary>
        /// Gets the singleton instance of the <see cref="HistoryHandler"/> class.
        /// </summary>
        /// <returns>The singleton instance of the <see cref="HistoryHandler"/> class.</returns>
        /// https://csharpindepth.com/articles/singleton (hentet: 03.03.2024) (Skeet.Jon, 2019)
        public static ContainerHandler GetInstance()
        {
            return instance;
        }


        /// <summary>
        /// Moves a container from a ship to an Automated Guided Vehicle (AGV).
        /// </summary>
        /// <param name="ship">The ship from which the container is being moved.</param>
        /// <param name="container">The container being moved.</param>
        /// <param name="harbor">The harbor instance.</param>
        /// <returns>The AGV to which the container is moved.</returns>
        public AGV MoveContainerFromShipToAGV(Ship ship, Container container, Harbor harbor)
        {
            Dock dock = ship.DockedAt;
            Crane crane = dock.Cranes.First();
            if (!crane.IsAvailable)
            {
                foreach (var c in dock.Cranes)
                {
                    if (dock.GetAvailableCrane(c))
                    {
                        crane = c;
                    }
                }
            }

            AGV agv = harbor.GetAvailableAGV();
            crane.IsAvailable = false;

            if (crane.handlingTime == 30)
            {

                ship.Containers.Remove(container);
                crane.LiftContainer(container);
                crane.Container = null;
                agv.LoadContainerToAGV(container);
                container.History.Add($"{container.Name} moved from Ship {ship.Name} to AGV {agv.Id}");
                crane.IsAvailable = true;
                crane.handlingTime = 0;

            }
            crane.handlingTime++;
            return agv;
        }


        /// <summary>
        /// Moves a container from an Automated Guided Vehicle (AGV) to a storage column within the container storage.
        /// </summary>
        /// <param name="containerStorage">The instance of the container storage where the operation takes place.</param>
        /// <param name="startColumnId">The ID of the starting column where the container will be moved.</param>
        /// <param name="endColumnId">The ID of the ending column.</param>
        /// <param name="agv">The AGV from which the container is being moved.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the start column ID is not within the valid range defined by the container storage.</exception>
        /// <exception cref="InsufficientStorageSpaceException">Thrown when there is not enough space in the storage column to add the container.</exception>

        public void MoveContainerFromAGVToStorageColumn(ContainerStorage containerStorage, int startColumnId,
            int endColumnId, AGV agv)
        {
            StorageColumn column = containerStorage.GetSpecificColumn(startColumnId);
            PortalCrane portalCrane = column.Crane;
            portalCrane.IsAvailable = false;


            portalCrane.handlingTime++;
            Container container = agv.Container;

            if (column.Capacity == column.OccupiedSpace)
            {
                column = containerStorage.GetSpecificColumn(startColumnId++);
            }
            else if (column.ColumnId < startColumnId || column.ColumnId > endColumnId)
            {
                throw new ArgumentOutOfRangeException("columnId", "Column ID is outside the valid range.");
            }


            else if (column.Capacity - column.OccupiedSpace < 1)
            {
                throw new InsufficientStorageSpaceException(column.ColumnId, column.Capacity - column.OccupiedSpace, 1);
            }

            if (portalCrane.handlingTime == 60)
            {
                agv.Container = null;
                portalCrane.LiftContainer(container);
                portalCrane.Container = null;
                column.AddContainer(container);
                column.OccupySpace(container);
                container.History.Add($"{container.Name} moved from AGV {agv.Id} to StorageColumn {column.ColumnId}");
                portalCrane.IsAvailable = true;
                portalCrane.handlingTime = 0;
            }
        }

        /// <summary>
        /// Moves a container from a storage column to an Automated Guided Vehicle (AGV).
        /// </summary>
        /// <param name="containerStorage">The instance of the container storage from which the container is being moved.</param>
        /// <param name="columnId">The ID of the column from which the container is being moved.</param>
        /// <param name="harbor">The instance of the harbor where the operation takes place.</param>
        /// <returns>The AGV to which the container is moved.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no container is available in the storage column.</exception>

        public AGV MoveContainerFromStorageColumnToAGV(ContainerStorage containerStorage, int columnId, Harbor harbor)
        {
            StorageColumn column = containerStorage.GetSpecificColumn(columnId);
            PortalCrane portalCrane = column.Crane;
            portalCrane.IsAvailable = false;
            AGV agv = harbor.GetAvailableAGV();

            if (column.Containers.Any())
            {

                portalCrane.handlingTime++;
                Container container = column.Containers.First();

                if (portalCrane.handlingTime == 60)
                {
                    column.RemoveContainer(container);
                    column.deOccupySpace(container, harbor);
                    portalCrane.LiftContainer(container);
                    portalCrane.Container = null;
                    agv.LoadContainerToAGV(container);
                    portalCrane.IsAvailable = true;
                    container.History.Add($"{container.Name} moved from StorageColumn {column.ColumnId} to AGV {agv.Id}");
                }
                return agv;
            }

            else
            {
                throw new InvalidOperationException("Unable to Move container from StorageColumn to AGV: No container available in the storage column.");
            }
        }


        /// <summary>
        /// Moves a container from an AGV to a ship.
        /// </summary>
        /// <param name="agv">The AGV from which the container is being moved.</param>
        /// <param name="ship">The ship to which the container is being moved.</param>
        public void MoveContainerFromAGVToShip(AGV agv, Ship ship)
        {
            Dock dock = ship.DockedAt;
            Crane crane = dock.Cranes.First();
            if (!crane.IsAvailable)
            {
                foreach (var c in dock.Cranes)
                {
                    if (dock.GetAvailableCrane(c))
                    {
                        crane = c;
                        break;
                    }
                }
            }

            crane.handlingTime++;
            Container container = agv.Container;

            if (crane.handlingTime == 30)
            {
                agv.Container = null;
                crane.LiftContainer(container);
                crane.Container = null;
                ship.Containers.Add(container);
                ship.IsReadyToSail = true;
                crane.IsAvailable = true;
                container.History.Add($"{container.Name} moved from AGV {agv.Id} to Ship {ship.Name}");

            }

        }

        /// <summary>
        /// Removes a specified percentage of containers from either a ship or a storage column.
        /// </summary>
        /// <param name="percentageDecimal">The percentage of containers to be removed.</param>
        /// <param name="ship">The ship from which containers are being removed.</param>
        /// <param name="storageColumn">The storage column from which containers are being removed.</param>
        public void RemovePercentageOfContainersFromSource(decimal percentageDecimal, Ship ship = null, StorageColumn storageColumn = null)
        {
            int percentage = (int)(1 - percentageDecimal);

            if (ship == null && storageColumn == null)
            {
                throw new ArgumentException("Both Ship and StorageColumn cannot be null.");
            }

            if (ship != null)
            {
                int numberOfContainersToRemove = (int)(ship.Containers.Count * percentageDecimal);

                for (int i = 0; i < numberOfContainersToRemove; i++)
                {
                    if (ship.Containers.Any())
                    {
                        Container container = ship.Containers[0];
                        ship.Containers.RemoveAt(0);
                        container.History.Add($"{container.Name} was picked up by a truck, and left the harbor.");
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else if (storageColumn != null)
            {
                int numberOfContainersToRemove = (int)(storageColumn.Containers.Count * percentageDecimal);

                for (int i = 0; i < numberOfContainersToRemove; i++)
                {
                    if (storageColumn.Containers.Any())
                    {
                        storageColumn.Containers.RemoveAt(0);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }



        /// <summary>
        /// Schedules container handling operations for a ship.
        /// </summary>
        /// <param name="ship">The ship for which container handling is being scheduled.</param>
        /// <param name="handlingTime">The time at which the handling operation is scheduled.</param>
        /// <param name="startColumnId">The ID of the starting column.</param>
        /// <param name="endColumnId">The ID of the ending column.</param>
        /// <param name="numberofContainers">The number of containers to be handled.</param>
        /// <param name="loadingType">The type of loading (unload/load).</param>
        public void ScheduleContainerHandling(Ship ship, DateTime handlingTime, int startColumnId,
        int endColumnId, int numberofContainers, LoadingType loadingType)
        {

            ScheduledContainerHandling scheduledContainerHandling = new ScheduledContainerHandling(handlingTime, startColumnId,
                endColumnId, numberofContainers, loadingType);

            if (!ship.ScheduledContainerHandlings.Contains(scheduledContainerHandling))
            {
                ship.ScheduledContainerHandlings.Add(scheduledContainerHandling);
            }


        }

        /// <summary>
        /// Checks and returns information about scheduled cargo handling operations for a ship.
        /// </summary>
        /// <param name="ship">The ship for which scheduled cargo handling operations are being checked.</param>
        /// <returns>A string containing information about scheduled cargo handling operations.</returns>
        public string CheckScheduledCargoHandling(Ship ship)
        {
            StringBuilder sb = new StringBuilder();


            foreach (var handling in ship.ScheduledContainerHandlings)
            {
                sb.AppendLine($"Handling time: {handling.HandlingTime}");
                sb.AppendLine($"Start column ID: {handling.StartColumnId}");
                sb.AppendLine($"End column ID: {handling.EndColumnId}");
                sb.AppendLine($"Number of containers: {handling.NumberOfContainers}");
                sb.AppendLine($"Loading type: {handling.LoadingType}");
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
