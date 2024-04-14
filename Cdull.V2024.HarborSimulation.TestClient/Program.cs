
using Cdull.V2024.HarborSimulation.SimulationFramework;
using Cdull.V2024.HarborSimulation.SimulationFramework.Enums;
using Cdull.V2024.HarborSimulation.SimulationFramework.Events;
using Cdull.V2024.HarborSimulation.TestClient;

namespace HarborSimulationTest
{
    class Program
    {
        static void Main(string[] args)
        {

            // Lager en overordnet plass for plasssering av largingskolonner, og velger indeksrekkevidde for plasseringen. 
            ContainerStorage containerStorage = new ContainerStorage("ContainerStorage", 0, 1000);
            // Oppretter en ny havn med navnet "ExceptiontestHarbor" og en lastelager for gods med kapasitet på 10000 enheter.
            Harbor harbor = new Harbor("ExceptiontestHarbor", containerStorage);

           //Lager 3 kaiplasser som skal betjenes av 7 kraner:
           //Kaiplassene betjener maks 20 ship og minst 3. 
            List<Dock> docks = harbor.InitializeDocks(2, Size.Large, 3);
            docks.AddRange(harbor.InitializeDocks(1, Size.Large, 1)); 

            //Lager 20 AGV-er som skal flytte på containere: 
            List<AGV> agvs = harbor.InitializeAGVs(20, 1000); 

            // Oppretter en liste over skip.
            List<Ship> ships = new List<Ship>();

            // Legger til 5 skip av typen LNGCarrier og størrelse medium med currentlocation 2000m unna Harbor:
            ships.AddRange(harbor.InitializeShips(2000, 5, Model.LNGCarrier, Size.Medium));

            // Legger til 5 skip av typen ContainerShip, med 5 små containere med currentLocation 1500m unna Harbor: 
            ships.AddRange(harbor.InitializeShips(1500, 5, Model.ContainerShip, Size.Large, 5, ContainerSize.Small));

            // Legger til 5 skip av typen ContainerShip, med 5 store containere med currentlocation 1700m unna Harbor: 
            ships.AddRange(harbor.InitializeShips(1700, 5, Model.ContainerShip, Size.Large, 5, ContainerSize.Large));

            // Lager en test containership som viser at man kan lage et og et ship: 
            Ship ship = new Ship("TestContainerShip", Model.ContainerShip, Size.Small, 1000);

            ship.InitializeContainers(5, ContainerSize.Small);
            
            ships.Add(ship);

         

            // Oppretter en instans av simuleringen.
            IHarborSimulation driver = new Simulation();

            // Setter starttid og sluttid for simuleringen.
            DateTime startTime = new DateTime(2024, 1, 1);
            DateTime endTime = new DateTime(2024, 1, 15);

            // Legger til hendelsesbehandlere for avgang, ankomst, fullført lossing og fullført lasting i havnen.
            harbor.DepartedShip += Harbor_ShipDeparted;
            harbor.ArrivedShip += Harbor_ShipArrived;
            harbor.CompletedUnloadingShip += Harbor_ShipCompletedUnloading;
            harbor.CompletedloadingShip += Harbor_ShipCompletedLoading;

            // Lager en instans av seiling.
            Sailing sailing = Sailing.GetInstance();

            // Planlegger seiling for container skip med starttidspunkt 2024-01-02, antall skip 50, og med ukentlig gjentakelse.
            sailing.ScheduleSailing(harbor, Model.ContainerShip, new DateTime(2024, 1, 2), 50, RecurringType.Weekly);

            // Planlegger seiling for LNGCarrier skip med starttidspunkt 2024-01-02, antall skip 40, og med daglig gjentakelse.
            sailing.ScheduleSailing(harbor, Model.LNGCarrier, new DateTime(2024, 1, 2), 40, RecurringType.Daily);

            List<int> columLocations = new List<int>
            {
                37, 74, 111, 148, 185, 222, 259, 292, 333, 270, 407, 444, 481, 518
            };
            List<StorageColumn> storageColumns = harbor.InitializeStorageColumns();
            //Lager 24 Lange og 7 korte lagringskolonner: 
            var columnsToAdd = new List<StorageColumn>
            {
                new StorageColumn(37, 1, 18, 6, 4),
                new StorageColumn(74, 2, 18, 6, 4),
                new StorageColumn(111, 3, 18, 6, 4),
                new StorageColumn(148, 1, 18, 6, 4),
                new StorageColumn(185, 1, 18, 6, 4),
                new StorageColumn(222, 1, 18, 6, 4),
                new StorageColumn(259, 1, 18, 6, 4),
                new StorageColumn(296, 1, 18, 6, 4),
                new StorageColumn(333, 1, 18, 6, 4),
                new StorageColumn(370, 1, 18, 6, 4),
                new StorageColumn(407, 1, 18, 6, 4),
                new StorageColumn(444, 1, 18, 6, 4),
                new StorageColumn(481, 1, 18, 6, 4),
                new StorageColumn(518, 1, 18, 6, 4),
                new StorageColumn(555, 1, 18, 6, 4),
                new StorageColumn(592, 1, 18, 6, 4),
                new StorageColumn(629, 1, 18, 6, 4),
                new StorageColumn(666, 1, 18, 6, 4),
                new StorageColumn(703, 1, 18, 6, 4),
                new StorageColumn(740, 1, 18, 6, 4),
                new StorageColumn(777, 1, 18, 6, 4),
                new StorageColumn(814, 1, 18, 6, 4),
                new StorageColumn(851, 1, 18, 6, 4),
                new StorageColumn(888, 1, 18, 6, 4),
                new StorageColumn(666, 1, 15, 6, 4),
                new StorageColumn(703, 1, 15, 6, 4),
                new StorageColumn(740, 1, 15, 6, 4),
                new StorageColumn(777, 1, 15, 6, 4),
                new StorageColumn(814, 1, 15, 6, 4),
                new StorageColumn(851, 1, 15, 6, 4),
                new StorageColumn(888, 1, 15, 6, 4),

            };

            foreach (var column in columnsToAdd)
            {
                containerStorage.AddStorageColumn(column);
            }

            
            ContainerHandler containerHandler = ContainerHandler.GetInstance();
          
            containerHandler.ScheduleContainerHandling(ship, new DateTime(2024, 1, 4), 1, 2, 10, LoadingType.Unload);
            containerHandler.ScheduleContainerHandling(ship, new DateTime(2024, 1, 7),  1, 2, 10, LoadingType.Load);
            ships.ForEach(ship =>
            {
                if (ship.Model.Equals(Model.ContainerShip))
                {
                    containerHandler.ScheduleContainerHandling(ship, new DateTime(2024, 1, 6), 2, 3, 10, LoadingType.Unload);
                    containerHandler.ScheduleContainerHandling(ship, new DateTime(2024, 1, 8), 2, 3, 10, LoadingType.Load);
                }
            });
            //Sjekker alle handlingene for test-shipet: 
            Console.WriteLine(containerHandler.CheckScheduledCargoHandling(ship));
         
            // Kjører simuleringen.
            driver.Run(harbor, startTime, endTime, ships, docks, agvs);

            // Tester at den nye generiske historikk-klassen fungerer for havn og skip.
            HistoryHandler historyHandler = HistoryHandler.GetInstance();

            // Skriver ut historikk for havnen den 2024-01-02.
            Console.WriteLine(historyHandler.GetHarborHistory(new DateTime(2024, 1, 2)));

            // Henter det første skipet i havnen og skriver ut historikk for det.
            Ship ship1 = harbor.GetShips().First();
            Console.WriteLine(historyHandler.GetShipHistory(ship1));

            // Henter det siste skipet i havnen og skriver ut historikk for det.
            //Ship ship2 = harbor.GetShips().Last();
            //Console.WriteLine(historyHandler.GetShipHistory(ship2));
         

            // Skriver ut historikk for alle skip i havnen.
            Console.WriteLine(historyHandler.GetShipsHistory());    
            
     
      
        

        }

        private static void Harbor_ShipDeparted(object? sender, ShipDepartureEventArgs e)
        {
            Console.WriteLine($"Ship '{e.DepartedShip}' departed harbor.");
        }

        private static void Harbor_ShipArrived(object? sender, ShipArrivalEventArgs e)
        {

            Console.WriteLine($"Ship '{e.ArrivedShip}' arrived harbor.");
        }

        private static void Harbor_ShipCompletedUnloading(object? sender, ShipUnloadingEventArgs e)
        {

            Console.WriteLine($"Ship '{e.CompletedUnloadingShip}' completed unloading containers." );
        }
        private static void Harbor_ShipCompletedLoading(object? sender, ShipLoadingEventArgs e)
        {

            Console.WriteLine($"Ship '{e.CompletedLoadingShip}' completed Loading containers.");
        }

    }
}
