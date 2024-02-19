using System;
using System.Collections.Generic;
using System.Linq;
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

        private void ResetBoard()
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
            InitializeGame();
            GameCount.Content = GameCountPrefix + TicTacGameCount++;
        }

        private void ExitGame_Click(object sender, RoutedEventArgs e)
        {
            if (game != null)
                if (game.CheckWin() == TicTacToeGame.TicTacResults.NoConclusion)
                {
                    MessageBoxResult doReset = MessageBox.Show("The game is not over yet.", "Game in progress.\nDo you want to reset the board in order to exit?", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                    switch (doReset)
                    {
                        case MessageBoxResult.Yes:
                            ResetBoard();
                            GameCount.Content = GameCountPrefix + TicTacGameCount--;
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
            Score =
            [
                new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player1, "N/A"),
                new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player2, "N/A"),
                new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Computer, "N/A")
            ];
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
                        Score[1] = new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player1, "1");
                    else
                        Score[1] = new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player1, (Convert.ToInt32(Score[0].Item2) + 1).ToString());
                    break;
                case TicTacToeGame.TicTacResults.Computer:
                    if (Score[2].Item2 == "N/A")
                        Score[2] = new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player1, "1");
                    else
                        Score[2] = new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player1, (Convert.ToInt32(Score[0].Item2) + 1).ToString());
                    break;
                case TicTacToeGame.TicTacResults.NoConclusion:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
        }
    }
}
