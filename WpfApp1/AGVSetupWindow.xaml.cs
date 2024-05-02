using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;
using System;
using System.Collections.Generic;
using System.Windows;

namespace HarborSimulationGUI
{
    public partial class AGVSetupWindow : Window
    {
        private Harbor harbor;
        private int i = 0;

        public AGVSetupWindow(Harbor harbor)
        {
            InitializeComponent();
            this.harbor = harbor;
        }

        private void InitializeAGVs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int numberOfAGVs = int.Parse(txtNumberOfAGVs.Text);
                int agvLocation = int.Parse(txtAGVLocation.Text);

                List<AGV> agvs = harbor.InitializeAGVs(numberOfAGVs, agvLocation);

                // Clear previous AGV info
                lstAGVInfo.Items.Clear();

                // Display AGV information
                foreach (var agv in agvs)
                {
              
                    string agvInfo = $"AGV ID: {i}, Location: {agv.Location}";
                    lstAGVInfo.Items.Add(agvInfo);
                    i++;
                    
                }

                // Update status or inform the user
                MessageBox.Show($"{numberOfAGVs} AGVs initialized successfully.", "AGV Initialization", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing AGVs: {ex.Message}", "AGV Initialization Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
