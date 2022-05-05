namespace ChessDll
{
    /// <summary>
    /// The chess figure char.
    /// </summary>
    internal enum Figure
    {
        none,

        whiteKing = 'K',

        whiteQueen = 'Q',

        whiteRook = 'R',

        whiteBishop = 'B',

        whiteKnight = 'N',

        whitePawn = 'P',


        blackKing = 'k',

        blackQueen = 'q',

        blackRook = 'r',

        blackBishop = 'b',

        blackKnight = 'n',

        blackPawn = 'p'
    }

    static class FigureExtensions
    {
        /// <summary>
        /// To get the chess figure color.
        /// </summary>
        /// <param name="figure">The chess figure.</param>
        /// <returns>The figure color.</returns>
        public static Color GetColor(this Figure figure)
        {
            if (figure == Figure.none)
            {
                return Color.none;
            }

            return
                (
                figure == Figure.whiteKing   ||
                figure == Figure.whiteQueen  ||
                figure == Figure.whiteRook   ||
                figure == Figure.whiteBishop ||
                figure == Figure.whiteKnight ||
                figure == Figure.whitePawn
                )
                ? Color.white
                : Color.black;
        }
    }
}
