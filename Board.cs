using System;
using static System.Console;

namespace BoardGame
{
    public abstract class Board
    {
        public string[,,] boards;
        public int boardSize;
        public int gameBoardAmount;

        public Board(int boardSize, int gameBoardAmount)
        {
            this.boardSize = boardSize;
            this.gameBoardAmount = gameBoardAmount;
            boards = new string[gameBoardAmount, boardSize, boardSize];
            InitializeBoards();
        }

        public void InitializeBoards()
        {
            for (int b = 0; b < gameBoardAmount; b++)
            {
                for (int i = 0; i < boardSize; i++)
                {
                    for (int j = 0; j < boardSize; j++)
                    {
                        boards[b, i, j] = " ";
                    }
                }
            }
        }

        public void UpdateBoard(MoveBackup moveBackup)
        {
            foreach (var move in moveBackup.Backup)
            {
                int boardIndex = move.boardNum;
                int row = move.Row;
                int col = move.Col;
                char piece = move.Player.Symbol;
                boards[boardIndex, row, col] = piece.ToString();
            }
        }

        public void UndoBoard(MoveBackup moveBackup)
        {
            foreach (var move in moveBackup.SystemBackup)
            {
                int boardIndex = move.boardNum;
                int row = move.Row;
                int col = move.Col;
                char piece = move.Player.Symbol;
                boards[boardIndex, row, col] = " ";
            }
        }
        public void PrintBoard()
        {
            PrintColumnHeaders();
            for (int i = 0; i < boardSize; i++)
            {
                PrintRowWithNumbers(i);
                if (i < boardSize - 1)
                {
                    PrintHorizontal();
                }
            }
            WriteLine();
        }

        private void PrintColumnHeaders()
        {
            for (int b = 0; b < gameBoardAmount; b++)
            {
                Write("  ");
                for (int c = 0; c < boardSize; c++)
                {
                    Write(c + 1);
                    if (c < boardSize - 1 && c < 8) Write(" ");
                }
                if (b < gameBoardAmount - 1) Write("   ");
            }
            WriteLine();
        }

        private void PrintRowWithNumbers(int r)
        {
            for (int b = 0; b < gameBoardAmount; b++)
            {
                Write(r + 1);
                if (r < 9) Write(" ");
                for (int c = 0; c < boardSize; c++)
                {
                    Write(boards[b, r, c]);
                    if (c < boardSize - 1) Write("|");
                }
                if (b < gameBoardAmount - 1) Write("   ");
            }
            WriteLine();
        }

        private void PrintHorizontal()
        {
            for (int b = 0; b < gameBoardAmount; b++)
            {
                Write("  ");
                for (int c = 0; c < boardSize - 1; c++)
                {
                    Write("-+");
                }
                Write("-");
                if (b < gameBoardAmount - 1) Write("   ");
            }
            WriteLine();
        }
    }

    public class NotaktoBoard : Board
    {
        public NotaktoBoard() : base(3, 3) { }
    }

    public class GomokuBoard : Board
    {
        public GomokuBoard() : base(15, 1) { }
    }
}