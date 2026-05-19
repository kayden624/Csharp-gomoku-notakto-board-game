using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame
{
    public abstract class HelpSystem
    {
        public string rule { get; set; }
        public string moveOptions { get; set; }

        public void DisplayContext()
        {
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Help Hint~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine();
            Console.WriteLine(rule);
            Console.WriteLine();
            Console.WriteLine(moveOptions);
            Console.WriteLine();
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        }
    }

    public class NotaktoHelpSystem: HelpSystem
    {
        public NotaktoHelpSystem()
        {
            rule = "Notakto is a variant of Tic-Tac-Toe played on multiple empty grids. Players take turns placing \"X\" on any grid, trying to avoid creating three in a row. The player who forces a three-in-a-row loses. The game ends when one player loses.";
            moveOptions = "There are some move you can do in current game: \n 1. Move 2. Undo 3. Redo 4. Save 5. Exit \n Type any command to execute action.";
        }
    }

    public class GomokuHelpSystem: HelpSystem
    {
        public GomokuHelpSystem()
        {
            rule = "Gomoku is a two-player board game played on a 15x15 grid. Players alternate placing black or white stones ('X' or 'O' symbol), aiming to align exactly five stones in a row—horizontally, vertically, or diagonally—without being blocked by the opponent. The first to do so wins.";
            moveOptions = "There are some move you can do in current game: \n 1. Move 2. Undo 3. Redo 4. Save 5. Exit \n Type any command to execute action.";
        }
    }
}
