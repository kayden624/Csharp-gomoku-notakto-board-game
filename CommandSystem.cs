using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using static BoardGame.Player;

namespace BoardGame
{
    public interface CommandSystem
    {
        public void Run();
    }

    public class MakeMove : CommandSystem
    {
        private Move move { get; set; }
        private MoveBackup moveBackup = MoveBackup.Instance;

        public MakeMove(int boardNum, int row, int col, Player player)
        {
            move = new Move(boardNum, row ,col ,player);
        }

        public void Run()
        {
            if (moveBackup.SystemBackup.Count != 0)
            {
                moveBackup.SystemBackup.Clear();
            }
            MoveBackup.Instance.Backup.Add(move);
            MoveBackup.Instance.tag++;
        }
    }

    public class RedoMove : CommandSystem
    {
        private MoveBackup moveBackup = MoveBackup.Instance;
        public void Run()
        {
            
            if (moveBackup.SystemBackup.Count == 0 || moveBackup.SystemBackup.Count == 1)
            {
                Console.WriteLine("There is no move can redo!");
                Console.WriteLine();
                return;
            }

            if (moveBackup.SystemBackup.Count > 1)
            {
                for (int i = 0; i < 2; i++)
                {
                    Move forward = moveBackup.SystemBackup[moveBackup.SystemBackup.Count - 1];
                    moveBackup.Backup.Add(forward);
                    moveBackup.SystemBackup.Remove(forward);
                    MoveBackup.Instance.tag++;
                }
            }
        }
    }

    public class UndoMove : CommandSystem
    {
        private MoveBackup moveBackup = MoveBackup.Instance;
        public void Run()
        {
            if (moveBackup.Backup.Count == 0 || moveBackup.Backup.Count == 1)
            {
                Console.WriteLine("There is no move can undo!");
                Console.WriteLine();
                return;
            }
            if (moveBackup.tag > 1)
            {
                for (int i = 0; i < 2; i++)
                {
                    Move back = moveBackup.Backup[moveBackup.tag - 1];
                    moveBackup.SystemBackup.Add(back);
                    moveBackup.Backup.Remove(back);
                    MoveBackup.Instance.tag--;
                }
            }
        }
    }

    public class LoadGame : CommandSystem
    {
        private string FILENAME { get; set; }
        private GameSystem game;
        
        public LoadGame(GameSystem game)
        {
            this.game = game;
        }
        
        private bool AddMove(Move move)
        {
            MoveBackup.Instance.Backup.Add(move);
            return true;
        }
        public void Run()
        {
            MoveBackup.Instance.SystemBackup.Clear();
            MoveBackup.Instance.Backup.Clear();

            switch (game.gameMode)
            {
                case GameSystem.GameMode.Notakto:
                    this.FILENAME = "NotaktoSave.txt";
                    break;
                case GameSystem.GameMode.Gomoku:
                    this.FILENAME = "GomokuSave.txt";
                    break;
            }

            if (!File.Exists(FILENAME))
            {
                Console.WriteLine("No saved game found! Sorry.");
                Console.WriteLine();
            }
            else
            {
                using (FileStream inFile = new FileStream(FILENAME, FileMode.Open, FileAccess.Read))
                {
                    StreamReader reader = new StreamReader(inFile);
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        var infos = line.Split(',');
                        switch (infos[4])
                        {
                            case "P1":
                                if (game.gameMode == GameSystem.GameMode.Notakto)
                                {
                                    game.player1 = new HumanPlayer(infos[3], infos[4], 'X');
                                }
                                else
                                {
                                    game.player1 = new HumanPlayer(infos[3], infos[4], 'O');
                                }
                                AddMove(new Move(int.Parse(infos[0]), int.Parse(infos[1]), int.Parse(infos[2]), game.player1));
                                game.player1.PlayerName = infos[3].ToString();
                                game.currentPlayer = game.player1;
                                break;
                            case "P2":
                                if (infos[5] == "true")
                                {
                                    game.player2 = new HumanPlayer(infos[3], infos[4], 'X');
                                }
                                else
                                {
                                    game.player2 = new ComputerPlayer("Computer", 'X');
                                }
                                AddMove(new Move(int.Parse(infos[0]), int.Parse(infos[1]), int.Parse(infos[2]), game.player2));
                                game.player2.PlayerName = infos[3].ToString();
                                game.currentPlayer = game.player2;
                                break;
                        }
                    }
                }
                game.isFinish = false;
                Console.WriteLine("Game loaded!");
                Console.WriteLine();
            }
        }
    }

    public class SaveGame : CommandSystem
    {
        private string FILENAME { get; set; }
        private MoveBackup moveBackup = MoveBackup.Instance;
        private GameSystem.GameMode gameMode;
        public SaveGame(GameSystem.GameMode gameMode)
        {
            this.gameMode = gameMode;
        }
        public void Run()
        {
            if (moveBackup.Backup.Count >= 2)
            {
                switch (gameMode)
                {
                    case GameSystem.GameMode.Notakto:
                        FILENAME = "NotaktoSave.txt";
                        break;
                    case GameSystem.GameMode.Gomoku:
                        FILENAME = "GomokuSave.txt";
                        break;
                }

                if (!File.Exists(FILENAME))
                {
                    FileStream outFile = new FileStream(FILENAME, FileMode.Create, FileAccess.Write);
                    outFile.Close();
                }

                using (StreamWriter writer = new StreamWriter(FILENAME))
                {
                    foreach (Move move in MoveBackup.Instance.Backup)
                    {
                        writer.WriteLine(move.ToTXTString());
                    }
                }
                Console.WriteLine("Game saved!");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("You need to start the game at least move two times to save!");
                Console.WriteLine();
            }
        }
    }

    public class Exit : CommandSystem
    {
        public void Run()
        {
            Console.WriteLine("Thank you for playing.");
            Environment.Exit(0);
        }
    }
}