using System;
using System.Windows;
using Cdull.V2024.HarborSimulation.SimulationFramework;
using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;
using Cdull.V2024.HarborSimulation.SimulationFramework.ShipOperations;
using System.Collections.Generic;
using System.Threading.Tasks;
using Size = Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure.Size;

namespace HarborSimulationApp
{
    public partial class MainWindow : Window
    {
        private Simulation simulation;

        public MainWindow()
        {
            InitializeComponent();
            simulation = new Simulation();
        }

        private async void StartSimulation_Click(object sender, RoutedEventArgs e)
        {
            

            // Simulate in a background task
            await Task.Run(() =>
            {
                // Setup harbor, ships, docks, etc. (similar to your existing setup code)
                Harbor harbor = SetupHarbor();

                DateTime startTime = new DateTime(2024, 1, 1);
                DateTime endTime = new DateTime(2024, 1, 15);

                List<Ship> ships = SetupShips();
                List<Dock> docks = SetupDocks();
                List<AGV> agvs = SetupAGVs();
                List<StorageColumn> storageColumns = SetupStorageColumns();
                ships.AddRange(harbor.InitializeShips(2000, 5, Model.LNGCarrier, Size.Medium));

                // Run the simulation
                simulation.Run(harbor, startTime, endTime, ships, docks, agvs, storageColumns);

                // Update UI on the main thread
                Application.Current.Dispatcher.Invoke(() =>
                {
                    SimulationStatus.Text = "Simulation completed";
                   
                });
            });
        }

        private Harbor SetupHarbor()
        {
            // Setup your harbor instance here
            ContainerStorage containerStorage = new ContainerStorage("ContainerStorage", 0, 500);
            Harbor harbor = new Harbor("TestHarbor", 1000, containerStorage);
            // Add docks, ships, etc. as needed
            return harbor;
        }

        private List<Ship> SetupShips()
        {
            // Setup your list of ships here
            List<Ship> ships = new List<Ship>();
            // Add ships to the list
            return ships;
        }

        private List<Dock> SetupDocks()
        {
            // Setup your list of docks here
            List<Dock> docks = new List<Dock>();
            // Add docks to the list
            return docks;
        }

        private List<AGV> SetupAGVs()
        {
            // Setup your list of AGVs here
            List<AGV> agvs = new List<AGV>();
            // Add AGVs to the list
            return agvs;
        }

        private List<StorageColumn> SetupStorageColumns()
        {
            // Setup your list of storage columns here
            List<StorageColumn> storageColumns = new List<StorageColumn>();
            // Add storage columns to the list
            return storageColumns;
        }
    }
}
