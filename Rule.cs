using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BoardGame
{
    public abstract class Rule
    {
        public abstract bool CheckIfWin();
        public abstract bool IsBoardDead(int boardNum);
        public abstract bool IsMoveValid(int boardNum, int row, int col);
    }

    public class GomokuRule: Rule
    {
        private GomokuBoard board;
        public GomokuRule(GomokuBoard board)
        {
            this.board = board;
        }

        public override bool CheckIfWin()
        {
            for (int b = 0; b < board.boards.GetLength(0); b++)
            {
                for (int i = 0; i < board.boardSize; i++)
                {
                    for (int j = 0; j < board.boardSize; j++)
                    {
                        if (CheckDirection(board.boards, b, i, j, 1, 0) || 
                            CheckDirection(board.boards, b, i, j, 0, 1) || 
                            CheckDirection(board.boards, b, i, j, 1, 1) || 
                            CheckDirection(board.boards, b, i, j, 1, -1))  
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool CheckDirection(string[,,] boards, int b, int row, int col, int dRow, int dCol)
        {
            int count = 0;
            string playerMark = boards[b, row, col];

            if (string.IsNullOrWhiteSpace(playerMark))
            {
                return false;
            }

            for (int k = 0; k < 5; k++)
            {
                int newRow = row + k * dRow;
                int newCol = col + k * dCol;

                if (newRow < 0 || newRow >= boards.GetLength(1) || newCol < 0 || newCol >= boards.GetLength(2))
                {
                    return false;
                }

                if (boards[b, newRow, newCol] == playerMark)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            return count == 5;
        }
        public override bool IsMoveValid(int boardNum, int row, int col)
        {
            if (board.boards[boardNum, row, col] == "X" || board.boards[boardNum, row, col] == "O")
            {
                return false;
            }
            return true;
        }
        public override bool IsBoardDead(int boardNum) { return true; }

    }

    public class NotaktoRule : Rule
    {
        private NotaktoBoard board;

        public NotaktoRule(NotaktoBoard board)
        {
            this.board = board;
        }

        public override bool IsBoardDead(int boardNum)
        {
            for (int i = 0; i < board.boardSize; i++)
            {
                if ((board.boards[boardNum, i, 0] == "X" && board.boards[boardNum, i, 1] == "X" && board.boards[boardNum, i, 2] == "X") || 
                    (board.boards[boardNum, 0, i] == "X" && board.boards[boardNum, 1, i] == "X" && board.boards[boardNum, 2, i] == "X"))   
                {
                    return true;
                }
            }

            if ((board.boards[boardNum, 0, 0] == "X" && board.boards[boardNum, 1, 1] == "X" && board.boards[boardNum, 2, 2] == "X") ||
                (board.boards[boardNum, 0, 2] == "X" && board.boards[boardNum, 1, 1] == "X" && board.boards[boardNum, 2, 0] == "X"))
            {
                return true;
            }

            return false;
        }

        public override bool CheckIfWin()
        {
            int deadBoards = 0;
            for (int b = 0; b < board.gameBoardAmount; b++)
            {
                if (IsBoardDead(b))
                {
                    deadBoards++;
                    Console.WriteLine("Board {0} is Dead", b+1);
                }
            }

            if (deadBoards == board.gameBoardAmount)
            {
                
                return true; 
            }
            return false;
        }

        public override bool IsMoveValid(int boardNum, int row, int col)
        {
            if (board.boards[boardNum, row, col] == "X")
            {
                return false;
            }
            return true;
        }
    }
}

