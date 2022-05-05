namespace ChessDll
{
    /// <summary>
    /// Class for chess rules for moving figures.
    /// </summary>
    internal class Moves
    {
        /// <summary>
        /// The container of the figure moving.
        /// </summary>
        private FigureMoving figureMoving;

        /// <summary>
        /// A chess board.
        /// </summary>
        private Board board;


        /// <summary>
        /// Class Moves constructor.
        /// </summary>
        /// <param name="board">The chess board.</param>
        public Moves(Board board)
        {
            this.board = board;
        }

        /// <summary>
        /// Is it possible to make a move?
        /// </summary>
        /// <param name="figureMoving">The container of the figure moving.</param>
        /// <returns>Yes | No.</returns>
        public bool CanMove(FigureMoving figureMoving)
        {
            this.figureMoving = figureMoving;
            return CanMoveFrom() && CanMoveTo() && CanFigureMove();
        }

        /// <summary>
        /// Can the figure move?
        /// </summary>
        /// <returns>Yes | No.</returns>
        private bool CanFigureMove()
        {
            switch (figureMoving.Figure)
            {
                case Figure.whiteKing:
                case Figure.blackKing:
                    return CanKingMove();

                case Figure.whiteQueen:
                case Figure.blackQueen:
                    return CanStraightMove();

                case Figure.whiteRook:
                case Figure.blackRook:
                    return CanRookMove();

                case Figure.whiteBishop:
                case Figure.blackBishop:
                    return CanBishopMove();

                case Figure.whiteKnight:
                case Figure.blackKnight:
                    return CanKnightMove();

                case Figure.whitePawn:
                case Figure.blackPawn:
                    return CanPawnMove();
            }

            return false;
        }

        /// <summary>
        /// Can the Pawn move?
        /// </summary>
        /// <returns>Yes | No.</returns>
        private bool CanPawnMove()
        {
            if (figureMoving.From.Y < 1 || figureMoving.From.Y > 6)
            {
                return false;
            }

            int stepY = figureMoving.Figure.GetColor() == Color.white ? 1 : -1;

            bool isCanPawnMove = CanPawnGoY(stepY) || CanPawnJump(stepY) || CanPawnEat(stepY);

            return isCanPawnMove;
        }

        /// <summary>
        /// Can the Pawn go by the coordinate Y?
        /// </summary>
        /// <param name="stepY">The step by the coordinate Y.</param>
        /// <returns>Yes | No.</returns>
        private bool CanPawnGoY(int stepY)
        {
            if (board.GetFigureAt(figureMoving.To) == Figure.none)
            {
                if (figureMoving.DeltaX == 0 && figureMoving.DeltaY == stepY)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Can the Pawn jump by the coordinate Y?
        /// </summary>
        /// <param name="stepY">The step by the coordinate Y.</param>
        /// <returns>Yes | No.</returns>
        private bool CanPawnJump(int stepY)
        {
            if (board.GetFigureAt(figureMoving.To) == Figure.none)
            {
                if (figureMoving.DeltaX == 0 && figureMoving.DeltaY == 2 * stepY)
                {
                    if (figureMoving.From.Y == 1 || figureMoving.From.Y == 6)
                    {
                        Square square = new Square(figureMoving.From.X, figureMoving.From.Y + stepY);
                        bool isEmptySquare = board.GetFigureAt(square) == Figure.none;

                        if (isEmptySquare)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Can the Pawn eat by the coordinate Y?
        /// </summary>
        /// <param name="stepY">The step by the coordinate Y.</param>
        /// <returns>Yes | No.</returns>
        private bool CanPawnEat(int stepY)
        {
            if (board.GetFigureAt(figureMoving.To) != Figure.none)
            {
                if (figureMoving.AbsDeltaX == 1 && figureMoving.DeltaY == stepY)
                {
                    return true;
                }
            }

            return false;

        }

        /// <summary>
        /// Can the Bishop move?
        /// </summary>
        /// <returns>Yes | No.</returns>
        private bool CanBishopMove()
        {
            bool isCanBishopMove = figureMoving.SignX != 0 && figureMoving.SignY != 0 && CanStraightMove();

            return isCanBishopMove;
        }

        /// <summary>
        /// Can the Rook move?
        /// </summary>
        /// <returns>Yes | No.</returns>
        private bool CanRookMove()
        {
            bool isCanRookMove = (figureMoving.SignX == 0 || figureMoving.SignY == 0) && CanStraightMove();

            return isCanRookMove;
        }

        /// <summary>
        /// Can the figure move in a straight line?
        /// </summary>
        /// <returns>Yes | No.</returns>
        private bool CanStraightMove()
        {
            Square at = figureMoving.From;

            do
            {
                at = new Square(at.X + figureMoving.SignX, at.Y + figureMoving.SignY);

                if (at == figureMoving.To)
                {
                    return true;
                }

            } while (at.OnBoard() && board.GetFigureAt(at) == Figure.none);

            return false;
        }

        /// <summary>
        /// Can the Knight move?
        /// </summary>
        /// <returns>Yes | No.</returns>
        private bool CanKnightMove()
        {
            bool isCanKnightMove = (figureMoving.AbsDeltaX == 1 && figureMoving.AbsDeltaY == 2) ||
                (figureMoving.AbsDeltaX == 2 && figureMoving.AbsDeltaY == 1);

            return isCanKnightMove;
        }

        /// <summary>
        /// Can the King move?
        /// </summary>
        /// <returns>Yes | No.</returns>
        private bool CanKingMove()
        {
            bool isCanKingMove = figureMoving.AbsDeltaX <= 1 && figureMoving.AbsDeltaY <= 1;

            return isCanKingMove;
        }

        /// <summary>
        /// Can the figure move TO the new chess square?
        /// </summary>
        /// <returns>Yes | No.</returns>
        private bool CanMoveTo()
        {
            bool isCanMoveTo = figureMoving.To.OnBoard() &&
                figureMoving.To != figureMoving.From &&
                board.GetFigureAt(figureMoving.To).GetColor() != board.MoveColor;

            return isCanMoveTo;
        }

        /// <summary>
        /// Can the figure move FROM the old chess square?
        /// </summary>
        /// <returns>Yes | No.</returns>
        private bool CanMoveFrom()
        {
            bool isCanMoveFrom = figureMoving.From.OnBoard() && figureMoving.Figure.GetColor() == board.MoveColor;

            return isCanMoveFrom;
        }
    }
}
