namespace ChessDll
{
    /// <summary>
    /// The color of chess figures.
    /// </summary>
    internal enum Color
    {
        none,

        white,

        black
    }

    static class ColorExtensions
    {
        /// <summary>
        /// Inverting color figures.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color FlipColor(this Color color)
        {
            if (color == Color.white)
            {
                return Color.black;
            }
            else if (color == Color.black)
            {
                return Color.white;
            }

            return Color.none;
        }
    }
}
