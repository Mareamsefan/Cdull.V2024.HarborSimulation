using System;
using System.Collections.Generic;
using System.Windows;
using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;

namespace HarborSimulationGUI
{
    public partial class StorageSetupWindow : Window
    {
        private Harbor harbor;

        public StorageSetupWindow(Harbor harbor)
        {
            InitializeComponent();
            this.harbor = harbor;
        }

        private void InitializeStorageColumns_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Parse input values
                List<int> longColumnLocations = ParseLocations(txtLongColumnLocations.Text);
                List<int> shortColumnLocations = ParseLocations(txtShortColumnLocations.Text);
                int longColumnLength = int.Parse(txtLongColumnLength.Text);
                int shortColumnLength = int.Parse(txtShortColumnLength.Text);
                int numberOfLongColumns = int.Parse(txtNumberOfLongColumns.Text);
                int numberOfShortColumns = int.Parse(txtNumberOfShortColumns.Text);
                int columnWidth = int.Parse(txtColumnWidth.Text);
                int columnHeight = int.Parse(txtColumnHeight.Text);

                // Initialize storage columns
                List<StorageColumn> columns = harbor.InitializeStorageColumns(
                    longColumnLocations, shortColumnLocations,
                    longColumnLength, shortColumnLength,
                    numberOfLongColumns, numberOfShortColumns,
                    columnWidth, columnHeight);

                // Clear previous storage column info
                lstStorageColumns.Items.Clear();

                // Display storage column information
                foreach (var column in columns)
                {
                    string columnInfo = $"Column ID: {column.ColumnId}, Location: {column.Location}, Length: {column.Length}";
                    if (!lstStorageColumns.Items.Contains(columnInfo)) // Check for duplicates
                    {
                        lstStorageColumns.Items.Add(columnInfo);
                    }
                }

                // Update status or inform the user
                MessageBox.Show($"Storage columns initialized successfully.", "Storage Columns Initialization", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing storage columns: {ex.Message}", "Storage Columns Initialization Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private List<int> ParseLocations(string input)
        {
            List<int> locations = new List<int>();
            string[] parts = input.Split(',');
            foreach (var part in parts)
            {
                if (int.TryParse(part.Trim(), out int location))
                {
                    locations.Add(location);
                }
            }
            return locations;
        }
    }
}
