using MinesweeperGameMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MinesweeperGameMvc.Controllers
{
    public class GameController : Controller
    {
        private static MinesweeperBoard _board;

        public ActionResult Index()
        {
            int initialMineCount = 15; // Starting mine count for Level 1
            _board = new MinesweeperBoard(10, 10, initialMineCount, 1);
            return View(_board);
        }

        [HttpPost]
        public ActionResult RevealCell(int row, int col)
        {
            _board.RevealCell(row, col);

            if (_board.IsGameOver)
            {
                ViewBag.Message = "Game Over! You hit a mine!";
                return PartialView("_Board", _board);
            }
            else if (_board.IsGameWon)
            {
                ViewBag.Message = $"Congratulations! You've completed Level {_board.Level}. Advancing to the next level...";
                _board.ResetForNextLevel(); // Automatically reset and advance to next level
            }

            return PartialView("_Board", _board);
        }

        [HttpPost]
        public ActionResult NextLevel()
        {
            _board.ResetForNextLevel();
            ViewBag.Message = $"Level {_board.Level} - Mines Remaining: {_board.MineCount}";
            return PartialView("_Board", _board);
        }

        public ActionResult GetGameMessage()
        {
            return PartialView("_GameMessage", _board);
        }
    }
}