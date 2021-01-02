using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GameOfLife.Models
{
    class Cyclical2DCellArray
    {
        private List<Cell> cells;
        private int width;
        private int height;

        public Cyclical2DCellArray(int width, int height)
        {
            this.width = width;
            this.height = height;
            cells = new List<Cell>();
            for (int i = 0; i < width*height; i++)
            {
                cells.Add(new Cell());
            }
        }

        public void Clear()
        {
            foreach (Cell cell in cells)
            {
                cell.IsAlive = false;
            };
        }

        public void CopyTo(Cyclical2DCellArray dest)
        {
            int width = Math.Min(this.width, dest.width);
            int height = Math.Min(this.height, dest.height);
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    dest[x, y] = this[x, y];
        }

        public Cell this[int x, int y]
        {
            get
            {
                int x1 = (x < 0 ? x + width : x) % width;
                int y1 = (y < 0 ? y + height : y) % height;
                int index = y1 * width + x1;
                return cells[index];
            }
            set
            {
                if (x < 0 || x >= width ||
                    y < 0 || y >= height)
                    throw new ArgumentOutOfRangeException();

                int x1 = x % width;
                int y1 = y % height;
                int index = y1 * width + x1;
                cells[index] = value;
            }
        }
    }
}
