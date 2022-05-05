using System.Collections.Generic;
using System.Text;

namespace ChessDll
{
    /// <summary>
    /// The Chess Board class.
    /// </summary>
    internal class Board
    {
        /// <summary>
        /// FEN (Forsyth–Edwards Notation) — standard notation to record chess diagrams.
        /// </summary>
        public string Fen { get; private set; }

        /// <summary>
        /// An array of chess pieces on a chessboard for two coordinates.
        /// </summary>
        readonly Figure[,] figures;

        /// <summary>
        /// Color of the current move.
        /// </summary>
        public Color MoveColor { get; private set; }

        /// <summary>
        /// The number of moves (White then black as one).
        /// </summary>
        public int MoveNumber { get; private set; }

        /// <summary>
        /// The Board constructor.
        /// </summary>
        /// <param name="fen">Standard notation to record chess diagrams.</param>
        public Board(string fen)
        {
            Fen = fen;
            figures = new Figure[8, 8];
            Init();
        }

        /// <summary>
        /// Splitting the FEN string into parts and initializing of variables.
        /// </summary>
        private void Init()
        {
            // rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1
            // 0------------------------------------------ 1 2--- 3 4 5
            // 0-5 these are parts of the string 'fen' (Screenshot of a chess game),
            // 2-4 parts will not implement
            string[] parts = Fen.Split();
            if (parts.Length != 6)
            {
                return;
            }

            InitFigures(parts[0]);
            MoveColor = (parts[1] == "b") ? Color.black : Color.white;
            MoveNumber = int.Parse(parts[5]);
        }

        /// <summary>
        /// Initialization of chess pieces from the string to the array figures[x, y].
        /// </summary>
        /// <param name="data">Part of the FEN string.</param>
        private void InitFigures(string data)
        {
            // string data before:
            // rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR
            for (int i = 8; i >= 2; i--)
            {
                data = data.Replace(i.ToString(), (i - 1).ToString() + "1");
            }
            // Result:
            // rnbqkbnr/pppppppp/11111111/11111111/11111111/11111111/PPPPPPPP/RNBQKBNR

            data = data.Replace("1", ".");
            // Result:
            // rnbqkbnr/pppppppp/......../......../......../......../PPPPPPPP/RNBQKBNR

            string[] lines = data.Split('/');

            // Initialization of chess pieces from the string to the array figures[x, y].
            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                {
                    figures[x, y] = lines[7 - y][x] == '.' ? Figure.none : (Figure)lines[7 - y][x];
                }
            }
        }

        /// <summary>
        /// Creating the collection of chess figures of the required color.
        /// </summary>
        /// <returns>The collection of chess figures of the required color.</returns>
        public IEnumerable<FigureOnSquare> YieldFigures()
        {
            foreach (Square square in Square.YieldSquares())
            {
                if (GetFigureAt(square).GetColor() == MoveColor)
                {
                    yield return new FigureOnSquare(GetFigureAt(square), square);
                }
            }
        }

        /// <summary>
        /// Getting the chess figure at the board square.
        /// </summary>
        /// <param name="square">The board square.</param>
        /// <returns>The chess figure at the board square.</returns>
        public Figure GetFigureAt(Square square)
        {
            if (square.OnBoard())
            {
                return figures[square.X, square.Y];
            }

            return Figure.none;
        }

        /// <summary>
        /// Setting the chess figure at the board square.
        /// </summary>
        /// <param name="square">The board square.</param>
        /// <param name="figure">The chess figure.</param>
        private void SetFigureAt(Square square, Figure figure)
        {
            if (square.OnBoard())
            {
                figures[square.X, square.Y] = figure;
            }
        }

        /// <summary>
        /// To move the chess figure.
        /// </summary>
        /// <param name="figureMoving">The container of the figure moving.</param>
        /// <returns>The next board after figure moving</returns>
        public Board Move(FigureMoving figureMoving)
        {
            Board next = new Board(Fen);

            // To set the empty figure at the OLD board square.
            next.SetFigureAt(figureMoving.From, Figure.none);

            // To set the figure at the NEW board square.
            Figure figureSet = figureMoving.Promotion == Figure.none ? figureMoving.Figure : figureMoving.Promotion;
            next.SetFigureAt(figureMoving.To, figureSet);

            // Increase the move number.
            if (MoveColor == Color.black)
            {
                next.MoveNumber++;
            }

            // Changing color to the opposite.
            next.MoveColor = MoveColor.FlipColor();

            // Generate the new Fen.
            next.GenerateFen();

            return next;
        }

        /// <summary>
        /// Generating the FEN (Forsyth–Edwards Notation).
        /// </summary>
        private void GenerateFen()
        {
            Fen = FenFigures() + " " +
                (MoveColor == Color.white ? "w" : "b") +
                " - - 0 " + MoveNumber.ToString();
        }

        /// <summary>
        /// Figures from the array figures[x, y] to the FEN (Forsyth–Edwards Notation)
        /// </summary>
        /// <returns>The FEN (Forsyth–Edwards Notation).</returns>
        private string FenFigures()
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                {
                    stringBuilder.Append(figures[x, y] == Figure.none ? '1' : (char)figures[x, y]);
                }

                if (y > 0)
                {
                    stringBuilder.Append('/');
                }
            }
            // Result:
            // rnbqkbnr/pppppppp/11111111/11111111/11111111/11111111/PPPPPPPP/RNBQKBNR

            string eight = "11111111";
            for (int i = 8; i >= 2; i--)
            {
                stringBuilder.Replace(eight.Substring(0, i), i.ToString());
            }
            // Result:
            // rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Is there a chess CHECK?
        /// </summary>
        /// <returns>There is a chess CHECK or not.</returns>
        public bool IsCheck()
        {
            Board beforeMove = new Board(Fen)
            {
                MoveColor = MoveColor.FlipColor()
            };

            return beforeMove.CanEatKing();
        }

        /// <summary>
        /// Can the King be eaten?
        /// </summary>
        /// <returns>Yes | No.</returns>
        private bool CanEatKing()
        {
            Square badKing = FindBadKing();
            Moves moves = new Moves(this);

            foreach (FigureOnSquare figureOnSquare in YieldFigures())
            {
                FigureMoving figureMoving = new FigureMoving(figureOnSquare, badKing);

                if (moves.CanMove(figureMoving))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Searching for the enemy King.
        /// </summary>
        /// <returns>The King square.</returns>
        private Square FindBadKing()
        {
            Figure badKing = MoveColor == Color.black ? Figure.whiteKing : Figure.blackKing;

            foreach (Square square in Square.YieldSquares())
            {
                if (GetFigureAt(square) == badKing)
                {
                    return square;
                }
            }

            return Square.none;
        }

        /// <summary>
        /// Can the King be eaten after the move?
        /// </summary>
        /// <param name="figureMoving">The container of the figure moving.</param>
        /// <returns>Yes | No.</returns>
        public bool IsCheckAfterMove(FigureMoving figureMoving)
        {
            Board afterMove = Move(figureMoving);

            return afterMove.CanEatKing();
        }
    }
}
