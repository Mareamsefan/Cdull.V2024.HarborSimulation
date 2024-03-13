using System;
using System.Collections;
using System.Collections.Generic;
using Cdull.V2024.HarborSimulation.SimulationFramework;
using Cdull.V2024.HarborSimulation.SimulationFramework.Enums;
using Cdull.V2024.HarborSimulation.SimulationFramework.Events;
using Cdull.V2024.HarborSimulation.SimulationFramework.Exceptions;
using Cdull.V2024.HarborSimulation.TestClient;

namespace HarborSimulationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Oppretter en ny havn med navnet "ExceptiontestHarbor" og en lastelager for gods med kapasitet på 10000 enheter.
            Harbor harbor = new Harbor("ExceptiontestHarbor", new CargoStorage("cargo", 10000));

            // Oppretter en liste over dokker ved å initialisere 10 dokker for container skip av stor størrelse.
            List<Dock> docks = harbor.InitializeDocks(10, Model.ContainerShip, Size.Large, 2);

            // Oppretter en liste over skip.
            List<Ship> ships = new List<Ship>();

            // Legger til 5 skip av typen ContainerShip og størrelse stor, med lastekapasitet på 100 enheter.
            ships.AddRange(harbor.InitializeShips(5, Model.ContainerShip, Size.Large, 100));

            // Legger til 5 skip av typen LNGCarrier og størrelse medium, med lastekapasitet på 50 enheter.
            ships.AddRange(harbor.InitializeShips(5, Model.LNGCarrier, Size.Medium, 50));

            // Oppretter en instans av simuleringen.
            IHarborSimulation driver = new Simulation();

            // Setter starttid og sluttid for simuleringen.
            DateTime startTime = new DateTime(2024, 1, 1);
            DateTime endTime = new DateTime(2024, 1, 15);

            // Setter starttidspunkt for seiling for alle skip i havnen som ikke har en gjentakende seilingsplan.
            DateTime startSailingTime = new DateTime(2024, 1, 2);

            // Legger til hendelsesbehandlere for avgang, ankomst, fullført lossing og fullført lasting i havnen.
            harbor.DepartedShip += Harbor_ShipDeparted;
            harbor.ArrivedShip += Harbor_ShipArrived;
            harbor.CompletedUnloadingShip += Harbor_ShipCompletedUnloading;
            harbor.CompletedloadingShip += Harbor_ShipCompletedLoading;

            // Får en instans av seiling.
            Sailing sailing = Sailing.GetInstance();

            // Planlegger seiling for container skip med starttidspunkt 2024-01-02, antall skip 50, og med ukentlig gjentakelse.
            sailing.ScheduleSailing(harbor, Model.ContainerShip, new DateTime(2024, 1, 2), 50, RecurringType.Weekly);

            // Planlegger seiling for LNGCarrier skip med starttidspunkt 2024-01-02, antall skip 40, og med daglig gjentakelse.
            sailing.ScheduleSailing(harbor, Model.LNGCarrier, new DateTime(2024, 1, 2), 40, RecurringType.Daily);

            // Kjører simuleringen.
            driver.Run(harbor, startTime, endTime, ships, docks);

            // Tester at den nye generiske historikk-klassen fungerer for havn og skip.
            HistoryHandler historyHandler = HistoryHandler.GetInstance();

            // Skriver ut historikk for havnen den 2024-01-02.
            Console.WriteLine(historyHandler.GetHarborHistory(new DateTime(2024, 1, 2)));

            // Henter det første skipet i havnen og skriver ut historikk for det.
            Ship ship1 = harbor.GetShips().First();
            Console.WriteLine(historyHandler.GetShipHistory(ship1));

            // Henter det siste skipet i havnen og skriver ut historikk for det.
            Ship ship2 = harbor.GetShips().Last();
            Console.WriteLine(historyHandler.GetShipHistory(ship2));
            Console.WriteLine(ship2.GetShipCargo().Count);

            // Skriver ut historikk for alle skip i havnen.
            Console.WriteLine(historyHandler.GetShipsHistory());
            List<(DateTime, int, RecurringType)> sailings = sailing.CheckScheduledSailings(Model.ContainerShip);

            foreach (var sailing_ in sailings)
            {
                Console.WriteLine(sailing_);
            }
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

            Console.WriteLine($"Ship '{e.CompletedUnloadingShip}' completed unloading cargo.");
        }
        private static void Harbor_ShipCompletedLoading(object? sender, ShipLoadingEventArgs e)
        {

            Console.WriteLine($"Ship '{e.CompletedLoadingShip}' completed loading cargo.");
        }

    }
}
