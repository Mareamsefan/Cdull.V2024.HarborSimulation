using Cdull.V2024.HarborSimulation.SimulationFramework;
using Cdull.V2024.HarborSimulation.SimulationFramework.Enums;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Size = Cdull.V2024.HarborSimulation.SimulationFramework.Enums.Size;
using Dock = Cdull.V2024.HarborSimulation.SimulationFramework.Dock;

namespace HarborSimulationGUI
{
    public partial class MainWindow : Window
    {
        private List<Dock> docks = new List<Dock>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateDock_Click(object sender, RoutedEventArgs e)
        {
            string dockName = DockNameTextBox.Text;
            if (string.IsNullOrWhiteSpace(dockName))
            {
                MessageBox.Show("Please enter a dock name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (DockSizeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a dock size.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string selectedSize = (DockSizeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            if (string.IsNullOrWhiteSpace(selectedSize))
            {
                MessageBox.Show("Invalid dock size selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Size dockSize;
            if (!Enum.TryParse(selectedSize, out dockSize))
            {
                MessageBox.Show("Invalid dock size selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Dock newDock = new Dock(dockName, dockSize);
            docks.Add(newDock);
            DockListBox.Items.Add($"Dock Name: {newDock.Name}, Size: {newDock.Size}");
        }

    }
}
