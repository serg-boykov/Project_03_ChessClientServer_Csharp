using ChessClientDll;
using ChessDll;
using System;
using System.Text;
using static System.Console;

namespace ChessClientConsole
{
    /*
     * По умолчанию Console имеет свойства:
     * Шрифт: Consolas
     * Размер шрифта: 16
     * Размер окна:
     *    Ширина: 120
     *    Высота: 30
     * 
     * Нужно установить:
     * Шрифт: MS Gothic
     * Рзамер шрифта: 28
     * Размер окна:
     *    Ширина: 30
     *    Высота: 30
     */


    /// <summary>
    /// The console chess client.
    /// </summary>
    internal class Program
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
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            Program program = new Program();
            program.Start();
        }


        /// <summary>
        /// The chess game client.
        /// </summary>
        Chess chess;


        /// <summary>
        /// The client starts.
        /// </summary>
        private void Start()
        {
            SetWindowSize(24, 29);

            ChessClient client = new ChessClient(HOST, USER);
            GameInfo gameInfo = client.GetCurrentGame();

            while (true)
            {
                Clear();
                chess = new Chess(gameInfo.FEN);
                Print(ChessToAscii());

                WriteLine(gameInfo);

                Write("\nEnter your move: ");
                string move = ReadLine();

                if (move.Length == 5)
                {
                    gameInfo = client.SendMove(move);
                }
                else if (move == "")
                {
                    gameInfo = client.GetCurrentGame();
                }
                else if (move == "q")
                {
                    SetWindowSize(120, 30);
                    break;
                }
            }
        }

        /// <summary>
        /// Printing the text on the console screen.
        /// </summary>
        /// <param name="text">The text on the console screen.</param>
        private void Print(string text)
        {
            OutputEncoding = Encoding.Unicode;
            ConsoleColor fcOld = ForegroundColor;
            ConsoleColor bcOld = BackgroundColor;

            // The number of characters in the string line
            // responsible for the chessboard and chess figures.
            int numStrBoard = 251;

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] >= 'a' && text[i] <= 'z' && i < numStrBoard)
                {
                    ForegroundColor = ConsoleColor.Black;
                    BackgroundColor = ConsoleColor.White;
                }
                else if (text[i] >= 'A' && text[i] <= 'Z' && i < numStrBoard)
                {
                    ForegroundColor = ConsoleColor.White;
                    BackgroundColor = ConsoleColor.Black;
                }
                else
                {
                    ForegroundColor = ConsoleColor.Cyan;
                }

                PrintChar(text[i], i);
            }

            // Restoring the color.
            ForegroundColor = fcOld;
            BackgroundColor = bcOld;
        }

        /// <summary>
        /// Printing the font symbol MS Gothic (This font has chess figures).
        /// Replacing letters with chess figures.
        /// </summary>
        /// <param name="f">The font symbol.</param>
        /// <param name="i">Font symbol index.</param>
        private void PrintChar(char f, int i)
        {
            int minPos = 72;
            int maxPos = 232;

            if (i > minPos && i < maxPos)
            {
                switch (f)
                {
                    case 'K': SetBGColor(i); Write('\u2654' + " "); break;
                    case 'Q': SetBGColor(i); Write('\u2655' + " "); break;
                    case 'R': SetBGColor(i); Write('\u2656' + " "); break;
                    case 'B': SetBGColor(i); Write('\u2657' + " "); break;
                    case 'N': SetBGColor(i); Write('\u2658' + " "); break;
                    case 'P': SetBGColor(i); Write('\u2659' + " "); break;
                    case 'k': SetBGColor(i); Write('\u265A' + " "); break;
                    case 'q': SetBGColor(i); Write('\u265B' + " "); break;
                    case 'r': SetBGColor(i); Write('\u265C' + " "); break;
                    case 'b': SetBGColor(i); Write('\u265D' + " "); break;
                    case 'n': SetBGColor(i); Write('\u265E' + " "); break;
                    case 'p': SetBGColor(i); Write('\u265F' + ""); break;
                    case '.': SetBGColor(i); Write("  "); break;
                    default: Write(f); break;

                }
            }
            else
            {
                Write(f);
            }

            BackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// To set the background color.
        /// </summary>
        /// <param name="i">Font symbol index.</param>
        void SetBGColor(int i) => BackgroundColor = (i % 2 == 0) ? ConsoleColor.DarkGray : ConsoleColor.Gray;

        /// <summary>
        /// Creating the chessboard.
        /// </summary>
        /// <returns>The chessboard as string.</returns>
        private string ChessToAscii()
        {
            var sb = new StringBuilder();

            sb.AppendLine("                     ");
            sb.AppendLine("    a b c d e f g h  ");
            sb.AppendLine("   +----------------+");

            for (int y = 7; y >= 0; y--)
            {
                sb.Append(" ");
                sb.Append(y + 1);
                sb.Append(" |");

                for (int x = 0; x < 8; x++)
                {
                    sb.Append(chess.GetFigureAt(x, y));
                }

                sb.AppendLine("| " + (y + 1));
            }

            sb.AppendLine("   +----------------+");
            sb.AppendLine("    A B C D E F G H  ");
            sb.AppendLine("                     ");

            // If there is CHECK.
            if (chess.IsCheck())
            {
                sb.AppendLine("CHECK!");
            }

            // You can also implement a check on Checkmate and Stalemate.

            return sb.ToString();
        }
    }
}
