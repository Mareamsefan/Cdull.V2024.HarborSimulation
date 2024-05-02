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
using Size = Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure.Size;

namespace Cdull.V2024.HarborSimulationWPF
{
    /// <summary>
    /// Interaction logic for CreateShipPage.xaml
    /// </summary>
    public partial class CreateShipPage : Page
    {
        private Harbor harbor;
        public CreateShipPage(Harbor createdHarbor)
        {
            InitializeComponent();
            harbor = createdHarbor;
        }

        private void InitializeShips_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int shipCurrentLocation = int.Parse(txtCurrentLocation.Text);
                int numberOfShips = int.Parse(txtNumberOfShips.Text);
                Model shipModel = (Model)Enum.Parse(typeof(Model), cmbShipModel.Text.Replace(" ", ""));
                Size shipSize = (HarborSimulation.SimulationFramework.Infrastructure.Size)Enum.Parse(typeof(Size), cmbShipSize.Text);
                int numberOfContainers = int.Parse(txtNumberOfContainers.Text);
                ContainerSize containerSize = (ContainerSize)Enum.Parse(typeof(ContainerSize), cmbContainerSize.Text);

                List<Ship> ships = harbor.InitializeShips(shipCurrentLocation, numberOfShips, shipModel, shipSize, numberOfContainers, containerSize);

      
                lstShipInfo.Items.Clear();

        
                foreach (var ship in ships)
                {
                    string shipInfo = $"Name: {ship.Name}, Model: {ship.Model}, Size: {ship.Size}, Current Location: {ship.CurrentLocation}, Containers Count: {numberOfContainers}, Container Size: {containerSize}";
                    lstShipInfo.Items.Add(shipInfo);
                }

                statusBarText.Text = $"{numberOfShips} ships initialized successfully.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing ships: {ex.Message}", "Initialization Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }
}
