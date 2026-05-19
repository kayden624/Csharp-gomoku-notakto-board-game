// Bryan

using System.Diagnostics;
using System.Windows.Input;
using System.IO;
using static BoardGame.Player;

namespace BoardGame
{
    public class GameSystem
    {
        public Player player1 { get; set; }
        public Player player2 { get; set; }
        public Player currentPlayer;
        public enum GameType {PVP, PVC, undetermined};
        public GameType gameType { get; set; }
        public enum GameMode {Notakto, Gomoku};
        public GameMode gameMode { get; set; }
        public bool isFinish = true;
        public Rule gameRule { get; set; }
        public NotaktoBoard notaktoBoard = new NotaktoBoard();
        public GomokuBoard gomokuBoard = new GomokuBoard();
        string action { get; set; }
        public int placeBoard;
        public int placeRow;
        public int placeCol;

        // --------------------------------Context--------------------------
        public void DisplayWelcomeText()
        {
            Console.WriteLine("Welcome to the BoardGame Arcade! The best platform of game board.");
            Console.WriteLine();
        }

        public void DisplayLoadGameText()
        {
            Console.WriteLine("Would you like to (Load Game or New Game):");
            Console.WriteLine();

            action = Console.ReadLine();
            while (!(action == "Load Game" || action == "New Game"))
            {
                Console.WriteLine("Please enter a valid response:");
                Console.WriteLine();
                action = Console.ReadLine();
            }

        }

        public void DisplayGameTypeText()
        {
            Console.WriteLine("Choose your game type (PVP or PVC):");
            Console.WriteLine();
            action = Console.ReadLine();
            while (!(action == "PVP" || action == "PVC"))
            {
                Console.WriteLine("Please enter a valid game type:");
                action = Console.ReadLine();
            }

        }

        public void DisplayGameModeText()
        {
            Console.WriteLine("Choose your game mode (Notakto or Gomoku):");
            Console.WriteLine();
            action = Console.ReadLine(); 
            while (!(action == "Notakto" || action == "Gomoku"))
            {
                Console.WriteLine("Please enter a valid game mode:");
                action = Console.ReadLine();
            }
            switch (action)
            {
                case "Notakto":
                    gameMode = GameMode.Notakto;
                    break;
                case "Gomoku":
                    gameMode = GameMode.Gomoku;
                    break;
            }
        }

        public void GameFinished()
        {
            if (isFinish)
            {
                if (gameMode == GameMode.Gomoku)
                { TogglePlayer(); }
                Console.WriteLine();
                Console.WriteLine("{0} Win!!", currentPlayer.PlayerName);
                Console.WriteLine();
                CommandSystem exit = new Exit();
                exit.Run();
            }
        }

        // --------------------------------Move--------------------------

        public void StartPlaying()
        {
            while (isFinish != true)
            {
                if (currentPlayer.IsHuman)
                {
                    HumanMove();
                } else
                {
                    ComputerMove();
                }
            }
            
        }

        public void TogglePlayer()
        {
            if (currentPlayer == player1)
            {
                currentPlayer = player2;
            } else
            {
                currentPlayer = player1;
            }
        }

        public void ComputerMove()
        {
            switch (gameMode)
            {
                case GameMode.Notakto:
                    do
                    {
                        Random rnd = new Random();
                        placeBoard = rnd.Next(0, 3);
                        placeRow = rnd.Next(0, 3);
                        placeCol = rnd.Next(0, 3);
                        
                    } while (!(gameRule.IsMoveValid(placeBoard, placeRow, placeCol) == true && gameRule.IsBoardDead(placeBoard) == false));

                    break;
                case GameMode.Gomoku:
                    do
                    {
                        Random rnd = new Random();
                        placeBoard = 1;
                        placeRow = rnd.Next(0, 15);
                        placeCol = rnd.Next(0, 15);

                    } while (!(gameRule.IsMoveValid(placeBoard, placeRow, placeCol) == true));
                    break;
            }

            CommandSystem makeMove = new MakeMove(placeBoard, placeRow, placeCol, currentPlayer);
            makeMove.Run();
            Console.WriteLine("Computer move:");
            Console.WriteLine();
            notaktoBoard.UpdateBoard(MoveBackup.Instance);
            notaktoBoard.PrintBoard();
            TogglePlayer();

            if (gameRule.CheckIfWin())
            {
                isFinish = true;
                GameFinished();
            }
        }

