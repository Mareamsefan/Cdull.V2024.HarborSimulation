﻿using Cdull.V2024.HarborSimulation.SimulationFramework.Infrastructure;
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
    /// Interaction logic for CreateStorageColumnPage.xaml
    /// </summary>
    public partial class CreateStorageColumnPage : Page
    {
      
        private Harbor harbor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateStorageColumnPage"/> class.
        /// </summary>
        /// <param name="createdHarbor">The harbor where the storage columns will be initialized.</param>
        public CreateStorageColumnPage(Harbor createdHarbor)
        {
            InitializeComponent();
            harbor = createdHarbor;
        }

        /// <summary>
        /// Handles the initialization of storage columns.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void InitializeStorageColumns_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<int> longColumnLocations = ParseLocations(txtLongColumnLocations.Text);
                List<int> shortColumnLocations = ParseLocations(txtShortColumnLocations.Text);
                int longColumnLength = int.Parse(txtLongColumnLength.Text);
                int shortColumnLength = int.Parse(txtShortColumnLength.Text);
                int numberOfLongColumns = int.Parse(txtNumberOfLongColumns.Text);
                int numberOfShortColumns = int.Parse(txtNumberOfShortColumns.Text);
                int columnWidth = int.Parse(txtColumnWidth.Text);
                int columnHeight = int.Parse(txtColumnHeight.Text);

                List<StorageColumn> columns = harbor.InitializeStorageColumns(
                    longColumnLocations, shortColumnLocations,
                    longColumnLength, shortColumnLength,
                    numberOfLongColumns, numberOfShortColumns,
                    columnWidth, columnHeight);


                lstStorageColumns.Items.Clear();

 
                foreach (var column in columns)
                {
                    string columnInfo = $"Column ID: {column.ColumnId}, Location: {column.Location}, Length: {column.Length}";
                    if (!lstStorageColumns.Items.Contains(columnInfo)) 
                    {
                      lstStorageColumns.Items.Add(columnInfo);
                    }
                }

                MessageBox.Show($"Storage columns initialized successfully.", "Storage Columns Initialization", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing storage columns: {ex.Message}", "Storage Columns Initialization Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Parses the input string to extract the locations.
        /// </summary>
        /// <param name="input">The input string containing locations separated by comma.</param>
        /// <returns>A list of integer locations.</returns>
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
