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
    /// Interaction logic for CreateContainerStoragePage.xaml
    /// </summary>
    public partial class CreateContainerStoragePage : Page
    {
        public ContainerStorage CreatedContainerStorage { get; private set; }
        public event EventHandler<ContainerStorageEventArgs> OnContainerStorageCreated;
        private bool IsContainerStorageCreated = false;

        public CreateContainerStoragePage()
        {
            InitializeComponent();
        }


        private void CreateContainerStorage(object sender, RoutedEventArgs e)
        {
            if (IsContainerStorageCreated)
            {
                MessageBox.Show("A ContainerStorage has already been created.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
             
                string storageName = StorageNameTextBox.Text;
                int startIndex = int.Parse(StartIndexTextBox.Text);
                int endIndex = int.Parse(EndIndexTextBox.Text);

                if (string.IsNullOrWhiteSpace(storageName))
                {
                    MessageBox.Show("Please enter a storage name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (startIndex < 0 || endIndex < 0)
                {
                    MessageBox.Show("Index values cannot be negative.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (startIndex >= endIndex)
                {
                    MessageBox.Show("Start index must be less than end index.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var containerStorage = new ContainerStorage(storageName, startIndex, endIndex);
                CreatedContainerStorage = containerStorage;
                IsContainerStorageCreated = true;

                if (containerStorage != null)
                {
                    OnContainerStorageCreated?.Invoke(this, new ContainerStorageEventArgs(containerStorage));
                }
                string containerStorageInfo = $"Storage Name: {storageName}, Start Index: {startIndex}, End Index: {endIndex}";
                //ContainerStorageInfo.Items.Add(containerStorageInfo);

                

            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid index values.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
