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
    /// Interaction logic for CreateHarborPage.xaml
    /// </summary>
    public partial class CreateHarborPage : Page
    {
        /// <summary>
        /// Gets the created harbor.
        /// </summary>
        public Harbor CreatedHarbor { get; set; }

        private ContainerStorage CreatedContainerStorage;
        private bool IsHarborCreated = false;


        /// <summary>
        /// Constructor for CreateHarborPage.
        /// </summary>
        /// <param name="createdContainerStorage">The created container storage</param>
        public CreateHarborPage(ContainerStorage createdContainerStorage)
        {
            InitializeComponent();
            CreatedContainerStorage = createdContainerStorage;
        }

        /// <summary>
        /// Handles the creation of a harbor.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void CreateHarbor(object sender, RoutedEventArgs e)
        {
            if (IsHarborCreated)
            {
                MessageBox.Show("A Harbor has already been created.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(HarborNameTextBox.Text))
            {
                MessageBox.Show("Please enter a harbor name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string harborName = HarborNameTextBox.Text;
            int indexRange;
            if (!int.TryParse(HarborIndexTextBox.Text, out indexRange))
            {
                MessageBox.Show("Please enter a valid index range.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (indexRange < 0)
            {
                MessageBox.Show("Harbor index range cannot be negative.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Harbor harbor = new Harbor(harborName, indexRange, CreatedContainerStorage);
            IsHarborCreated = true;
            CreatedHarbor = harbor;
            
            string harborInfo = $"Harbor Name: {harborName}, Index Range: {indexRange}";
            HarborInfo.Items.Add(harborInfo);
        }
    }

}

