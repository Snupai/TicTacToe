using System.Drawing;
using System.Windows.Documents;

namespace TicTacToe
{
    class TicTacToeGame
    {
        public char[,] Field { get; private set; }
        public int Turn { get; private set; }
        public Point LastMove { get; private set; }
        public List<Point> MovesTaken { get; private set; }
        public Players Player1 { get; private set; }
        public Players Player2 { get; private set; }
        private bool isSinglePlayer;
        public enum Players
        {
            Computer,
            Player1,
            Player2
        }

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

        public enum TicTacResults
        {
            Draw,
            Computer,
            Player1,
            Player2,
            NoConclusion = 100
        }

        // only call after first 5 turns since can't win under 5 turns
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

        public void SetField(Point move, char value)
        {
            int row = move.X;
            int column = move.Y;
            if (row < 1 || row > 3 || column < 1 || column > 3) { throw new ArgumentException("Row and column must be between 1 and 3"); }
            if (value != 'X' && value != 'O') { throw new ArgumentException("Value must be 'X' or 'O'"); }
            if (Field[row - 1, column - 1] != ' ') { throw new ArgumentException("Field already taken"); }
            LastMove = move;
            MovesTaken.Add(move);
            Field[row - 1, column - 1] = value;
            Turn++;
        }

        public void SetFieldComputer()
        {
            if (!isSinglePlayer) return;

            Point move = FindBestMove(Field);

            if (Player1 == Players.Computer) { SetField(move, 'X'); }
            else if (Player2 == Players.Computer) { SetField(move, 'O'); }
        }



        //--------------------------------------------------------------------------------
        
        // This function returns true if there are moves 
        // remaining on the board. It returns false if 
        // there are no moves left to play. 
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

        // This is the minimax function. It considers all 
        // the possible ways the game can go and returns 
        // the value of the board 
        private int Minimax(char[,] board, int depth, bool isMax)
        {
            int score = Evaluate(board);

            if (score == 10)
                return score;

            if (score == -10)
                return score;

            if (!HasMovesLeft(board))
                return 0;

            if (isMax)
            {
                int best = int.MinValue;

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

        // This will return the best possible 
        // move for the Computer 
        private Point FindBestMove(char[,] board)
        {
            int bestValue = int.MinValue;
            Point bestMove = new Point { X = -1, Y = -1 };

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == ' ')
                    {
                        board[i, j] = 'O';
                        int moveValue = Minimax(board, 0, false);
                        board[i, j] = ' ';

                        if (moveValue > bestValue)
                        {
                            bestMove = new Point { X = i, Y = j };
                            bestValue = moveValue;
                        }
                    }
                }
            }

            bestMove.X++;
            bestMove.Y++;

            return bestMove;
        }

        //--------------------------------------------------------------------------------
    }
}
