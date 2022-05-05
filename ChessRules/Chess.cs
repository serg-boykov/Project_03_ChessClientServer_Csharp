using System.Collections.Generic;

namespace ChessDll
{
    /// <summary>
    /// The main chess class.
    /// </summary>
    public class Chess
    {
        /// <summary>
        /// FEN (Forsyth–Edwards Notation) — standard notation to record chess diagrams.
        /// </summary>
        public string FEN { get; private set; }

        /// <summary>
        /// A chess board.
        /// </summary>
        readonly Board board;

        /// <summary>
        /// Chess moves of the figures.
        /// </summary>
        readonly Moves moves;

        /// <summary>
        /// All possible movements of the figures.
        /// </summary>
        List<FigureMoving> allMoves;

        /// <summary>
        /// The Chess class constructor.
        /// </summary>
        /// <param name="fen">Standard notation to record chess diagrams.</param>
        public Chess(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            this.FEN = fen;
            board = new Board(fen);
            moves = new Moves(board);
        }

        /// <summary>
        /// The Chess class constructor.
        /// </summary>
        /// <param name="board">The chess board.</param>
        private Chess(Board board)
        {
            this.board = board;
            this.FEN = board.Fen;
            moves = new Moves(board);
        }

        /// <summary>
        /// Moving a chess piece.
        /// </summary>
        /// <param name="move">String move code.</param>
        /// <returns>New chess position.</returns>
        ///
        /// string move:
        /// Pe2e4 - P (chess figure), e2 (start position), e4 (end position)
        /// Pe7e8Q - P (start chess figure), e2 (start position), e4 (end position), Q (chess figure promotion)
        public Chess Move(string move)
        {
            FigureMoving figureMoving = new FigureMoving(move);

            // If the figure can't move.
            if (!moves.CanMove(figureMoving))
            {
                return this;
            }

            // If there is a check after the move (если Шах)
            if (board.IsCheckAfterMove(figureMoving))
            {
                return this;
            }

            Board nextBoard = board.Move(figureMoving);
            var nextChess = new Chess(nextBoard);
            return nextChess;
        }

        /// <summary>
        /// Getting the char figure at the board square.
        /// </summary>
        /// <param name="x">The coordinate x of the board.</param>
        /// <param name="y">The coordinate y of the board.</param>
        /// <returns>The char figure at the board square.</returns>
        public char GetFigureAt(int x, int y)
        {
            Square square = new Square(x, y);
            Figure figure = board.GetFigureAt(square);

            return figure == Figure.none ? '.' : (char)figure;
        }

        /// <summary>
        /// Finding for all possible movements of the figures of the required color.
        /// </summary>
        private void FindAllMoves()
        {
            allMoves = new List<FigureMoving>();

            // Cycle through all figures of the required color.
            foreach (FigureOnSquare figureOnSquare in board.YieldFigures())
            {
                // Cycle through all board squares.
                foreach (Square squareTo in Square.YieldSquares())
                {
                    FigureMoving figureMoving = new FigureMoving(figureOnSquare, squareTo);

                    // If the figure can move.
                    if (moves.CanMove(figureMoving))
                    {
                        // If no chess CHECK.
                        if (!board.IsCheckAfterMove(figureMoving))
                        {
                            allMoves.Add(figureMoving);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// To get the list of all possible movements of the figures of the required color.
        /// </summary>
        /// <returns>The list of all possible movements of the figures.</returns>
        public List<string> GetAllMoves()
        {
            // Finding for all possible movements of the figures of the required color.
            FindAllMoves();

            List<string> list = new List<string>();

            foreach (FigureMoving figureMoving in allMoves)
            {
                list.Add(figureMoving.ToString());
            }

            return list;
        }

        /// <summary>
        /// Is there a chess CHECK?
        /// </summary>
        /// <returns>There is a chess CHECK or not.</returns>
        public bool IsCheck()
        {
            return board.IsCheck();
        }
    }
}
