using System;

namespace ChessDll
{
    /// <summary>
    /// Class-container for the properties of the movement of the figures.
    /// </summary>
    internal class FigureMoving
    {
        /// <summary>
        /// The chess figure.
        /// </summary>
        public Figure Figure { get; private set; }

        /// <summary>
        /// The figure moves FROM the square.
        /// </summary>
        public Square From { get; private set; }

        /// <summary>
        /// The figure moves TO the square.
        /// </summary>
        public Square To { get; private set; }

        /// <summary>
        /// The chess figure promotion.
        /// </summary>
        public Figure Promotion { get; private set; }

        /// <summary>
        /// The class FigureMoving constructor.
        /// </summary>
        /// <param name="fs">The container of the figure on the square.</param>
        /// <param name="to">The new square.</param>
        /// <param name="promotion">The chess figure promotion.</param>
        public FigureMoving(FigureOnSquare fs, Square to, Figure promotion = Figure.none)
        {
            Figure = fs.Figure;
            From = fs.Square;
            To = to;
            Promotion = promotion;
        }

        /// <summary>
        /// The class FigureMoving constructor.
        /// </summary>
        /// <param name="move">The move code as string.</param>
        /// 
        /// string move:
        /// Pe2e4 - P (chess figure), e2 (start position), e4 (end position)
        /// Pe7e8Q - P (start chess figure), e2 (start position), e4 (end position), Q (chess figure promotion.)
        /// 
        /// Pe2e4     Pe7e8Q  as  move[i]
        /// 01234     012345  as  i
        public FigureMoving(string move)
        {
            Figure = (Figure)move[0];
            From = new Square(move.Substring(1, 2));
            To = new Square(move.Substring(3, 2));
            Promotion = (move.Length == 6) ? (Figure)move[5] : Figure.none;
        }

        /// <summary>
        /// The difference in X coordinates of the figure.
        /// </summary>
        public int DeltaX => To.X - From.X;

        /// <summary>
        /// The difference in Y coordinates of the figure.
        /// </summary>
        public int DeltaY => To.Y - From.Y;


        /// <summary>
        /// The absolute difference of the figure's X coordinates.
        /// </summary>
        public int AbsDeltaX => Math.Abs(DeltaX);

        /// <summary>
        /// The absolute difference of the figure's Y coordinates.
        /// </summary>
        public int AbsDeltaY => Math.Abs(DeltaY);


        /// <summary>
        /// The difference sign in X coordinates of the figure.
        /// </summary>
        public int SignX => Math.Sign(DeltaX);

        /// <summary>
        /// The difference sign in Y coordinates of the figure.
        /// </summary>
        public int SignY => Math.Sign(DeltaY);


        /// <summary>
        /// Overriding method.
        /// </summary>
        /// <returns>The new text.</returns>
        public override string ToString()
        {
            string text = (char)Figure + From.Name + To.Name;

            if (Promotion != Figure.none)
            {
                text += (char)Promotion;
            }

            return text;
        }
    }
}
