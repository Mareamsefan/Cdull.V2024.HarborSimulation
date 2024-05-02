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
    /// Interaction logic for HarborSimulationHome.xaml
    /// </summary>
    public partial class HarborSimulationHome : Page
    {
        private ContainerStorage containerStorage;

        public HarborSimulationHome()
        {
            InitializeComponent();
        }

        private void CreateContainerStorage_Click(object sender, RoutedEventArgs e)
        {
            // Opprett en instans av CreateContainerStoragePage
            CreateContainerStoragePage createContainerStoragePage = new CreateContainerStoragePage();
            // Naviger til CreateContainerStoragePage
            MainFrame.Navigate(createContainerStoragePage);
            // Hent CreatedContainerStorage fra CreateContainerStoragePage etter navigeringen
            createContainerStoragePage.OnContainerStorageCreated += (sender, args) =>
            {
                containerStorage = args.CreatedContainerStorage;
            };

        }

        private void CreateHarbor_Click(object sender, RoutedEventArgs e)
        {
            // Sørg for at containerStorage er satt før du navigerer til HarborPage
            if (containerStorage != null)
            {
                // Opprett en instans av HarborPage og send med containerStorage som parameter
                CreateHarborPage harborPage = new CreateHarborPage(containerStorage);
                MainFrame.Navigate(harborPage);
            }
            else
            {
                MessageBox.Show("Please create a container storage first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ComponentsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void lstHarborInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
