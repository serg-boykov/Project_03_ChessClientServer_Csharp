using ChessAPI.Models;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;

namespace ChessAPI.Controllers
{
    /// <summary>
    /// The API controller class.
    /// </summary>
    public class GamesController : ApiController
    {
        private readonly ModelChessDB db = new ModelChessDB();

        /// <summary>
        /// Method GET: api/Games.
        /// </summary>
        /// <returns>The game.</returns>
        public JsonResult<Game> GetGames()
        {
            Logic logic = new Logic();
            Game game = logic.GetCurrentGame();
            return Json(game);
        }

        /// <summary>
        /// Method GET: api/Games/5.
        /// </summary>
        /// <param name="id">The game ID.</param>
        /// <returns>The game.</returns>
        public JsonResult<Game> GetGame(int id)
        {
            Logic logic = new Logic();
            Game game = logic.GetGame(id);
            return Json(game);
        }

        /// <summary>
        /// Method GET: api/Games/5/Pe2e4.
        /// </summary>
        /// <param name="id">The game ID.</param>
        /// <param name="move">The chess move.</param>
        /// <returns>The game.</returns>
        public JsonResult<Game> GetMove(int id, string move)
        {
            Logic logic = new Logic();
            Game game = logic.MakeMove(id, move);
            return Json(game);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /*
        /// <summary>
        /// Is there the game in the database?
        /// </summary>
        /// <param name="id">The game ID.</param>
        /// <returns>Yes | No.</returns>
        private bool GameExists(int id)
        {
            return db.Games.Count(e => e.ID == id) > 0;
        }
        */
    }
}