using GameOfLife.Models;
using System.Collections.Generic;
using System.Diagnostics;


namespace GameOfLife
{
    class GameOfLifeLogic
    {
        public Cyclical2DCellArray Cells { get; private set; }
        private int gridWidth, gridHeight;
        public int GridWidth { get {return gridWidth; } }
        public int GridHeight { get { return gridHeight; } }

        List<Cell> cellsToDie;
        List<Cell> cellsToBorn;
        public GameOfLifeLogic(int width, int height)
        {
            gridHeight = height; gridWidth = width;
            Cells = new Cyclical2DCellArray(width, height);
            cellsToDie = new List<Cell>();
            cellsToBorn = new List<Cell>();
        }

        public void SetGridSize(int width, int height)
        {
            if (gridWidth == width && gridHeight == height) return;
            Cyclical2DCellArray tmp = new Cyclical2DCellArray(width, height);
            Cells.CopyTo(tmp);
            Cells = tmp;
            gridWidth = width;
            gridHeight = height;
        }

        private byte CountNeighbours(int y, int x)
        {
            byte count = 0;
            if (Cells[y - 1,x - 1].IsAlive) count++;
            if (Cells[y,x - 1].IsAlive) count++;
            if (Cells[y + 1,x - 1].IsAlive) count++;
            if (Cells[y - 1,x].IsAlive) count++;
            if (Cells[y + 1,x].IsAlive) count++;
            if (Cells[y - 1,x + 1].IsAlive) count++;
            if (Cells[y,x + 1].IsAlive) count++;
            if (Cells[y + 1,x + 1].IsAlive) count++;
            return count;
        }

        public void Step()
        {
            //Assign cells to born and die
            cellsToBorn.Clear();
            cellsToDie.Clear();
            for (int i =0; i < GridWidth; i++)
            {
                for (int j = 0; j < GridHeight; j++)
                {
                    byte neighbors = CountNeighbours(i,j);
                    if (neighbors < 2 || neighbors>3)
                    {
                        cellsToDie.Add(Cells[i,j]);
                    }else if (neighbors == 3)
                    {
                        cellsToBorn.Add(Cells[i,j]);
                    }
                }
            }

            //Set new table
            foreach (Cell cell in cellsToBorn)
            {
                cell.IsAlive = true;
            }
            foreach (Cell cell in cellsToDie)
            {
                cell.IsAlive = false;
            }
        }

    }
}
