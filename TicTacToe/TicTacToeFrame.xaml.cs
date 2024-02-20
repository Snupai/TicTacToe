using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicTacToe
{
    /// <summary>
    /// Interaktionslogik für TicTacToeFrame.xaml
    /// </summary>
    public partial class TicTacToeFrame : Page
    {
        public TicTacToeFrame()
        {
            InitializeComponent();
        }

        TicTacToeGame game;
        int TicTacGameCount = 0;
        const string GameCountPrefix = "Games played ";
        List<Tuple<TicTacToeGame.Players, string>> Score =
            [
                new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player1, "N/A"),
                new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player2, "N/A"),
                new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Computer, "N/A")
            ]; // later implement loading from file
        string Ties = "Ties N/A";

        private void InitializeGame()
        {
            if (GameModeSingle.IsChecked == true)
                game = new TicTacToeGame(TicTacToeGame.Players.Player1, TicTacToeGame.Players.Computer);
            else if (GameModeMulti.IsChecked == true)
                game = new TicTacToeGame(TicTacToeGame.Players.Player1, TicTacToeGame.Players.Player2);
            else { throw new Exception(Name = "Select GameMode"); }

            ResetBoard();

            TopLeftButton.IsEnabled = true;
            TopCenterButton.IsEnabled = true;
            TopRightButton.IsEnabled = true;
            CenterLeftButton.IsEnabled = true;
            CenterCenterButton.IsEnabled = true;
            CenterRightButton.IsEnabled = true;
            BottomLeftButton.IsEnabled = true;
            BottomCenterButton.IsEnabled = true;
            BottomRightButton.IsEnabled = true;
        }

        private void LockButtons()
        {
            TopLeftButton.IsEnabled = false;
            TopCenterButton.IsEnabled = false;
            TopRightButton.IsEnabled = false;
            CenterLeftButton.IsEnabled = false;
            CenterCenterButton.IsEnabled = false;
            CenterRightButton.IsEnabled = false;
            BottomLeftButton.IsEnabled = false;
            BottomCenterButton.IsEnabled = false;
            BottomRightButton.IsEnabled = false;
        }

        private void ResetBoard()
        {
            TopLeft.Content = "";
            TopCenter.Content = "";
            TopRight.Content = "";
            CenterLeft.Content = "";
            CenterCenter.Content = "";
            CenterRight.Content = "";
            BottomLeft.Content = "";
            BottomCenter.Content = "";
            BottomRight.Content = "";
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            GameCount.Content = GameCountPrefix + ++TicTacGameCount;
            InitializeGame();
        }

        private void ExitGame_Click(object sender, RoutedEventArgs e)
        {
            if (game != null)
                if (game.CheckWin() == TicTacToeGame.TicTacResults.NoConclusion)
                {
                    MessageBoxResult doReset = MessageBox.Show("Game in progress.", "Do you want to reset the board in order to exit?", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                    switch (doReset)
                    {
                        case MessageBoxResult.Yes:
                            LockButtons();
                            ResetBoard();
                            GameCount.Content = GameCountPrefix + --TicTacGameCount;
                            game = null; // Reset the game dat
                            break;
                        case MessageBoxResult.No:
                            return;
                    }
                }

            MessageBoxResult result = MessageBox.Show("Would you like to save the game before exiting?", "Save game", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.No);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    SaveGame();
                    Environment.Exit(0);
                    break;
                case MessageBoxResult.No:
                    Environment.Exit(0);
                    break;
                case MessageBoxResult.Cancel:
                    // Do nothing
                    break;
            }
        }

        private void SaveGame()
        {
            // Implement game saving logic later
            // maybe json or csv
            // set TicTacGameCount to loaded game count
        }

        private void ResetScores_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Would you like to all Scores?", "Reset Scores", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Score =
                    [
                        new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player1, "N/A"),
                        new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player2, "N/A"),
                        new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Computer, "N/A")
                    ];

                    TicTacGameCount = 0;
                    Ties = "Ties N/A";
                    ComputerScore.Content = Score[2].Item2;
                    Player1Score.Content = Score[0].Item2;
                    Player2Score.Content = Score[1].Item2;
                    TiesScore.Content = Ties.Split(' ')[Ties.Split(' ').Length - 1];
                    GameCount.Content = GameCountPrefix + TicTacGameCount;
                    LockButtons();
                    ResetBoard();
                    break;
                case MessageBoxResult.No:
                    // Do nothing
                    break;
            }
        }

        private async void DisplayField()
        {
            try
            {
                char[,] board = game.Field;

                TcpIpSender sender = new TcpIpSender(IPAddress.Parse("192.168.123.6"), 50000);

                string message = $"{board[0, 0]};{board[0, 1]};{board[0, 2]}\n" +
                                 $"{board[1, 0]};{board[1, 1]};{board[1, 2]}\n" +
                                 $"{board[2, 0]};{board[2, 1]};{board[2, 2]}";

                await sender.SendenAsync(message);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void AddScore()
        {
            switch (game.CheckWin())
            {
                case TicTacToeGame.TicTacResults.Player1:
                    if (Score[0].Item2 == "N/A")
                        Score[0] = new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player1, "1");
                    else
                        Score[0] = new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player1, (Convert.ToInt32(Score[0].Item2) + 1).ToString());
                    break;
                case TicTacToeGame.TicTacResults.Player2:
                    if (Score[1].Item2 == "N/A")
                        Score[1] = new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player2, "1");
                    else
                        Score[1] = new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player2, (Convert.ToInt32(Score[1].Item2) + 1).ToString());
                    break;
                case TicTacToeGame.TicTacResults.Computer:
                    if (Score[2].Item2 == "N/A")
                        Score[2] = new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Computer, "1");
                    else
                        Score[2] = new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Computer, (Convert.ToInt32(Score[2].Item2) + 1).ToString());
                    break;
                case TicTacToeGame.TicTacResults.NoConclusion:
                    break;
                case TicTacToeGame.TicTacResults.Draw:
                    if (Ties.Split(' ')[Ties.Split(' ').Length - 1] == "N/A")
                        Ties = "Ties 1";
                    else
                        Ties = "Ties " + (Convert.ToInt32(Ties.Split(' ')[Ties.Split(' ').Length - 1]) + 1).ToString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            ComputerScore.Content = Score[2].Item2;
            Player1Score.Content = Score[0].Item2;
            Player2Score.Content = Score[1].Item2;
            TiesScore.Content = Ties.Split(' ')[Ties.Split(' ').Length - 1];

            DisplayField();
        }

        private void DoComputerTurn()
        {
            if (game.CheckWin() != TicTacToeGame.TicTacResults.NoConclusion)
            {
                LockButtons();
                AddScore();
                return;
            }
            game.SetFieldComputer();
            switch (game.LastMove)
            {
                case { X: 1, Y: 1 }:
                    TopLeft.Content = "O";
                    TopLeftButton.IsEnabled = false;
                    break;
                case { X: 1, Y: 2 }:
                    TopCenter.Content = "O";
                    TopCenterButton.IsEnabled = false;
                    break;
                case { X: 1, Y: 3 }:
                    TopRight.Content = "O";
                    TopRightButton.IsEnabled = false;
                    break;
                case { X: 2, Y: 1 }:
                    CenterLeft.Content = "O";
                    CenterLeftButton.IsEnabled = false;
                    break;
                case { X: 2, Y: 2 }:
                    CenterCenter.Content = "O";
                    CenterCenterButton.IsEnabled = false;
                    break;
                case { X: 2, Y: 3 }:
                    CenterRight.Content = "O";
                    CenterRightButton.IsEnabled = false;
                    break;
                case { X: 3, Y: 1 }:
                    BottomLeft.Content = "O";
                    BottomLeftButton.IsEnabled = false;
                    break;
                case { X: 3, Y: 2 }:
                    BottomCenter.Content = "O";
                    BottomCenterButton.IsEnabled = false;
                    break;
                case { X: 3, Y: 3 }:
                    BottomRight.Content = "O";
                    BottomRightButton.IsEnabled = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (game.CheckWin() != TicTacToeGame.TicTacResults.NoConclusion)
            {
                LockButtons();
                AddScore();
                return;
            }
        }

        private void TopLeft_Click(object sender, RoutedEventArgs e)
        {
            if (game.Turn >= 5)
            {
                if (game.CheckWin() != TicTacToeGame.TicTacResults.NoConclusion)
                {
                    AddScore();
                    return;
                }
            }

            if (game.Turn % 2 == 0)
            {
                game.SetField(new System.Drawing.Point(1, 1), 'X');
                TopLeft.Content = "X";
            }
            else
            {
                game.SetField(new System.Drawing.Point(1, 1), 'O');
                TopLeft.Content = "O";
            }
            TopLeftButton.IsEnabled = false;
            if (GameModeSingle.IsChecked == true)
            {
                DoComputerTurn();
            }
            DisplayField();
        }

        private void TopMiddle_Click(object sender, RoutedEventArgs e)
        {
            if (game.Turn >= 5)
            {
                if (game.CheckWin() != TicTacToeGame.TicTacResults.NoConclusion)
                {
                    AddScore();
                    return;
                }
            }

            if (game.Turn % 2 == 0)
            {
                game.SetField(new System.Drawing.Point(1, 2), 'X');
                TopCenter.Content = "X";
            }
            else
            {
                game.SetField(new System.Drawing.Point(1, 2), 'O');
                TopCenter.Content = "O";
            }
            TopCenterButton.IsEnabled = false;
            if (GameModeSingle.IsChecked == true)
            {
                DoComputerTurn();
            }
            DisplayField();
        }

        private void TopRight_Click(object sender, RoutedEventArgs e)
        {
            if (game.Turn >= 5)
            {
                if (game.CheckWin() != TicTacToeGame.TicTacResults.NoConclusion)
                {
                    AddScore();
                    return;
                }
            }

            if (game.Turn % 2 == 0)
            {
                game.SetField(new System.Drawing.Point(1, 3), 'X');
                TopRight.Content = "X";
            }
            else
            {
                game.SetField(new System.Drawing.Point(1, 3), 'O');
                TopRight.Content = "O";
            }
            TopRightButton.IsEnabled = false;
            if (GameModeSingle.IsChecked == true)
            {
                DoComputerTurn();
            }
            DisplayField();
        }

        private void MiddleLeft_Click(object sender, RoutedEventArgs e)
        {
            if (game.Turn >= 5)
            {
                if (game.CheckWin() != TicTacToeGame.TicTacResults.NoConclusion)
                {
                    AddScore();
                    return;
                }
            }

            if (game.Turn % 2 == 0)
            {
                game.SetField(new System.Drawing.Point(2, 1), 'X');
                CenterLeft.Content = "X";
            }
            else
            {
                game.SetField(new System.Drawing.Point(2, 1), 'O');
                CenterLeft.Content = "O";
            }
            CenterLeftButton.IsEnabled = false;
            if (GameModeSingle.IsChecked == true)
            {
                DoComputerTurn();
            }
            DisplayField();
        }

        private void MiddleMiddle_Click(object sender, RoutedEventArgs e)
        {
            if (game.Turn >= 5)
            {
                if (game.CheckWin() != TicTacToeGame.TicTacResults.NoConclusion)
                {
                    AddScore();
                    return;
                }
            }

            if (game.Turn % 2 == 0)
            {
                game.SetField(new System.Drawing.Point(2, 2), 'X');
                CenterCenter.Content = "X";
            }
            else
            {
                game.SetField(new System.Drawing.Point(2, 2), 'O');
                CenterCenter.Content = "O";
            }
            CenterCenterButton.IsEnabled = false;
            if (GameModeSingle.IsChecked == true)
            {
                DoComputerTurn();
            }
            DisplayField();
        }

        private void MiddleRight_Click(object sender, RoutedEventArgs e)
        {
            if (game.Turn >= 5)
            {
                if (game.CheckWin() != TicTacToeGame.TicTacResults.NoConclusion)
                {
                    AddScore();
                    return;
                }
            }

            if (game.Turn % 2 == 0)
            {
                game.SetField(new System.Drawing.Point(2, 3), 'X');
                CenterRight.Content = "X";
            }
            else
            {
                game.SetField(new System.Drawing.Point(2, 3), 'O');
                CenterRight.Content = "O";
            }
            CenterRightButton.IsEnabled = false;
            if (GameModeSingle.IsChecked == true)
            {
                DoComputerTurn();
            }
            DisplayField();
        }

        private void BottomLeft_Click(object sender, RoutedEventArgs e)
        {
            if (game.Turn >= 5)
            {
                if (game.CheckWin() != TicTacToeGame.TicTacResults.NoConclusion)
                {
                    AddScore();
                    return;
                }
            }

            if (game.Turn % 2 == 0)
            {
                game.SetField(new System.Drawing.Point(3, 1), 'X');
                BottomLeft.Content = "X";
            }
            else
            {
                game.SetField(new System.Drawing.Point(3, 1), 'O');
                BottomLeft.Content = "O";
            }
            BottomLeftButton.IsEnabled = false;
            if (GameModeSingle.IsChecked == true)
            {
                DoComputerTurn();
            }
            DisplayField();
        }

        private void BottomMiddle_Click(object sender, RoutedEventArgs e)
        {
            if (game.Turn >= 5)
            {
                if (game.CheckWin() != TicTacToeGame.TicTacResults.NoConclusion)
                {
                    AddScore();
                    return;
                }
            }

            if (game.Turn % 2 == 0)
            {
                game.SetField(new System.Drawing.Point(3, 2), 'X');
                BottomCenter.Content = "X";
            }
            else
            {
                game.SetField(new System.Drawing.Point(3, 2), 'O');
                BottomCenter.Content = "O";
            }
            BottomCenterButton.IsEnabled = false;
            if (GameModeSingle.IsChecked == true)
            {
                DoComputerTurn();
            }
            DisplayField();
        }

        private void BottomRight_Click(object sender, RoutedEventArgs e)
        {
            if (game.Turn >= 5)
            {
                if (game.CheckWin() != TicTacToeGame.TicTacResults.NoConclusion)
                {
                    AddScore();
                    return;
                }
            }

            if (game.Turn % 2 == 0)
            {
                game.SetField(new System.Drawing.Point(3, 3), 'X');
                BottomRight.Content = "X";
            }
            else
            {
                game.SetField(new System.Drawing.Point(3, 3), 'O');
                BottomRight.Content = "O";
            }
            BottomRightButton.IsEnabled = false;
            if (GameModeSingle.IsChecked == true)
            {
                DoComputerTurn();
            }
            DisplayField();
        }
    }
}
