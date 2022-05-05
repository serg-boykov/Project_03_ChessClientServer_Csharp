using System.Collections.Specialized;

namespace ChessClientDll
{
    /// <summary>
    /// Information about the chess game.
    /// </summary>
    public struct GameInfo
    {
        /// <summary>
        /// The chess game identifier.
        /// </summary>
        public int GameID;

        /// <summary>
        /// Forsyth–Edwards Notation, FEN —
        /// Standard notation of recording chess diagrams.
        /// </summary>
        public string FEN;

        /// <summary>
        /// Game status: wait, play, done.
        /// </summary>
        public string Status;

        /// <summary>
        /// The name of the player who plays White chess figures.
        /// </summary>
        public string White;

        /// <summary>
        /// The name of the player who plays Black chess figures.
        /// </summary>
        public string Black;

        /// <summary>
        /// The last chess move.
        /// </summary>
        public string LastMove;

        /// <summary>
        /// The color of the chess figures that the player plays.
        /// </summary>
        public string YourColor;

        /// <summary>
        /// Whose is the current chess move.
        /// If your move is True, otherwise False.
        /// </summary>
        public bool IsYourMove;

        /// <summary>
        /// the name of the player who offered the opponent a draw.
        /// </summary>
        public string OfferDraw;

        /// <summary>
        /// The name of the winner.
        /// </summary>
        public string Winner;


        /// <summary>
        /// Information about the game from the list.
        /// </summary>
        /// <param name="list">List of game parameters.</param>
        public GameInfo(NameValueCollection list)
        {
            GameID = int.Parse(list["ID"]);
            FEN = list["FEN"];
            Status = list["Status"];
            White = list["White"];
            Black = list["Black"];
            LastMove = list["LastMove"];
            YourColor = list["YourColor"];
            IsYourMove = bool.Parse(list["IsYourMove"]);
            OfferDraw = list["OfferDraw"];
            Winner = list["Winner"];
        }

        override
            public string ToString() =>
            "GameID = " + GameID +
            "\nFEN = " + FEN +
            "\nStatus = " + Status +
            "\nWhite = " + White +
            "\nBlack = " + Black +
            "\nLastMove = " + LastMove +
            "\nYourColor = " + YourColor +
            "\nIsYourMove = " + IsYourMove +
            "\nOfferDraw = " + OfferDraw +
            "\nWinner = " + Winner;
    }
}
