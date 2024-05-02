using System;
using System.Collections.Generic;
using System.Windows;
using Cdull.V2024.HarborSimulation.SimulationFramework;
using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;
using Size = Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure.Size;

namespace HarborSimulationGUI
{
    public partial class Docks : Window
    {
        private Harbor harbor;

        public Docks()
        {
            InitializeComponent();
        }

        public Docks(Harbor harbor)
        {
            InitializeComponent();
            this.harbor = harbor;
        }

        private void InitializeDocks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int numberOfDocks = int.Parse(txtNumberOfDocks.Text);
                Size dockSize = (Size)Enum.Parse(typeof(Size), cmbDockSize.Text);
                int numberOfCranes = int.Parse(txtNumberOfCranes.Text);

                List<Dock> docks = harbor.InitializeDocks(numberOfDocks, dockSize, numberOfCranes);

                // Display dock information in ListBox
                DisplayDockInformation(docks);
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid input format. Please enter valid numeric values.", "Initialization Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Error initializing docks: {ex.Message}", "Initialization Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Initialization Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayDockInformation(List<Dock> docks)
        {
            lstDockInfo.Items.Clear();

            foreach (var dock in docks)
            {
                lstDockInfo.Items.Add($"Dock ID: {dock.Id}, Size: {dock.Size}, Number of Cranes: {dock.Cranes.Count}");
            }

            statusBarText.Text = $"{docks.Count} docks initialized successfully.";
        }
    }
}
