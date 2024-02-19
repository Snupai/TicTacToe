using System.Drawing;

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

        private void SetFieldComputer()
        {
            if (!isSinglePlayer) return;
            Point move;
            int row;
            int column;

            Random random = new Random();
            do
            {
                row = random.Next(1, 3+1);
                column = random.Next(1, 3+1);
                move = new Point(row, column);
            } while (MovesTaken.Contains(move));

            if (Player1 == Players.Computer) { SetField(move, 'X'); }
            else if (Player2 == Players.Computer) { SetField(move, 'O'); }
        }
    }
}
