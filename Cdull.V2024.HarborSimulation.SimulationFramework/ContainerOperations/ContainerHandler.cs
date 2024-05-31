using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;
using System.Text;

namespace Cdull.V2024.HarborSimulation.SimulationFramework.ContainerOperations
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
        internal void MoveContainerFromShipToAGV(Ship ship, Container container, Harbor harbor)
        {
            Dock dock = ship.GetDockedAt;
            Crane crane = dock.GetCranes.First();
            if (!crane.GetIsAvailable)
            {
                foreach (var c in dock.GetCranes)
                {
                    if (dock.GetAvailableCrane(c))
                    {
                        crane = c;
                    }
                }
            }

            AGV agv = harbor.GetAvailableAGV();
            crane.SetIsAvailable(false); 

            if (crane.GetHandlingTime == 30)
            {

                ship.GetContainers.Remove(container);
                crane.LiftContainer(container);
                container = crane.UnloadContainer(); 
                agv.LoadContainerToAGV(container);
                container.AddToHistory($"{container.GetName} moved at {harbor.GetCurrentTime} from Ship {ship.GetName} to AGV {agv.GetId}");
                crane.SetIsAvailable(true);
                crane.SetHandlingTime(0);
               
            }
            else
            {
                int updatedHandlingTime = crane.GetHandlingTime + 1;
                crane.SetHandlingTime(updatedHandlingTime);
            }

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

        internal void MoveContainerFromAGVToStorageColumn(Harbor harbor, int startColumnId,
            int endColumnId, AGV agv)
        {
            StorageColumn column = harbor.GetContainerStorage.GetSpecificColumn(startColumnId);
            PortalCrane portalCrane = column.GetCrane;
            portalCrane.SetIsAvailable(false);



            portalCrane.SetHandlingTime(portalCrane.GetHandlingTime + 1);
            Container container = agv.GetContainer;

            if (column.GetCapacity == column.GetOccupiedSpace())
            {
                column = harbor.GetContainerStorage.GetSpecificColumn(startColumnId++);
            }
            else if (column.GetId < startColumnId || column.GetId > endColumnId)
            {
                throw new ArgumentOutOfRangeException("columnId", "Column ID is outside the valid range.");
            }


            else if (column.GetCapacity - column.GetOccupiedSpace() < 1)
            {
                throw new InsufficientStorageSpaceException(column.GetId, column.GetCapacity - column.GetOccupiedSpace(), 1);
            }

            if (portalCrane.GetHandlingTime == 30)
            {
                Console.WriteLine("TESTING" + column.GetId);
                portalCrane.LiftContainer(container);
                column.AddContainer(container);
                column.OccupySpace(container);
                agv.SetContainer(null);
                portalCrane.SetContainer(null);
                container.AddToHistory($"{container.GetName} moved at {harbor.GetCurrentTime} from AGV {agv.GetId} to StorageColumn {column.GetId}");
                portalCrane.SetIsAvailable(true);
                portalCrane.SetHandlingTime(0);
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

        internal AGV MoveContainerFromStorageColumnToAGV(int columnId, Harbor harbor)
        {
            StorageColumn column = harbor.GetContainerStorage.GetSpecificColumn(columnId);
            PortalCrane portalCrane = column.GetCrane;
            portalCrane.SetIsAvailable(false);
            AGV agv = harbor.GetAvailableAGV();

            if (column.GetContainers.Any())
            {

                portalCrane.SetHandlingTime(portalCrane.GetHandlingTime + 1);
                Container container = column.GetContainers.First();

                if (portalCrane.GetHandlingTime == 60)
                {
                    column.RemoveContainer(container);
                    column.deOccupySpace(container, harbor);
                    portalCrane.LiftContainer(container);
                    portalCrane.SetContainer(null);
                    agv.LoadContainerToAGV(container);
                    portalCrane.SetIsAvailable(true);
                    container.AddToHistory($"{container.GetName} moved at {harbor.GetCurrentTime} from StorageColumn {column.GetId} to AGV {agv.GetId}");
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
        internal void MoveContainerFromAGVToShip(Harbor harbor, AGV agv, Ship ship)
        {
            Dock dock = ship.GetDockedAt;
            Crane crane = dock.GetCranes.First();
            if (!crane.GetIsAvailable)
            {
                foreach (var c in dock.GetCranes)
                {
                    if (dock.GetAvailableCrane(c))
                    {
                        crane = c;
                        break;
                    }
                }
            }

          
            crane.SetHandlingTime(crane.GetHandlingTime + 1);
            Container container = agv.GetContainer;

            if (crane.GetHandlingTime == 30)
            {
                agv.SetContainer(null);
                crane.LiftContainer(container);
                crane.SetContainer(null);
                ship.GetContainers.Add(container);
                ship.SetIsReadyToSail(true);
                crane.SetIsAvailable(true);
                container.AddToHistory($"{container.GetName} moved at {harbor.GetCurrentTime} from AGV {agv.GetId} to Ship {ship.GetName}");

            }

        }

        /// <summary>
        /// Removes a specified percentage of containers from either a ship or a storage column.
        /// </summary>
        /// <param name="percentageDecimal">The percentage of containers to be removed.</param>
        /// <param name="ship">The ship from which containers are being removed.</param>
        /// <param name="storageColumn">The storage column from which containers are being removed.</param>
        internal void RemovePercentageOfContainersFromSource(decimal percentageDecimal, Ship ship = null, StorageColumn storageColumn = null)
        {
            int percentage = (int)(1 - percentageDecimal);

            if (ship == null && storageColumn == null)
            {
                throw new ArgumentException("Both Ship and StorageColumn cannot be null.");
            }

            if (ship != null)
            {
                int numberOfContainersToRemove = (int)(ship.GetContainers.Count * percentageDecimal);

                for (int i = 0; i < numberOfContainersToRemove; i++)
                {
                    if (ship.GetContainers.Any())
                    {
                        Container container = ship.GetContainers[0];
                        ship.GetContainers.RemoveAt(0);
                        container.AddToHistory($"{container.GetName} was picked up by a truck, and left the harbor.");
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else if (storageColumn != null)
            {
                int numberOfContainersToRemove = (int)(storageColumn.GetContainers.Count * percentageDecimal);

                for (int i = 0; i < numberOfContainersToRemove; i++)
                {
                    if (storageColumn.GetContainers.Any())
                    {
                        storageColumn.GetContainers.RemoveAt(0);
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
        public string CheckScheduledContainerHandling(Ship ship)
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
