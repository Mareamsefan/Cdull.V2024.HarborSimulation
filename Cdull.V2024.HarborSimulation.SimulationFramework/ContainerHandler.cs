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

        public AGV MoveContainerFromShipToAGV(Ship ship, Container container,  Harbor harbor)
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

            if (crane.handlingTime == 30) {
            
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
        public void MoveContainerFromAGVToStorageColumn(ContainerStorage containerStorage, int startColumnId,
            int endColumnId, AGV agv)
        {
            StorageColumn column = containerStorage.GetSpecificColumn(startColumnId);
            PortalCrane portalCrane = column.Crane;
            portalCrane.IsAvailable = false;
            // Sjekk om AGV har last
     
            portalCrane.handlingTime++;
            Container container = agv.Container; // Få tak i containeren fra AGV-en

            if (column.Capacity == column.OccupiedSpace)
            {
                column = containerStorage.GetSpecificColumn(startColumnId++);
            }
            else if (column.ColumnId < startColumnId || column.ColumnId > endColumnId)
            {
                    throw new ArgumentOutOfRangeException("columnId", "Column ID is outside the valid range.");

            }
                // Fjern containeren fra AGV-en og legg til i lagringskolonnen
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
                Console.WriteLine("Denne kjører");
            }
            
           
        }
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
                throw new InvalidOperationException("Unable to move container from StorageColumn to AGV: No container available in the storage column.");
            }
          
        }

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
                crane.IsAvailable = true;
                container.History.Add($"{container.Name} moved from AGV {agv.Id} to Ship {ship.Name}");
              
            }
            
         
        }


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
                        ship.Containers.RemoveAt(0); // Fjerner det første elementet i listen
                        container.History.Add($"{container.Name} was picked up by a truck, and left the harbor.");
                    }
                    else
                    {
                        break; // Avslutter løkken hvis det ikke er flere containere igjen i skipet
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
                        storageColumn.Containers.RemoveAt(0); // Fjerner det første elementet i listen
                    }
                    else
                    {
                        break; // Avslutter løkken hvis det ikke er flere containere igjen i kolonnen
                    }
                }
            }
        }



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


        public string CheckScheduledCargoHandling(Ship ship)
        {
            StringBuilder sb = new StringBuilder();

            // Legg til informasjon om hver planlagte containerbehandling for skipet
            foreach (var handling in ship.ScheduledContainerHandlings)
            {
                sb.AppendLine($"Handling time: {handling.HandlingTime}");
                sb.AppendLine($"Start column ID: {handling.StartColumnId}");
                sb.AppendLine($"End column ID: {handling.EndColumnId}");
                sb.AppendLine($"Number of containers: {handling.NumberOfContainers}");
                sb.AppendLine($"Loading type: {handling.LoadingType}");
                sb.AppendLine(); // Legg til en tom linje mellom hver håndtering
            }

            return sb.ToString();
        }
    } 
}
