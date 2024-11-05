using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MinesweeperGameMvc.Models
{
    public class MinesweeperCell
    {
        public bool IsMine { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; }
        public int AdjacentMines { get; set; }
    }
}