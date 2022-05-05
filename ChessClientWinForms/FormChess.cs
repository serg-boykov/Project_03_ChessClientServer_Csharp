using ChessClientDll;
using ChessDll;
using System.Drawing;
using System.Windows.Forms;

namespace ChessClientWinForms
{
    public partial class FormChess : Form
    {
        /// <summary>
        /// The route to the Web API controller.
        /// </summary>
        private const string HOST = "https://localhost:44333/api/Games";

        /// <summary>
        /// The game user.
        /// </summary>
        public const string USER = "";

        /// <summary>
        /// The size of the square of the chessboard.
        /// </summary>
        const int SIZE = 50;

        /// <summary>
        /// The chess board.
        /// </summary>
        Panel[,] chessBoard;

        /// <summary>
        /// The chess rules dll.
        /// </summary>
        Chess chess;

        /// <summary>
        /// If no move then True
        /// </summary>
        bool wait;

        /// <summary>
        /// Start coordinates of a chess figure.
        /// </summary>
        int xFrom, yFrom;

        /// <summary>
        /// The chess game client.
        /// </summary>
        readonly ChessClient chessClient;

        /// <summary>
        /// The class constructor.
        /// </summary>
        public FormChess()
        {
            InitializeComponent();
            chessClient = new ChessClient(HOST, USER);
            InitPanels();
            wait = true;
            RefreshPosition();
        }

        /// <summary>
        /// Update the chessboard in the multiplayer version of the game 
        /// when the partner has made a move. 
        /// You need to make a mouse click on the edge of the chessboard.
        /// </summary>
        void RefreshPosition()
        {
            chess = new Chess(chessClient.GetCurrentGame().FEN);
            ShowPosition();
        }