        public void HumanMove()
        {
            Console.WriteLine("{0}, It's your turn.", currentPlayer.PlayerName);
            Console.WriteLine("Enter your action (Move, Redo, Undo, Save, Help, Exit).");
            Console.WriteLine();
            string action = Console.ReadLine();
            if (action == "Move" || action == "Redo" || action == "Undo" || action == "Save"|| action == "Help" || action == "Exit")
            {
                switch (action)
                {
                    case "Move":
                        Console.WriteLine("Which place do you want to move:");
                        Console.WriteLine();
                        string move = Console.ReadLine();
                        switch (gameMode)
                        {
                            case GameMode.Notakto:
                                try
                                {
                                    string[] Nosplits = move.Split(' ');
                                    placeBoard = Int32.Parse(Nosplits[0]) - 1;
                                    placeRow = Int32.Parse(Nosplits[1]) - 1;
                                    placeCol = Int32.Parse(Nosplits[2]) - 1;
                                    
                                    if (gameRule.IsBoardDead(placeBoard))
                                    {
                                        Console.WriteLine("Baord is dead, please choose another");
                                        Console.WriteLine();
                                        break;
                                    }

                                    if (gameRule.IsMoveValid(placeBoard, placeRow, placeCol))
                                    {
                                        CommandSystem makeMove = new MakeMove(placeBoard , placeRow , placeCol , currentPlayer); 
                                        makeMove.Run();
                                        TogglePlayer();

                                        notaktoBoard.UpdateBoard(MoveBackup.Instance);
                                        notaktoBoard.PrintBoard();

                                        if (gameRule.CheckIfWin())
                                        {
                                            isFinish = true;
                                            GameFinished();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("It's occupied, please choose a new one.");
                                        Console.WriteLine();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error: " + ex.Message);
                                    Console.WriteLine("The move you place is invalid.");
                                    Console.WriteLine();
                                }
                                break;

                            case GameMode.Gomoku:
                                try
                                {
                                    string[] Gosplits = move.Split(' ');
                                    placeBoard = 1 - 1;
                                    placeRow = Int32.Parse(Gosplits[0]) - 1;
                                    placeCol = Int32.Parse(Gosplits[1]) - 1;

                                    if (gameRule.IsMoveValid(placeBoard, placeRow, placeCol))
                                    {
                                        CommandSystem makeMove = new MakeMove(placeBoard, placeRow, placeCol, currentPlayer);
                                        makeMove.Run();
                                        TogglePlayer();

                                        gomokuBoard.UpdateBoard(MoveBackup.Instance);
                                        gomokuBoard.PrintBoard();

                                        if (gameRule.CheckIfWin())
                                        {

                                            isFinish = true;
                                            GameFinished();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("It's occupied, please choose a new one.");
                                        Console.WriteLine();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Error: " + ex.Message);
                                    Console.WriteLine("The move you place is invalid.");
                                    Console.WriteLine();
                                }
                                break;
                        }
                        break;
                    case "Redo":
                        CommandSystem redo = new RedoMove();
                        redo.Run();
                        switch (gameMode)
                        {
                            case GameMode.Notakto:
                                notaktoBoard.UpdateBoard(MoveBackup.Instance);
                                notaktoBoard.PrintBoard();
                                break;
                            case GameMode.Gomoku:
                                gomokuBoard.UpdateBoard(MoveBackup.Instance);
                                gomokuBoard.PrintBoard();
                                break;
                        }
                        break;
                    case "Undo":
                        CommandSystem undo = new UndoMove();
                        undo.Run();
                        switch (gameMode)
                        {
                            case GameMode.Notakto:
                                notaktoBoard.UndoBoard(MoveBackup.Instance);
                                notaktoBoard.PrintBoard();
                                break;
                            case GameMode.Gomoku:
                                gomokuBoard.UndoBoard(MoveBackup.Instance);
                                gomokuBoard.PrintBoard();
                                break;
                        }
                        break;
                    case "Save":
                        CommandSystem save = new SaveGame(gameMode);
                        save.Run();
                        break;
                    case "Help":
                        switch (gameMode)
                        {
                            case GameMode.Notakto:
                                HelpSystem Nohelp = new NotaktoHelpSystem();
                                Nohelp.DisplayContext();
                                break;
                            case GameMode.Gomoku:
                                HelpSystem Gohelp = new GomokuHelpSystem();
                                Gohelp.DisplayContext();
                                break;
                        }
                        break;
                    case "Exit":
                        CommandSystem exit = new Exit();
                        exit.Run();
                        break;
                }
            }
            else {
                Console.WriteLine("Please enter a valid action.");
                Console.WriteLine();
            }

        }
        // --------------------------------Setup--------------------------

        public void InitialGame(out GameSystem game)
        {
            bool isValid = true;
            game = new GameSystem();
            game.DisplayWelcomeText();
            game.DisplayGameModeText();
            while (isValid)
            {
                game.DisplayLoadGameText();
                switch (game.action)
                {
                    case "Load Game":
                        CommandSystem load = new LoadGame(game);
                        load.Run();
                        if (game.isFinish == false)
                        {
                            isValid = false;
                            switch (gameMode)
                            {
                                case GameMode.Notakto:
                                    game.gameRule = new NotaktoRule(notaktoBoard);
                                    notaktoBoard.UpdateBoard(MoveBackup.Instance);
                                    notaktoBoard.PrintBoard();
                                    break;
                                case GameMode.Gomoku:
                                    game.gameRule = new GomokuRule(gomokuBoard);
                                    gomokuBoard.UpdateBoard(MoveBackup.Instance);
                                    gomokuBoard.PrintBoard();
                                    break;
                            }
                        }
                        game.TogglePlayer();
                        break;
                    case "New Game":
                        game.DisplayGameTypeText();
                        switch (game.action)
                        {
                            case "PVP":
                                game.gameType = GameType.PVP;
                                break;
                            case "PVC":
                                game.gameType = GameType.PVC;
                                break;
                        }
                        game.CreatePlayer();
                        game.DisplayGameBoard();
                        isValid = false;
                        break;
                }
            }
            
        }

        // --------------------------------Player--------------------------

        public void CreatePlayer()
        {
            Console.WriteLine("Enter player1's name:");
            Console.WriteLine();
            string name1 = Console.ReadLine();
            switch (gameMode)
            {
                case GameMode.Notakto:
                    player1 = new HumanPlayer(name1, "P1", 'X');
                    break;
                case GameMode.Gomoku:
                    player1 = new HumanPlayer(name1, "P1", 'O');
                    break;
            }
            switch (gameType)
            {
                case GameType.PVP:
                    Console.WriteLine("Enter player2's name:");
                    Console.WriteLine();
                    string name2 = Console.ReadLine();
                    player2 = new HumanPlayer(name2, "P2", 'X');
                    break;
                case GameType.PVC:
                    player2 = new ComputerPlayer("Computer",'X');
                    break;
            }
        }

        // --------------------------------Rule--------------------------

        public void DisplayGameBoard()
        {
            switch (gameMode)
            {
                case GameMode.Notakto:
                    gameRule = new NotaktoRule(notaktoBoard);
                    notaktoBoard.PrintBoard();
                    notaktoBoard.InitializeBoards();
                    break;
                case GameMode.Gomoku:
                    gameRule = new GomokuRule(gomokuBoard);
                    gomokuBoard.PrintBoard();
                    gomokuBoard.InitializeBoards();
                    break;
            }
            currentPlayer = player1;
            isFinish = false;
        }
    }
}