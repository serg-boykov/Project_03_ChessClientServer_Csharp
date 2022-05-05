using System.Collections.Generic;

namespace ChessDll
{
    /// <summary>
    /// Struct of the chessboard square.
    /// </summary>
    internal struct Square
    {
        /// <summary>
        /// Non -existing square.
        /// </summary>
        public static Square none = new Square(-1, 1);

        /// <summary>
        /// The square constructor.
        /// </summary>
        /// <param name="x">The coordinate x.</param>
        /// <param name="y">The coordinate y.</param>
        public Square(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// The coordinate property X.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// The coordinate property Y.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// The name of the board square.
        /// </summary>
        public string Name { get { return ((char)('a' + X)).ToString() + (Y + 1).ToString(); } }


        /// <summary>
        /// The square constructor.
        /// </summary>
        /// <param name="e2">The string code of the board square.</param>
        public Square(string e2)
        {
            if (e2.Length == 2 &&
                e2[0] >= 'a' && e2[0] <= 'h' &&
                e2[1] >= '1' && e2[1] <= '8')
            {
                X = e2[0] - 'a';
                Y = e2[1] - '1';
            }
            else this = none;
        }

        /// <summary>
        /// Is there a square on the board?
        /// </summary>
        /// <returns>Yes | No.</returns>
        public bool OnBoard()
        {
            return X >= 0 && X < 8 &&
                   Y >= 0 && Y < 8;
        }

        /// <summary>
        /// Overriding the operator '=='.
        /// </summary>
        /// <param name="square1">The board square 1.</param>
        /// <param name="square2">The board square 2.</param>
        /// <returns>Yes | No.</returns>
        public static bool operator ==(Square square1, Square square2)
        {
            return square1.X == square2.X && square1.Y == square2.Y;
        }

        /// <summary>
        /// Overriding the operator '!='.
        /// </summary>
        /// <param name="square1">The board square 1.</param>
        /// <param name="square2">The board square 2.</param>
        /// <returns>Yes | No.</returns>
        public static bool operator !=(Square square1, Square square2)
        {
            return !(square1 == square2);
        }

        /// <summary>
        /// Overriding the method Equals().
        /// </summary>
        /// <param name="obj">Object for comparing equality.</param>
        /// <returns>Yes | No.</returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Overriding the method GetHashCode().
        /// </summary>
        /// <returns>Yes | No.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Creating the list of all chessboard square.
        /// </summary>
        /// <returns>The list of all chessboard square.</returns>
        public static IEnumerable<Square> YieldSquares()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    yield return new Square(x, y);
                }
            }
        }
    }
}
