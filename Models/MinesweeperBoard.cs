using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MinesweeperGameMvc.Models
{
    public class MinesweeperBoard
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public MinesweeperCell[,] Cells { get; set; }
        public bool IsGameOver { get; set; } = false;
        public bool IsGameWon { get; set; } = false;
        public int Level { get; set; } = 1; // Track the current level
        public int MineCount { get; private set; } // Track the number of mines for the level

        // Constructor with adjustable mine count based on level
        public MinesweeperBoard(int rows, int columns, int initialMineCount, int level)
        {
            Rows = rows;
            Columns = columns;
            Level = level;

            // Decrease mine count as the level increases (e.g., reduce by 2 mines per level)
            MineCount = Math.Max(1, initialMineCount - (level - 1) * 5);
            Cells = new MinesweeperCell[rows, columns];
            InitializeBoard(MineCount);
        }

        public void ResetForNextLevel()
        {
            IsGameOver = false;
            IsGameWon = false;
            Level++;
            MineCount += 5; // Further decrease mines for the next level
            Cells = new MinesweeperCell[Rows, Columns];
            InitializeBoard(MineCount);
        }

        public void RevealAllMines()
        {
            foreach (var cell in Cells)
            {
                if (cell.IsMine)
                {
                    cell.IsRevealed = true;
                }
            }
        }


        private void InitializeBoard(int mineCount)
        {
            // Initialize each cell
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    Cells[r, c] = new MinesweeperCell();
                }
            }
            // Place mines and calculate adjacent mine counts
            PlaceMines(mineCount);
            CalculateAdjacentMines();
        }

        private void PlaceMines(int mineCount)
        {
            Random rnd = new Random();
            for (int i = 0; i < mineCount; i++)
            {
                int row, col;
                do
                {
                    row = rnd.Next(Rows);
                    col = rnd.Next(Columns);
                } while (Cells[row, col].IsMine);
                Cells[row, col].IsMine = true;
            }
        }

        private void CalculateAdjacentMines()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    if (!Cells[r, c].IsMine)
                    {
                        int mineCount = 0;
                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                int row = r + i;
                                int col = c + j;
                                if (row >= 0 && row < Rows && col >= 0 && col < Columns && Cells[row, col].IsMine)
                                {
                                    mineCount++;
                                }
                            }
                        }
                        Cells[r, c].AdjacentMines = mineCount;
                    }
                }
            }
        }

        // Reveal cell logic
        public void RevealCell(int row, int col)
        {
            if (IsGameOver || IsGameWon || Cells[row, col].IsRevealed) return;

            Cells[row, col].IsRevealed = true;

            // Check if it's a mine
            if (Cells[row, col].IsMine)
            {
                IsGameOver = true;
                RevealAllMines(); // Reveal all mines
                return;
            }

            // Reveal adjacent cells if no adjacent mines
            if (Cells[row, col].AdjacentMines == 0)
            {
                RevealAdjacentCells(row, col);
            }

            // Check if all non-mine cells are revealed for a win
            CheckWinCondition();
        }


        private void RevealAdjacentCells(int row, int col)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int newRow = row + i;
                    int newCol = col + j;
                    if (newRow >= 0 && newRow < Rows && newCol >= 0 && newCol < Columns &&
                        !Cells[newRow, newCol].IsRevealed && !Cells[newRow, newCol].IsMine)
                    {
                        RevealCell(newRow, newCol); // Recursively reveal empty cells
                    }
                }
            }
        }

        private void CheckWinCondition()
        {
            // Win if all non-mine cells are revealed
            foreach (var cell in Cells)
            {
                if (!cell.IsMine && !cell.IsRevealed)
                {
                    return; // Still cells left to reveal, no win yet
                }
            }
            IsGameWon = true;
        }
    }
}