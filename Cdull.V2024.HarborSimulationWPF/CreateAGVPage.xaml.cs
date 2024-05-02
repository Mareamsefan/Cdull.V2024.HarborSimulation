using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;
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

namespace Cdull.V2024.HarborSimulationWPF
{
    /// <summary>
    /// Interaction logic for CreateAGVPage.xaml
    /// </summary>
    public partial class CreateAGVPage : Page
    {
      

        private Harbor harbor;
        private int i = 0;
        public CreateAGVPage(Harbor createdHarbor)
        {
            InitializeComponent();
            harbor = createdHarbor;
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

                    string agvInfo = $"AGV ID: {i}, Location: {agvLocation}";
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
