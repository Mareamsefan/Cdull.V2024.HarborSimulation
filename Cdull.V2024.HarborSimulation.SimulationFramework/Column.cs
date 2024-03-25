using System;
using System.Collections.Generic;
using System.Linq;

namespace Cdull.V2024.HarborSimulation.SimulationFramework
{
    public class Column
    {
        private int id;
        private int height;
        private int width;
        private int distance;
        private int length;

        public List<int> ColumnList { get; set; }

        public Column(int columnId, int columnHeight, int columnWidth, int columnDistance, int distance)
        {
            id = columnId;
            height = columnHeight;
            width = columnWidth;
            distance = columnDistance;

            // Opprett en liste med størrelse lik length, width og height
            ColumnList = Enumerable.Repeat(0, columnHeight * columnWidth * columnDistance).ToList();
        }

        public Column(int id, int length, int height)
        {
            this.id = id;
            this.length = length;
            this.height = height;
        }
    }
}
