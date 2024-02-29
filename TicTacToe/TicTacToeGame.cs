using System.Drawing;
using System.Windows.Documents;

namespace TicTacToe
{
    class TicTacToeGame
    {
        /// <summary>
        /// Represents the game field.
        /// </summary>
        public char[,] Field { get; private set; }

        /// <summary>
        /// Represents the current turn number.
        /// </summary>
        public int Turn { get; private set; }

        /// <summary>
        /// Represents the last move made on the field.
        /// </summary>
        public Point LastMove { get; private set; }

        /// <summary>
        /// Represents the list of moves taken in the game.
        /// </summary>
        public List<Point> MovesTaken { get; private set; }

        /// <summary>
        /// Represents the first player.
        /// </summary>
        public Players Player1 { get; private set; }

        /// <summary>
        /// Represents the second player.
        /// </summary>
        public Players Player2 { get; private set; }

        /// <summary>
        /// Indicates whether the game is a single player game.
        /// </summary>
        private bool isSinglePlayer;

        /// <summary>
        /// Enum representing the players in the game.
        /// </summary>
        public enum Players
        {
            /// <summary>
            /// Represents the computer player.
            /// </summary>
            Computer,
            /// <summary>
            /// Represents the first human player.
            /// </summary>
            Player1,
            /// <summary>
            /// Represents the second human player.
            /// </summary>
            Player2
        }

        /// <summary>
        /// Initializes a new instance of the TicTacToeGame class.
        /// </summary>
        /// <param name="player1">The first player.</param>
        /// <param name="player2">The second player.</param>
        public TicTacToeGame(Players player1, Players player2)
        {
            Field = new char[3, 3] { { ' ', ' ', ' ' }, { ' ', ' ', ' ' }, { ' ', ' ', ' ' } };
            LastMove = new Point(0, 0);
            Turn = 0;
            Player1 = player1;
            Player2 = player2;
            MovesTaken = new List<Point>();
            if (Player1 == Players.Computer || Player2 == Players.Computer) { isSinglePlayer = true; }
        }

        /// <summary>
        /// Enum representing the result of a TicTacToe game.
        /// </summary>
        public enum TicTacResults
        {
            /// <summary>
            /// The game ended in a draw.
            /// </summary>
            Draw,
            /// <summary>
            /// The computer won the game.
            /// </summary>
            Computer,
            /// <summary>
            /// Player 1 won the game.
            /// </summary>
            Player1,
            /// <summary>
            /// Player 2 won the game.
            /// </summary>
            Player2,
            /// <summary>
            /// The game has no conclusion yet.
            /// </summary>
            NoConclusion = 100
        }

        /// <summary>
        /// Check for a win condition in a Tic Tac Toe game.
        /// </summary>
        /// <returns>The result of the game: Player1, Player2, Computer, Draw, or NoConclusion.</returns>
        public TicTacResults CheckWin()
        {
            // Check for draw
            if (Field.Cast<char>().All(x => x != ' ')) return TicTacResults.Draw;

            // Check for horizontal wins
            for (int i = 0; i < 3; i++)
            {
                if (Field[i, 0] == Field[i, 1] && Field[i, 1] == Field[i, 2])
                {
                    if (Field[i, 0] != ' ')
                    {
                        if (Field[i, 0] == 'X')
                        {
                            return TicTacResults.Player1;
                        }
                        else
                        {
                            if (isSinglePlayer) { return TicTacResults.Computer; }
                            else { return TicTacResults.Player2; }
                        }
                    }
                }
            }

            // Check for vertical wins
            for (int i = 0; i < 3; i++)
            {
                if (Field[0, i] == Field[1, i] && Field[1, i] == Field[2, i])
                {
                    if (Field[0, i] != ' ')
                    {
                        if (Field[0, i] == 'X')
                        {
                            return TicTacResults.Player1;
                        }
                        else
                        {
                            if (isSinglePlayer) { return TicTacResults.Computer; }
                            else { return TicTacResults.Player2; }
                        }
                    }
                }
            }

            // Check for diagonal wins
            if ((Field[0, 0] == Field[1, 1] && Field[1, 1] == Field[2, 2]) || (Field[0, 2] == Field[1, 1] && Field[1, 1] == Field[2, 0]))
            {
                if (Field[1, 1] != ' ')
                {
                    if (Field[1, 1] == 'X')
                    {
                        return TicTacResults.Player1;
                    }
                    else
                    {
                        if (isSinglePlayer) { return TicTacResults.Computer; }
                        else { return TicTacResults.Player2; }
                    }
                }
            }

            return TicTacResults.NoConclusion;
        }

