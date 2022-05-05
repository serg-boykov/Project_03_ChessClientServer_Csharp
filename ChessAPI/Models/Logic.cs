using ChessDll;
using System.Linq;

namespace ChessAPI.Models
{
    /// <summary>
    /// The game logic class.
    /// </summary>
    public class Logic
    {
        private readonly ModelChessDB db;

        /// <summary>
        /// The class Logic constructor.
        /// </summary>
        public Logic()
        {
            db = new ModelChessDB();
        }

        /// <summary>
        /// Getting the current game or the new game.
        /// </summary>
        /// <returns>The game.</returns>
        public Game GetCurrentGame()
        {
            Game game = db
                .Games
                .Where(g => g.Status == "play")
                .OrderBy(g => g.Id)
                .FirstOrDefault();

            if (game == null)
            {
                game = CreateNewGame();
            }

            return game;
        }

        /// <summary>
        /// Creating the new game.
        /// </summary>
        /// <returns>The new game.</returns>
        private Game CreateNewGame()
        {
            Game game = new Game();

            Chess chess = new Chess();

            game.Fen = chess.FEN;
            game.Status = "play";
            game.LastMove = "";
            game.YourColor = "white";

            db.Games.Add(game);
            db.SaveChanges();

            return game;
        }

        /// <summary>
        /// Getting the game by the game ID.
        /// </summary>
        /// <param name="id">The game ID.</param>
        /// <returns>The game.</returns>
        internal Game GetGame(int id)
        {
            return db.Games.Find(id);
        }

        /// <summary>
        /// To make the chess move.
        /// </summary>
        /// <param name="id">The game ID.</param>
        /// <param name="move">The chess move.</param>
        /// <returns></returns>
        public Game MakeMove(int id, string move)
        {
            Game game = GetGame(id);

            if (game == null || game.Status != "play")
            {
                return game;
            }

            Chess chess = new Chess(game.Fen);
            Chess chessNext = chess.Move(move);

            if (chessNext.FEN == game.Fen)
            {
                return game;
            }

            game.Fen = chessNext.FEN;
            game.LastMove = move;

            string moveColor = game.Fen.Substring(game.Fen.IndexOf(' ') + 1, 1);
            game.YourColor = moveColor == "w" ? "white" : "black";

            // It can test there is either the CHECKMATE or the STALEMATE ...
            // if (chessNext.IsCheckmate || chess.IsStalemate)
            if (chessNext.IsCheck()) // Is there the chess CHECK?
            {
                game.Status = "done";
            }

            // Setting the base status as a modified to save the database.
            db.Entry(game).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return game;
        }
    }
}