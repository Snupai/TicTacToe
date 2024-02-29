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
        /// <summary>
        /// Constructor for the TicTacToeFrame class
        /// </summary>
        public TicTacToeFrame()
        {
            InitializeComponent();
        }

        // Initialize a new TicTacToeGame instance
        TicTacToeGame game;

        // Number of TicTacToe games played
        int TicTacGameCount = 0;

        // Prefix for the game count
        const string GameCountPrefix = "Games played ";

        // List to store the scores of the players and the computer
        List<Tuple<TicTacToeGame.Players, string>> Score =
        [
            new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player1, "N/A"),
            new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player2, "N/A"),
            new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Computer, "N/A")
        ];

        // Initial value for the number of ties
        string Ties = "Ties N/A";

        private void InitializeGame()
        {
            // Initialize the game based on the selected game mode
            if (GameModeSingle.IsChecked == true)
                game = new TicTacToeGame(TicTacToeGame.Players.Player1, TicTacToeGame.Players.Computer);
            else if (GameModeMulti.IsChecked == true)
                game = new TicTacToeGame(TicTacToeGame.Players.Player1, TicTacToeGame.Players.Player2);
            else { throw new Exception(Name = "Select GameMode"); }

            // Reset the game board
            ResetBoard();

            // Enable all the game buttons
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

        /// <summary>
        /// Disables all the buttons on the game board.
        /// </summary>
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

        /// <summary>
        /// Resets the content of all the cells on the game board.
        /// </summary>
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

        /// <summary>
        /// Clears the display field and initializes a new game.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            ClearDisplayField();
            GameCount.Content = GameCountPrefix + ++TicTacGameCount;
            InitializeGame();
        }

        private void ExitGame_Click(object sender, RoutedEventArgs e)
        {
            // Check if there is an active game
            if (game != null)
            {
                // Check if the game has no conclusion
                if (game.CheckWin() == TicTacToeGame.TicTacResults.NoConclusion)
                {
                    // Prompt the user to reset the board before exiting
                    MessageBoxResult doReset = MessageBox.Show("Game in progress.", "Do you want to reset the board in order to exit?", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                    switch (doReset)
                    {
                        case MessageBoxResult.Yes:
                            LockButtons();
                            ResetBoard();
                            ClearDisplayField();
                            GameCount.Content = GameCountPrefix + --TicTacGameCount;
                            game = null; // Reset the game data
                            break;
                        case MessageBoxResult.No:
                            return;
                    }
                }
            }

            // Prompt the user to save the game before exiting
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
            /* TODO: Implement game saving logic later
            maybe json or csv
            set TicTacGameCount to loaded game count*/
        }

        /// <summary>
        /// Handles the click event for resetting the scores
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">The event arguments</param>
        private void ResetScores_Click(object sender, RoutedEventArgs e)
        {
            // Prompt the user for confirmation
            MessageBoxResult result = MessageBox.Show("Would you like to all Scores?", "Reset Scores", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    // Reset the scores and game count
                    Score =
                    [
                        new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player1, "N/A"),
                        new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player2, "N/A"),
                        new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Computer, "N/A")
                    ];

                    TicTacGameCount = 0;
                    Ties = "Ties N/A";
                    // Update the UI with the reset scores and game count
                    ComputerScore.Content = Score[2].Item2;
                    Player1Score.Content = Score[0].Item2;
                    Player2Score.Content = Score[1].Item2;
                    TiesScore.Content = Ties.Split(' ')[Ties.Split(' ').Length - 1];
                    GameCount.Content = GameCountPrefix + TicTacGameCount;
                    // Lock the buttons, reset the board, and clear the display field
                    LockButtons();
                    ResetBoard();
                    ClearDisplayField();
                    break;
                case MessageBoxResult.No:
                    // Do nothing
                    break;
            }
        }

        private async void ClearDisplayField()
        {
            try
            {
                // Initialize a 3x3 empty character board
                char[,] board = { { ' ', ' ', ' ' }, { ' ', ' ', ' ' }, { ' ', ' ', ' ' } };

                // Create a TcpIpSender instance with the specified IP address and port
                TcpIpSender sender = new TcpIpSender(IPAddress.Parse("192.168.42.6"), 50000); //Raspi: 192.168.123.6 for testing: 127.0.0.1

                // Construct the message to be sent containing the board state
                string message = $"{board[0, 0]};{board[0, 1]};{board[0, 2]}\n" +
                                 $"{board[1, 0]};{board[1, 1]};{board[1, 2]}\n" +
                                 $"{board[2, 0]};{board[2, 1]};{board[2, 2]}";

                // Send the message asynchronously
                await sender.SendenAsync(message);
                Thread.Sleep(2000); // to allow drawing of the board
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Display the game board field and send it via TCP/IP.
        /// </summary>
        private async void DisplayField()
        {
            try
            {
                // Get the game board field
                char[,] board = game.Field;

                // Initialize a TCP/IP sender with the given IP address and port
                TcpIpSender sender = new TcpIpSender(IPAddress.Parse("192.168.42.6"), 50000); //Raspi: 192.168.123.6 for testing: 127.0.0.1

                // Construct the message to be sent
                string message = $"{board[0, 0]};{board[0, 1]};{board[0, 2]}\n" +
                                 $"{board[1, 0]};{board[1, 1]};{board[1, 2]}\n" +
                                 $"{board[2, 0]};{board[2, 1]};{board[2, 2]}";

                // Send the message via TCP/IP
                await sender.SendenAsync(message);

                // Delay to allow time for drawing the board
                Thread.Sleep(3000);
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        /// Updates the score based on the game result and displays it on the UI
        /// </summary>
        private void AddScore()
        {
            switch (game.CheckWin())
            {
                // Update Player 1 score
                case TicTacToeGame.TicTacResults.Player1:
                    if (Score[0].Item2 == "N/A")
                        Score[0] = new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player1, "1");
                    else
                        Score[0] = new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player1, (Convert.ToInt32(Score[0].Item2) + 1).ToString());
                    break;
                // Update Player 2 score
                case TicTacToeGame.TicTacResults.Player2:
                    if (Score[1].Item2 == "N/A")
                        Score[1] = new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player2, "1");
                    else
                        Score[1] = new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Player2, (Convert.ToInt32(Score[1].Item2) + 1).ToString());
                    break;
                // Update Computer score
                case TicTacToeGame.TicTacResults.Computer:
                    if (Score[2].Item2 == "N/A")
                        Score[2] = new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Computer, "1");
                    else
                        Score[2] = new Tuple<TicTacToeGame.Players, string>(TicTacToeGame.Players.Computer, (Convert.ToInt32(Score[2].Item2) + 1).ToString());
                    break;
                // No conclusion, do nothing
                case TicTacToeGame.TicTacResults.NoConclusion:
                    break;
                // Update ties score
                case TicTacToeGame.TicTacResults.Draw:
                    if (Ties.Split(' ')[Ties.Split(' ').Length - 1] == "N/A")
                        Ties = "Ties 1";
                    else
                        Ties = "Ties " + (Convert.ToInt32(Ties.Split(' ')[Ties.Split(' ').Length - 1]) + 1).ToString();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            // Update UI with the latest scores
            ComputerScore.Content = Score[2].Item2;
            Player1Score.Content = Score[0].Item2;
            Player2Score.Content = Score[1].Item2;
            TiesScore.Content = Ties.Split(' ')[Ties.Split(' ').Length - 1];

            // Display the updated game field
            DisplayField();
        }

        /// <summary>
        /// Perform the computer's turn in the Tic Tac Toe game.
        /// </summary>
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
                    // Update TopLeft cell with "O" and disable the button
                    TopLeft.Content = "O";
                    TopLeftButton.IsEnabled = false;
                    break;
                case { X: 1, Y: 2 }:
                    // Update TopCenter cell with "O" and disable the button
                    TopCenter.Content = "O";
                    TopCenterButton.IsEnabled = false;
                    break;
                // ... (repeat for other cases)
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

        /// <summary>
        /// Handles the click event for the TopLeft button.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void TopLeft_Click(object sender, RoutedEventArgs e)
        {
            // Check if it's the 5th turn or later
            if (game.Turn >= 5)
            {
                // Check if there's a winner
                if (game.CheckWin() != TicTacToeGame.TicTacResults.NoConclusion)
                {
                    // Add score and exit the method
                    AddScore();
                    return;
                }
            }

            // Determine the player's move based on the turn number
            if (game.Turn % 2 == 0)
            {
                // Set the field to 'X' and update the button content
                game.SetField(new System.Drawing.Point(1, 1), 'X');
                TopLeft.Content = "X";
            }
            else
            {
                // Set the field to 'O' and update the button content
                game.SetField(new System.Drawing.Point(1, 1), 'O');
                TopLeft.Content = "O";
            }

            // Disable the button after the move
            TopLeftButton.IsEnabled = false;

            // If the single player mode is selected, let the computer make a move
            if (GameModeSingle.IsChecked == true)
            {
                DoComputerTurn();
            }

            // Update the display of the game field
            DisplayField();
        }

        /// <summary>
        /// Handles the click event for the top middle button.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void TopMiddle_Click(object sender, RoutedEventArgs e)
        {
            if (game.Turn >= 5)
            {
                if (game.CheckWin() != TicTacToeGame.TicTacResults.NoConclusion)
                {
                    AddScore(); // Add score if there is a win
                    return;
                }
            }

            if (game.Turn % 2 == 0)
            {
                game.SetField(new System.Drawing.Point(1, 2), 'X'); // Set field to 'X'
                TopCenter.Content = "X"; // Update UI with 'X'
            }
            else
            {
                game.SetField(new System.Drawing.Point(1, 2), 'O'); // Set field to 'O'
                TopCenter.Content = "O"; // Update UI with 'O'
            }
            TopCenterButton.IsEnabled = false; // Disable the button
            if (GameModeSingle.IsChecked == true)
            {
                DoComputerTurn(); // Perform computer turn in single player mode
            }
            DisplayField(); // Update the game board display
        }

        /// <summary>
        /// Handles the click event for the top right button.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void TopRight_Click(object sender, RoutedEventArgs e)
        {
            // Check if it's the 5th turn or later
            if (game.Turn >= 5)
            {
                // Check if there's a win
                if (game.CheckWin() != TicTacToeGame.TicTacResults.NoConclusion)
                {
                    // Add score and exit
                    AddScore();
                    return;
                }
            }

            // Set the field based on the current turn
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

            // Disable the button
            TopRightButton.IsEnabled = false;

            // If it's single player mode, make the computer's move
            if (GameModeSingle.IsChecked == true)
            {
                DoComputerTurn();
            }

            // Update the display
            DisplayField();
        }

        /// <summary>
        /// Handles the click event for the middle left button in the TicTacToe game.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void MiddleLeft_Click(object sender, RoutedEventArgs e)
        {
            // Check if it's the 5th turn or later
            if (game.Turn >= 5)
            {
                // Check if there's a winner
                if (game.CheckWin() != TicTacToeGame.TicTacResults.NoConclusion)
                {
                    AddScore(); // Add score and return
                    return;
                }
            }

            // Set the field based on the current turn
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
            CenterLeftButton.IsEnabled = false; // Disable the button
            if (GameModeSingle.IsChecked == true) // Check if single player mode is enabled
            {
                DoComputerTurn(); // Perform the computer's turn
            }
            DisplayField(); // Update the game field display
        }

        /// <summary>
        /// Handles the click event for the middle button in the Tic Tac Toe game.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void MiddleMiddle_Click(object sender, RoutedEventArgs e)
        {
            // Check if it's at least the 5th turn and there is a winner
            if (game.Turn >= 5 && game.CheckWin() != TicTacToeGame.TicTacResults.NoConclusion)
            {
                AddScore();
                return;
            }

            // Set the field and display the symbol based on the current turn
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

            // Disable the button and handle computer turn in single player mode
            CenterCenterButton.IsEnabled = false;
            if (GameModeSingle.IsChecked == true)
            {
                DoComputerTurn();
            }

            // Update the game field display
            DisplayField();
        }

        /// <summary>
        /// Handles the click event for the middle right button
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">The event arguments</param>
        private void MiddleRight_Click(object sender, RoutedEventArgs e)
        {
            // Check if it's the 5th turn or later and a winner has already been determined
            if (game.Turn >= 5 && game.CheckWin() != TicTacToeGame.TicTacResults.NoConclusion)
            {
                AddScore();
                return;
            }

            // Set the field and display the corresponding symbol based on the turn
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

            // If in single player mode, make the computer's turn
            if (GameModeSingle.IsChecked == true)
            {
                DoComputerTurn();
            }
            DisplayField(); // Update the game board display
        }

        /// <summary>
        /// Handles the click event for the BottomLeft button
        /// </summary>
        /// <param name="sender">The object that raised the event</param>
        /// <param name="e">The event arguments</param>
        private void BottomLeft_Click(object sender, RoutedEventArgs e)
        {
            // Check if the game has reached at least the 5th turn
            if (game.Turn >= 5)
            {
                // Check for a win and add score if there is a conclusion
                if (game.CheckWin() != TicTacToeGame.TicTacResults.NoConclusion)
                {
                    AddScore();
                    return;
                }
            }

            // Set the field and display the appropriate symbol based on the turn
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

            // Disable the button to prevent further clicks
            BottomLeftButton.IsEnabled = false;

            // If single player mode is selected, let the computer take its turn
            if (GameModeSingle.IsChecked == true)
            {
                DoComputerTurn();
            }

            // Update the display of the game field
            DisplayField();
        }

        /// <summary>
        /// Handles the click event for the bottom middle button.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void BottomMiddle_Click(object sender, RoutedEventArgs e)
        {
            // Check if it's the 5th turn or later and there's a winner, then add score and return
            if (game.Turn >= 5 && game.CheckWin() != TicTacToeGame.TicTacResults.NoConclusion)
            {
                AddScore();
                return;
            }

            // Set the field and display the respective symbol based on the turn
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

            // Disable the button and handle computer turn for single player mode
            BottomCenterButton.IsEnabled = false;
            if (GameModeSingle.IsChecked == true)
            {
                DoComputerTurn();
            }

            // Update the display of the game field
            DisplayField();
        }

        /// <summary>
        /// Handles the click event for the BottomRight button.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments.</param>
        private void BottomRight_Click(object sender, RoutedEventArgs e)
        {
            // Check if it's the 5th turn or later
            if (game.Turn >= 5)
            {
                // Check if there's a winner
                if (game.CheckWin() != TicTacToeGame.TicTacResults.NoConclusion)
                {
                    // Add score and exit
                    AddScore();
                    return;
                }
            }

            // Set the field based on the current turn
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

            // Disable the button to prevent further clicks
            BottomRightButton.IsEnabled = false;

            // If single-player mode, let the computer take a turn
            if (GameModeSingle.IsChecked == true)
            {
                DoComputerTurn();
            }

            // Update the display
            DisplayField();
        }
    }
}
