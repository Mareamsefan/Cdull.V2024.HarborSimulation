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
        private CreateHarborPage harborPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="HarborSimulationHome"/> class.
        /// </summary>
        public HarborSimulationHome()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the click event for creating a container storage.
        /// </summary>
        private void CreateContainerStorage_Click(object sender, RoutedEventArgs e)
        {
            CreateContainerStoragePage createContainerStoragePage = new CreateContainerStoragePage();
            MainFrame.Navigate(createContainerStoragePage);
            createContainerStoragePage.OnContainerStorageCreated += (sender, args) =>
            {
                containerStorage = args.CreatedContainerStorage;
            };

        }

        /// <summary>
        /// Handles the click event for creating a harbor.
        /// </summary>
        private void CreateHarbor_Click(object sender, RoutedEventArgs e)
        {
            if (containerStorage != null)
            {
                harborPage = new CreateHarborPage(containerStorage);
                MainFrame.Navigate(harborPage);
            }
            else
            {
                MessageBox.Show("Please create a container storage first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles the click event for creating docks.
        /// </summary>
        private void CreateDocks_Click(object sender, RoutedEventArgs e)
        {   
            if (harborPage.CreatedHarbor != null)
            {
                CreateDockPage createDockPage = new CreateDockPage(harborPage.CreatedHarbor);
                MainFrame.Navigate(createDockPage);
            }
            else
            {
                MessageBox.Show("Please create a harbor first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles the click event for creating AGVs.
        /// </summary>
        private void CreateAGVs_Click(object sender, RoutedEventArgs e)
        {
            if (harborPage.CreatedHarbor != null)
            {
                CreateAGVPage createAGVPage = new CreateAGVPage(harborPage.CreatedHarbor);
                MainFrame.Navigate(createAGVPage);
            }
            else
            {
                MessageBox.Show("Please create a harbor first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles the click event for creating storage columns.
        /// </summary>
        private void CreateStorageColumns_Click(object sender, RoutedEventArgs e)
        {
            if (harborPage.CreatedHarbor != null)
            {
                CreateStorageColumnPage createStorageColumnPage = new CreateStorageColumnPage(harborPage.CreatedHarbor);
                MainFrame.Navigate(createStorageColumnPage);
            }
            else
            {
                MessageBox.Show("Please create a harbor first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
