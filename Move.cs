using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame
{
    public class Move
    {
        public int boardNum {  get; set; }
        public int Row {  get; set; }
        public int Col { get; set; }
        public Player Player {  get; set; }
        
        public Move(int boardnum, int row, int col, Player player)
        {
            boardNum = boardnum;
            Row = row;
            Col = col;
            Player = player;
        }

        public string ToTXTString()
        {
            return String.Format("{0},{1},{2},{3},{4},{5}", boardNum, Row, Col, Player.PlayerName, Player.PlayerState, Player.IsHuman);
        }
    }

    public class MoveBackup
    {
        public List<Move> Backup { get; set;}
        public List<Move> SystemBackup { get; set;}
        public int tag = 0;
        private static MoveBackup instance;

        private MoveBackup()
        { 
            Backup = new List<Move>();
            SystemBackup = new List<Move>();
        }

        public static MoveBackup Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MoveBackup();
                }
                return instance;
            }
        }

        public int CountMoves()
        {
            return Backup.Count;
        }

    }
}