        /// <summary>
        /// Creating the chessboard.
        /// </summary>
        private void InitPanels()
        {
            chessBoard = new Panel[8, 8];

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    chessBoard[x, y] = AddPanel(x, y);
                }
            }
        }

        /// <summary>
        /// Show chess figures on a chessboard.
        /// </summary>
        void ShowPosition()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    ShowFigure(x, y, chess.GetFigureAt(x, y));
                }
            }

            MarkSquare();
        }

        /// <summary>
        /// To set the required chess figure on the required chessboard square.
        /// </summary>
        /// <param name="x">The coordinate X.</param>
        /// <param name="y">The coordinate Y.</param>
        /// <param name="figure">The chess figure char.</param>
        private void ShowFigure(int x, int y, char figure)
        {
            chessBoard[x, y].BackgroundImage = GetFigureImage(figure);
        }

        /// <summary>
        /// Get an image of a chess figure from resources.
        /// </summary>
        /// <param name="figure">The chess figure char.</param>
        /// <returns>The image of a chess figure</returns>
        private Image GetFigureImage(char figure)
        {
            switch (figure)
            {
                case 'R': return Properties.Resources.WhiteRook;
                case 'N': return Properties.Resources.WhiteKnight;
                case 'B': return Properties.Resources.WhiteBishop;
                case 'Q': return Properties.Resources.WhiteQueen;
                case 'K': return Properties.Resources.WhiteKing;
                case 'P': return Properties.Resources.WhitePawn;

                case 'r': return Properties.Resources.BlackRook;
                case 'n': return Properties.Resources.BlackKnight;
                case 'b': return Properties.Resources.BlackBishop;
                case 'q': return Properties.Resources.BlackQueen;
                case 'k': return Properties.Resources.BlackKing;
                case 'p': return Properties.Resources.BlackPawn;

                default: return null;
            }
        }

        /// <summary>
        /// Adding a chessboard square by coordinates.
        /// </summary>
        /// <param name="x">The coordinate X.</param>
        /// <param name="y">The coordinate Y.</param>
        /// <returns>The chessboard square.</returns>
        Panel AddPanel(int x, int y)
        {
            Panel panel = new Panel
            {
                BackColor = GetColor(x, y),
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                Location = GetLocation(x, y),
                Name = "p" + x + y,
                Size = new System.Drawing.Size(SIZE, SIZE)
            };
            panel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Panel_MouseClick);

            board.Controls.Add(panel);

            return panel;
        }

        /// <summary>
        /// Getting the coordinates of the chess square.
        /// </summary>
        /// <param name="x">The coordinate X.</param>
        /// <param name="y">The coordinate Y.</param>
        /// <returns>The coordinates of the chess square.</returns>
        private Point GetLocation(int x, int y)
        {
            return new Point(SIZE / 2 + x * SIZE,
                             SIZE / 2 + (7 - y) * SIZE);
        }

        /// <summary>
        /// Getting the color of the chess square.
        /// </summary>
        /// <param name="x">The coordinate X.</param>
        /// <param name="y">The coordinate Y.</param>
        /// <returns>The color of the chess square.</returns>
        private Color GetColor(int x, int y)
        {
            return (x + y) % 2 == 0 ? Color.DarkGray : Color.White;
        }

        /// <summary>
        /// Getting the marked color of the chess square.
        /// </summary>
        /// <param name="x">The coordinate X.</param>
        /// <param name="y">The coordinate Y.</param>
        /// <returns>The marked color of the chess square.</returns>
        private Color GetMarkedColor(int x, int y)
        {
            return (x + y) % 2 == 0 ? Color.Green : Color.LightGreen;
        }

        /// <summary>
        /// The cell click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Panel_MouseClick(object sender, MouseEventArgs e)
        {
            string xy = ((Panel)sender).Name.Substring(1); // 01
            int x = xy[0] - '0';
            int y = xy[1] - '0';

            if (wait)
            {
                wait = false;
                xFrom = x;
                yFrom = y;
            }
            else
            {
                wait = true;

                // You need to form a string like "Pe2e4"
                string figure = chess.GetFigureAt(xFrom, yFrom).ToString();
                string move = figure + ToCoordinate(xFrom, yFrom) + ToCoordinate(x, y);

                //chess = chess.Move(move); // Test

                // To make the chess move.
                chess = new Chess(chessClient.SendMove(move).FEN);
            }

            ShowPosition();
        }

        /// <summary>
        /// To mark chessboard square.
        /// </summary>
        void MarkSquare()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    chessBoard[x, y].BackColor = GetColor(x, y);
                }
            }


            if (wait)
            {
                MarkSquareFrom();
            }
            else
            {
                MarkSquaresTo();
            }
        }

        /// <summary>
        /// To mark the chess figures what can move.
        /// </summary>
        void MarkSquareFrom()
        {
            foreach (string move in chess.GetAllMoves()) // Pe2e4 -> to find xy = e2
            {
                int x = move[1] - 'a';
                int y = move[2] - '1';

                chessBoard[x, y].BackColor = GetMarkedColor(x, y);
            }
        }

        /// <summary>
        /// To mark the chess squares where the clicked figure can move.
        /// </summary>
        void MarkSquaresTo()
        {
            string suffix = chess.GetFigureAt(xFrom, yFrom) + ToCoordinate(xFrom, yFrom);

            foreach (string move in chess.GetAllMoves()) // Pe2e4 -> to find xy = e4
            {
                if (move.StartsWith(suffix))
                {
                    int x = move[3] - 'a';
                    int y = move[4] - '1';

                    chessBoard[x, y].BackColor = GetMarkedColor(x, y);

                }
            }
        }

        /// <summary>
        /// A handler for updating the chessboard in the multiplayer version of the game 
        /// when a partner has made a move. 
        /// You need to make a mouse click on the edge of the chessboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormChess_MouseClick(object sender, MouseEventArgs e)
        {
            RefreshPosition();
        }

        /// <summary>
        /// Getting the coordinates of a chess square like "E2" or "E4".
        /// </summary>
        /// <param name="x">The coordinate X.</param>
        /// <param name="y">The coordinate Y.</param>
        /// <returns>The coordinates of a chess square like "E2" or "E4".</returns>
        string ToCoordinate(int x, int y)
        {
            return ((char)('a' + x)).ToString() + ((char)('1' + y)).ToString();
        }
    }
}
