using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Cdull.V2024.HarborSimulation.SimulationFramework;
using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;
using Size = Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure.Size;

namespace HarborSimulationGUI
{
    public partial class MainWindow : Window
    {
        private Harbor harbor;

        public MainWindow()
        {
            InitializeComponent();
            harbor = new Harbor("TestHarbor", 1000, new ContainerStorage("ContainerStorage", 0, 500));
        }

        private void InitializeShips_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int shipCurrentLocation = int.Parse(txtCurrentLocation.Text);
                int numberOfShips = int.Parse(txtNumberOfShips.Text);
                Model shipModel = (Model)Enum.Parse(typeof(Model), cmbShipModel.Text.Replace(" ", ""));
                Size shipSize = (Size)Enum.Parse(typeof(Size), cmbShipSize.Text);
                int numberOfContainers = int.Parse(txtNumberOfContainers.Text);
                ContainerSize containerSize = (ContainerSize)Enum.Parse(typeof(ContainerSize), cmbContainerSize.Text);

                List<Ship> ships = harbor.InitializeShips(shipCurrentLocation, numberOfShips, shipModel, shipSize, numberOfContainers, containerSize);

                // Clear previous ship info
                lstShipInfo.Items.Clear();

                // Display ship information
                foreach (var ship in ships)
                {
                    string shipInfo = $"Name: {ship.Name}, Model: {ship.Model}, Size: {ship.Size}, Current Location: {ship.CurrentLocation}, Containers Count: {numberOfContainers}, Container Size: {containerSize}";
                    lstShipInfo.Items.Add(shipInfo);
                }

                // Update status bar
                statusBarText.Text = $"{numberOfShips} ships initialized successfully.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing ships: {ex.Message}", "Initialization Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
