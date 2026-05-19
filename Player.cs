using System;
using System.Collections.Generic;

namespace BoardGame
{
    public class Player
    {
        public string PlayerName { get; set; }
        public string PlayerState { get; set; }
        public bool IsHuman { get; set; }
        public char Symbol { get; set; } 

        public Player(string playerName, string playerState, bool isHuman, char symbol)
        {
            PlayerName = playerName;
            PlayerState = playerState;
            IsHuman = isHuman;
            Symbol = symbol;

        }

        public class HumanPlayer : Player
        {
            public HumanPlayer(string playerName, string playerState, char symbol)
                : base(playerName, playerState, true, symbol)
            {
            } 
        }

        public class ComputerPlayer : Player
        {
            public ComputerPlayer(string playerName, char symbol)
                : base(playerName, "P2", false, symbol)
            {
            }
        }
    }
}