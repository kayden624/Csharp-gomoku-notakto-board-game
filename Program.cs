using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGame
{
    public class Program
    {
        
        static void Main(string[] args)
        { 
            GameSystem game = new GameSystem();
            game.InitialGame(out game);
            game.StartPlaying();
            game.GameFinished();

        }
        
    }
}