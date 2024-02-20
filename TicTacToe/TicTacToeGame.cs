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
                            return TicTacResults.Player2;
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
                            return TicTacResults.Player2;
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
                        return TicTacResults.Player2;
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

            Point move = findBestMove(Field);

            if (Player1 == Players.Computer) { SetField(move, 'X'); }
            else if (Player2 == Players.Computer) { SetField(move, 'O'); }
        }



        //--------------------------------------------------------------------------------

        // This function returns true if there are moves 
        // remaining on the board. It returns false if 
        // there are no moves left to play. 
        private Boolean isMovesLeft(char[,] board)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (board[i, j] == ' ')
                        return true;
            return false;
        }

        // This is the evaluation function as discussed 
        // in the previous article ( http://goo.gl/sJgv68 ) 
        private int evaluate(char[,] board)
        {
            // Checking for Rows for X or O victory. 
            for (int row = 0; row < 3; row++)
            {
                if (board[row, 0] == board[row, 1] &&
                    board[row, 1] == board[row, 2])
                {
                    if (board[row, 0] == 'O')
                        return +10;
                    else if (board[row, 0] == 'X')
                        return -10;
                }
            }

            // Checking for Columns for X or O victory. 
            for (int col = 0; col < 3; col++)
            {
                if (board[0, col] == board[1, col] &&
                    board[1, col] == board[2, col])
                {
                    if (board[0, col] == 'O')
                        return +10;

                    else if (board[0, col] == 'X')
                        return -10;
                }
            }

            // Checking for Diagonals for X or O victory. 
            if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
            {
                if (board[0, 0] == 'O')
                    return +10;
                else if (board[0, 0] == 'X')
                    return -10;
            }

            if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
            {
                if (board[0, 2] == 'O')
                    return +10;
                else if (board[0, 2] == 'X')
                    return -10;
            }

            // Else if none of them have won then return 0 
            return 0;
        }

        // This is the minimax function. It considers all 
        // the possible ways the game can go and returns 
        // the value of the board 
        private int minimax(char[,] board,
                           int depth, Boolean isMax)
        {
            int score = evaluate(board);

            // If Maximizer has won the game  
            // return his/her evaluated score 
            if (score == 10)
                return score;

            // If Minimizer has won the game  
            // return his/her evaluated score 
            if (score == -10)
                return score;

            // If there are no more moves and  
            // no winner then it is a tie 
            if (isMovesLeft(board) == false)
                return 0;

            // If this maximizer's move 
            if (isMax)
            {
                int best = -1000;

                // Traverse all cells 
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        // Check if cell is empty 
                        if (board[i, j] == ' ')
                        {
                            // Make the move 
                            board[i, j] = 'O';

                            // Call minimax recursively and choose 
                            // the maximum value 
                            best = Math.Max(best, minimax(board,
                                            depth + 1, !isMax));

                            // Undo the move 
                            board[i, j] = ' ';
                        }
                    }
                }
                return best;
            }

            // If this minimizer's move 
            else
            {
                int best = 1000;

                // Traverse all cells 
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        // Check if cell is empty 
                        if (board[i, j] == ' ')
                        {
                            // Make the move 
                            board[i, j] = 'X';

                            // Call minimax recursively and choose 
                            // the minimum value 
                            best = Math.Min(best, minimax(board,
                                            depth + 1, !isMax));

                            // Undo the move 
                            board[i, j] = ' ';
                        }
                    }
                }
                return best;
            }
        }

        // This will return the best possible 
        // move for the player 
        private Point findBestMove(char[,] board)
        {
            int bestVal = -1000;
            Point bestMove = new Point();
            bestMove.X = -1;
            bestMove.Y = -1;

            // Traverse all cells, evaluate minimax function  
            // for all empty cells. And return the cell  
            // with optimal value. 
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Check if cell is empty 
                    if (board[i, j] == ' ')
                    {
                        // Make the move 
                        board[i, j] = 'O';

                        // compute evaluation function for this 
                        // move. 
                        int moveVal = minimax(board, 0, false);

                        // Undo the move 
                        board[i, j] = ' ';

                        // If the value of the current move is 
                        // more than the best value, then update 
                        // best/ 
                        if (moveVal > bestVal)
                        {
                            bestMove.X = i;
                            bestMove.Y = j;
                            bestVal = moveVal;
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
