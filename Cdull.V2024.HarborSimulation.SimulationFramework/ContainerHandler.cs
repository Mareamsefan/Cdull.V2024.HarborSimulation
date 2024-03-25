using Cdull.V2024.HarborSimulation.SimulationFramework.Enums;
using Cdull.V2024.HarborSimulation.SimulationFramework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class ContainerHandler
    {
        private static readonly ContainerHandler instance = new ContainerHandler();
        private Dictionary<(Ship, DateTime), List<(int, int, int, LoadingType)>> ScheduledContainerHandling = new Dictionary<(Ship, DateTime), List<(int, int, int, LoadingType)>>();


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
        /// Adds container from docked ships to the container storage in the harbor.
        /// </summary>
        /// <returns>True if container is successfully added to the storage; otherwise, false.</returns>
        /// <remarks>
        /// This method checks for docked ships and unloads container from them into the container storage.
        /// It removes container items from the ships and adds them to the container storage.
        /// The method also raises an event when unloading is completed for a ship.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown when there are no docked ships in the harbor.</exception>
        /// <exception cref="StorageSpaceException">Thrown when there is not enough space in the container storage to add all container from the ship.</exception>
        internal bool AddContainerToStorage(Ship ship, Harbor harbor, int startColumnId, int endColumnId)
        {
            if (ship == null)
            {
                throw new ArgumentNullException(nameof(ship), "Ship cannot be null.");
            }

            if (!harbor.DockedShips.Contains(ship))
            {
                Console.WriteLine(harbor.DockedShips.Count);
                Console.WriteLine(harbor.WaitingShips.Count);
                throw new ArgumentException("Ship is not docked in the harbor.", nameof(ship));
            }

            if (ship.Containers.Any() && !ship.IsUnloadingCompleted)
            {
                int totalContainerCount = ship.Containers.Count;
                int occupiedSpace = 0;

                // Beregn totalt antall opptatt plass i kolonnene som skal tømmes
                for (int columnId = startColumnId; columnId <= endColumnId; columnId++)
                {
                    StorageColumn storageColumn = harbor.ContainerStorage.GetSpecificColumn(columnId);
                    occupiedSpace += storageColumn.GetOccupiedSpace();
                }

                // Sjekk om det er nok plass i containerlagringen for alle containerne som skal tømmes
                if (occupiedSpace + totalContainerCount <= harbor.ContainerStorage.Capacity)
                {
                  
                    // Tøm containerne fra skipet og legg dem til i containerlagringen
                    foreach (Container container in ship.Containers.ToList())
                    {
                        StorageColumn storageColumn = harbor.ContainerStorage.GetSpecificColumn(startColumnId);

                        for (int columnId = startColumnId; columnId <= endColumnId; columnId++)
                        { 
                            storageColumn.AddContainer(container);
                            storageColumn.OccupySpace(container);          
                        }

                        // Fjern containeren fra skipet
                        ship.Containers.Remove(container);
                    }

                    // Marker at tømming av skipet er fullført
                    ship.IsUnloadingCompleted = true;

                    // Utløs hendelse om at tømming av skipet er fullført
                    harbor.RaiseShipCompletedUnloading(ship);

                    return true;
                }
                else
                {
                    // Beregn hvor mye plass som er igjen i den første lagringskolonnen
                    int remainingSpace = harbor.ContainerStorage.GetSpecificColumn(startColumnId).Capacity
                                        - harbor.ContainerStorage.GetSpecificColumn(startColumnId).GetOccupiedSpace();

                    // Beregn hvor mange containere som ikke kan lastes inn i den første lagringskolonnen
                    int remainingContainers = totalContainerCount;

                    // Kast InsufficientStorageSpaceException med detaljert informasjon
                    throw new InsufficientStorageSpaceException(endColumnId, remainingSpace, remainingContainers);
                }
            }

            return false;
        }



        /// <summary>
        /// Adds container from the container storage to the docked ships.
        /// </summary>
        /// <param name="numberOfContainers">The maximum number of container items to add to each ship.</param>
        /// <remarks>
        /// This method iterates through all docked ships and attempts to add container to each ship up to the specified limit.
        /// It removes container items from the container storage and adds them to the ship's container list.
        /// The method also updates container history with Loading information.
        /// </remarks>
        /// <exception cref="ArgumentException">Thrown when the number of container to add is not greater than zero.</exception>
        /// <exception cref="InsufficientContainerException">Thrown when there is not enough container in the container storage.</exception>
        internal void AddContainerToShip(Ship ship, int numberOfContainers, Harbor harbor, int startColumnId, int endColumnId)
        {
            if (numberOfContainers <= 0)
            {
                throw new ArgumentException("Number of containers to add must be greater than zero.");
            }

            if (ship == null)
            {
                throw new ArgumentNullException(nameof(ship), "Ship parameter cannot be null.");
            }

            if (ship.Containers.Count < numberOfContainers)
            {
                for (int i = 0; i < numberOfContainers; i++)
                {
                    bool addedContainer = false;

                    // Gå gjennom kolonnene fra endColumnId til endColumnId
                    for (int columnId = startColumnId; columnId <= endColumnId; columnId++)
                    {
                        StorageColumn storageColumn = harbor.ContainerStorage.GetSpecificColumn(columnId);

                        // Sjekk om det er nok containere i kolonnen og om skipet kan ta imot flere containere
                        if (storageColumn.Containers.Count > 0 && !ship.IsLoadingCompleted)
                        {
                            Container container = storageColumn.Containers.First();
                            ship.Containers.Add(container);
                            storageColumn.RemoveContainer(container);
                            storageColumn.deOccupySpace(container, harbor);
                            container.History.Add($"{container.Name} loaded at {harbor.CurrentTime} on {ship.Name}");

                            addedContainer = true;
                            break; // Avslutt løkken når en container er lagt til i skipet
                        }
                    }

                    // Kast unntak hvis det ikke ble lagt til noen container i skipet
                    if (!addedContainer)
                    {
                        throw new InsufficientContainerException("Not enough containers in cargostorage");
                    }
                }

                ship.IsReadyToSail = true;
                ship.IsLoadingCompleted = true;

                // Utløs hendelse hvis lasting av skipet er fullført og tømming av skipet også er fullført
                if (ship.IsUnloadingCompleted)
                {
                    harbor.RaiseShipCompletedLoading(ship);
                }
            }
        }


        public void ScheduleContainerHandling(Ship ship, ContainerStorage containerStorage, DateTime handlingTime, int startColumnId,
        int endColumnId, int numberofContainers, LoadingType loadingType, Harbor harbor)
        {
            // Sjekk for ugyldige parametere
            if (handlingTime < harbor.CurrentTime)
            {
                throw new ArgumentException("Handling time cannot be in the past.");
            }

            if (startColumnId < 0 || endColumnId < 0)
            {
                throw new ArgumentException("Column IDs cannot be negative.");
            }

            if (startColumnId > endColumnId)
            {
                throw new ArgumentException("Start column ID cannot be greater than end column ID.");
            }

            if (numberofContainers > ship.Containers.Count)
            {
                throw new ArgumentException("Cannot unload more containers than what is available on the ship.");
            }


            // Gå gjennom alle kolonner i området og planlegg containerhåndtering
            for (int columnId = startColumnId; columnId <= endColumnId; columnId++)
            {
                // Finn riktig kolonne i lagringen
                StorageColumn storageColumn = containerStorage.GetSpecificColumn(columnId);

                // Sjekk om kolonnen eksisterer
                if (storageColumn == null)
                {
                    throw new ArgumentException($"Storage column with ID {columnId} not found.");
                }

                // Opprett en unik nøkkel for hvert skip og hver kolonne
                var key = (ship, handlingTime);

                // Legg til den planlagte håndteringen i dictionarien
                if (!ScheduledContainerHandling.ContainsKey(key))
                {
                    ScheduledContainerHandling[key] = [(startColumnId, endColumnId, numberofContainers, loadingType)];
                }
            }
        }
        internal void PerformScheduledContainerHandling(Harbor harbor)
        {
            // Gå gjennom alle nøklene i dictionaryen
            foreach (var key in ScheduledContainerHandling.Keys.ToList()) // Kopierer nøklene for å unngå endring under iterering
            {
                Ship ship = key.Item1;
                DateTime handlingTime = key.Item2;
                var handlingInfo = ScheduledContainerHandling[key];

                // Sjekk om handlingstiden allerede har passert
                if (handlingTime <= harbor.CurrentTime)
                {
                    // Utfør containerhåndteringen
                    foreach (var info in handlingInfo)
                    {
                        int startColumnId = info.Item1;
                        int endColumnId = info.Item2;
                        int numberOfContainers = info.Item3;
                        LoadingType loadingType = info.Item4;

                        // Utfør containerhåndteringen for hvert segment av kolonnene
                        for (int columnId = startColumnId; columnId <= endColumnId; columnId++)
                        {
                            if (loadingType == LoadingType.Load)
                            {
                                AddContainerToShip(ship, numberOfContainers, harbor, startColumnId, endColumnId); 
                            }
                            else if (loadingType == LoadingType.Unload)
                            {
                                AddContainerToStorage(ship, harbor, startColumnId, endColumnId); 
                            }
                        }

                        // Fjern handlingen fra dictionaryen etter at den er utført
                        ScheduledContainerHandling.Remove(key);
                    }
                }
            }
        }



        public List<(int, int, int, DateTime, LoadingType)> CheckScheduledCargoHandling(Ship ship)
        {
            // Opprett en liste for å lagre informasjonen om planlagte containerhåndteringer for skipet
            List<(int, int, int,  DateTime, LoadingType)> scheduledHandlingInfo = new List<(int, int, int, DateTime, LoadingType)>();

            // Gå gjennom alle nøklene i dictionaryen
            foreach (var key in ScheduledContainerHandling.Keys)
            {
                Ship keyShip = key.Item1; 
                // Hent ut skipet fra nøkkelen
                foreach(var value in ScheduledContainerHandling[key]){

                    // Sjekk om skipet fra nøkkelen er det samme som det gitte skipet
                    if (keyShip == ship)
                    {
                        // Hvis ja, legg til all informasjonen om planlagt containerhåndtering for dette skipet
                        scheduledHandlingInfo.Add((value.Item2, value.Item1, value.Item3, key.Item2, value.Item4));
                    }
                }
            }
            // Returner listen med informasjon om planlagte containerhåndteringer
            return scheduledHandlingInfo;
        }



    }
}
