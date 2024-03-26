using System;
using System.Collections.Generic;
using System.Linq;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    /// <summary>
    /// Represents a column used for storage in the harbor.
    /// </summary>
    public class Column
    {
        private int id;
        private int height;
        private int width;
        private int distance;
        private int length;


        /// <summary>
        /// List containing the elements of the column.
        /// </summary>
        public List<int> ColumnList { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Column"/> class with specified parameters.
        /// </summary>
        /// <param name="columnId">The unique identifier of the column.</param>
        /// <param name="columnHeight">The height of the column.</param>
        /// <param name="columnWidth">The width of the column.</param>
        /// <param name="columnDistance">The distance of the column.</param>
        /// <param name="distance">The distance of the column.</param>
        public Column(int columnId, int columnHeight, int columnWidth, int columnDistance, int distance)
        {
            id = columnId;
            height = columnHeight;
            width = columnWidth;
            distance = columnDistance;

            
            ColumnList = Enumerable.Repeat(0, columnHeight * columnWidth * columnDistance).ToList();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Column"/> class with specified parameters.
        /// </summary>
        /// <param name="id">The unique identifier of the column.</param>
        /// <param name="length">The length of the column.</param>
        /// <param name="height">The height of the column.</param>
        public Column(int id, int length, int height)
        {
            this.id = id;
            this.length = length;
            this.height = height;
        }
    }
}
