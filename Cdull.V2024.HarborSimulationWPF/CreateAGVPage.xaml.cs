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
        /// <summary>
        /// Represents a page for creating AGVs (Automated Guided Vehicles) in a harbor simulation.
        /// </summary>
        private Harbor harbor;
        private int i = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAGVPage"/> class.
        /// </summary>
        /// <param name="createdHarbor">The harbor object where AGVs will be initialized.</param>
        public CreateAGVPage(Harbor createdHarbor)
        {
            InitializeComponent();
            harbor = createdHarbor;
        }

        /// <summary>
        /// Event handler for the button click to initialize AGVs.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void InitializeAGVs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int numberOfAGVs = int.Parse(txtNumberOfAGVs.Text);
                int agvLocation = int.Parse(txtAGVLocation.Text);

                List<AGV> agvs = harbor.InitializeAGVs(numberOfAGVs, agvLocation);

                lstAGVInfo.Items.Clear();

                foreach (var agv in agvs)
                {

                    string agvInfo = $"AGV ID: {i}, Location: {agv.Location}";
                    lstAGVInfo.Items.Add(agvInfo);
                    i++;

                }

                MessageBox.Show($"{numberOfAGVs} AGVs initialized successfully.", "AGV Initialization", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing AGVs: {ex.Message}", "AGV Initialization Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
