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
        public Harbor CreatedHarbor { get; set; }
        private ContainerStorage CreatedContainerStorage;
        private bool IsHarborCreated = false;


        public CreateHarborPage(ContainerStorage createdContainerStorage)
        {
            InitializeComponent();
            CreatedContainerStorage = createdContainerStorage;
        }
        private void CreateHarbor(object sender, RoutedEventArgs e)
        {
            if (IsHarborCreated)
            {
                MessageBox.Show("A Harbor has already been created.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Sjekk om HarborNameTextBox er fylt ut
            if (string.IsNullOrWhiteSpace(HarborNameTextBox.Text))
            {
                MessageBox.Show("Please enter a harbor name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Opprett en instans av Harbor-klassen med navnet gitt av brukeren, den opprettede containerStorage og indexRange
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
            /*
            string harborInfo = $"Harbor Name: {harborName}, Index Range: {indexRange}";
            HarborInfo.Items.Add(harborInfo);*/
        }
    }

}

