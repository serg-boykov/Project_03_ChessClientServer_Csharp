namespace ChessDll
{
    /// <summary>
    /// The class-container of the figure on the board square.
    /// </summary>
    internal class FigureOnSquare
    {
        /// <summary>
        /// The chess figure.
        /// </summary>
        public Figure Figure { get; private set; }

        /// <summary>
        /// The board square.
        /// </summary>
        public Square Square { get; private set; }


        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="figure">The chess figure.</param>
        /// <param name="square">The board square.</param>
        public FigureOnSquare(Figure figure, Square square)
        {
            Figure = figure;
            Square = square;
        }
    }
}
