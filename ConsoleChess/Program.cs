using ChessClientDll;
using System;
using static System.Console;

namespace ConsoleChess
{
    /// <summary>
    /// Testing the chess client dll.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The route to the Web API controller.
        /// </summary>
        public const string HOST = "https://localhost:44333/api/Games";

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
        ChessClient client;

        /// <summary>
        /// The client starts.
        /// </summary>
        private void Start()
        {
            client = new ChessClient(HOST, USER);

            // To print information about the current game.
            Console.WriteLine(client.GetCurrentGame());

            // 
            while (true)
            {
                Write("\nYour Move: ");
                string move = ReadLine();

                if (move == "q")
                {
                    return;
                }

                // To clean the console screen.
                Clear();

                // To make a chess move and to print info.
                WriteLine(client.SendMove(move));
            }
        }
    }
}