        /// <summary>
        /// Sets the field at the specified position with the given value.
        /// </summary>
        /// <param name="move">The position to set the field at.</param>
        /// <param name="value">The value to set the field to ('X' or 'O').</param>
        public void SetField(Point move, char value)
        {
            // Check if the given row and column are within the valid range
            int row = move.X;
            int column = move.Y;
            if (row < 1 || row > 3 || column < 1 || column > 3)
            {
                throw new ArgumentException("Row and column must be between 1 and 3");
            }

            // Check if the value is valid
            if (value != 'X' && value != 'O')
            {
                throw new ArgumentException("Value must be 'X' or 'O'");
            }

            // Check if the field is already taken
            if (Field[row - 1, column - 1] != ' ')
            {
                throw new ArgumentException("Field already taken");
            }

            // Update the last move and moves taken, and set the field value
            LastMove = move;
            MovesTaken.Add(move);
            Field[row - 1, column - 1] = value;
            Turn++;
        }

        /// <summary>
        /// Sets the field for the computer player.
        /// </summary>
        public void SetFieldComputer()
        {
            if (!isSinglePlayer) return;

            // Find the best move for the computer player
            Point move = FindBestMove(Field);

            if (Player1 == Players.Computer) { SetField(move, 'X'); }
            else if (Player2 == Players.Computer) { SetField(move, 'O'); }
        }



        //--------------------------------------------------------------------------------


        /// <summary>
        /// Checks if there are any available moves left on the board.
        /// </summary>
        /// <param name="board">The game board represented as a 2D array of characters</param>
        /// <returns>True if there are available moves, false otherwise</returns>
        private bool HasMovesLeft(char[,] board)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row, col] == ' ')
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // https://www.neverstopbuilding.com/blog/minimax
        /// <summary>
        /// Evaluates the tic-tac-toe board to check for a win condition.
        /// </summary>
        /// <param name="board">The 3x3 tic-tac-toe board</param>
        /// <returns>
        /// +10 if 'O' wins, -10 if 'X' wins, 0 if no win condition
        /// </returns>
        private int Evaluate(char[,] board)
        {
            // Checking for Rows or Columns for X or O victory.
            for (int i = 0; i < 3; i++)
            {
                // Check Rows
                if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                {
                    if (board[i, 0] == 'O')
                        return +10;
                    else if (board[i, 0] == 'X')
                        return -10;
                }
                // Check Columns
                if (board[0, i] == board[1, i] && board[1, i] == board[2, i])
                {
                    if (board[0, i] == 'O')
                        return +10;
                    else if (board[0, i] == 'X')
                        return -10;
                }
            }

            // Checking for Diagonals for X or O victory. 
            if ((board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]) ||
                (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0]))
            {
                if (board[1, 1] == 'O')
                    return +10;
                else if (board[1, 1] == 'X')
                    return -10;
            }

            // No win condition
            return 0;
        }


        /// <summary>
        /// Implementation of the Minimax algorithm for Tic Tac Toe game
        /// </summary>
        /// <param name="board">The current state of the Tic Tac Toe board</param>
        /// <param name="depth">The depth of the recursion</param>
        /// <param name="isMax">A boolean indicating whether it's the maximizing player's turn</param>
        /// <returns>The best score for the current board state</returns>
        private int Minimax(char[,] board, int depth, bool isMax)
        {
            // Evaluate the current state of the board
            int score = Evaluate(board);

            // Return the score if a winning move is found
            if (score == 10 || score == -10)
                return score;

            // Return 0 if there are no more moves left
            if (!HasMovesLeft(board))
                return 0;

            if (isMax)
            {
                int best = int.MinValue;

                // Find the best possible move for the maximizing player
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (board[i, j] == ' ')
                        {
                            board[i, j] = 'O';
                            best = Math.Max(best, Minimax(board, depth + 1, !isMax));
                            board[i, j] = ' ';
                        }
                    }
                }
                return best;
            }
            else
            {
                int best = int.MaxValue;

                // Find the best possible move for the minimizing player
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (board[i, j] == ' ')
                        {
                            board[i, j] = 'X';
                            best = Math.Min(best, Minimax(board, depth + 1, !isMax));
                            board[i, j] = ' ';
                        }
                    }
                }
                return best;
            }
        }

        /// <summary>
        /// Finds the best move for the AI player on the given Tic Tac Toe board.
        /// </summary>
        /// <param name="board">The current state of the Tic Tac Toe board</param>
        /// <returns>The best move for the AI player as a Point object</returns>
        private Point FindBestMove(char[,] board)
        {
            // Initialize the best value and best move
            int bestValue = int.MinValue;
            Point bestMove = new Point { X = -1, Y = -1 };

            // Iterate through the board to find the best move
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Check if the current cell is empty
                    if (board[i, j] == ' ')
                    {
                        // Try placing the AI player's symbol in the current cell
                        board[i, j] = 'O';

                        // Use the Minimax algorithm to calculate the value of the move
                        int moveValue = Minimax(board, 0, false);

                        // Undo the move
                        board[i, j] = ' ';

                        // Update the best move if the current move is better
                        if (moveValue > bestValue)
                        {
                            bestMove = new Point { X = i, Y = j };
                            bestValue = moveValue;
                        }
                    }
                }
            }

            // Increment the coordinates of the best move to match the board's indexing
            bestMove.X++;
            bestMove.Y++;

            // Return the best move for the AI player
            return bestMove;
        }

        //--------------------------------------------------------------------------------
    }
}
