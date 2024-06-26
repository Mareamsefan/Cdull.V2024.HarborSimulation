﻿using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Dock = Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure.Dock;
using Size = Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure.Size;

namespace Cdull.V2024.HarborSimulationWPF
{
    /// <summary>
    /// Interaction logic for CreateDockPage.xaml
    /// </summary>
    public partial class CreateDockPage : Page
    {
       
        private Harbor harbor;

        /// <summary>
        /// Constructor for CreateDockPage.
        /// </summary>
        /// <param name="createdHarbor">The created harbor</param>
        public CreateDockPage(Harbor createdHarbor)
        {
            InitializeComponent();
            harbor = createdHarbor;
        }

        /// <summary>
        /// Handles the initialization of docks.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void InitializeDocks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int numberOfDocks = int.Parse(txtNumberOfDocks.Text);
                Size dockSize = (Size)Enum.Parse(typeof(Size), cmbDockSize.Text);
                int numberOfCranes = int.Parse(txtNumberOfCranes.Text);

                List<Dock> docks = harbor.InitializeDocks(numberOfDocks, dockSize, numberOfCranes);

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

        /// <summary>
        /// Displays dock information in the ListBox.
        /// </summary>
        /// <param name="docks">The list of docks</param>
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

