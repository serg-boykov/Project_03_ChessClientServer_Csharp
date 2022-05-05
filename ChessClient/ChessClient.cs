using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace ChessClientDll
{
    /// <summary>
    /// The chess client dll.
    /// </summary>
    public class ChessClient
    {
        /// <summary>
        /// The route to the Web API controller.
        /// </summary>
        public string Host { get; private set; }

        /// <summary>
        /// The game user name.
        /// </summary>
        public string User { get; private set; }

        /// <summary>
        /// The identifier of the current game.
        /// </summary>
        int CurrentGameID;

        /// <summary>
        /// The class constructor.
        /// </summary>
        /// <param name="host">The route to the Web API controller.</param>
        /// <param name="user">The game user name.</param>
        public ChessClient(string host, string user)
        {
            Host = host;
            User = user;
        }

        /// <summary>
        /// Getting information about the current game.
        /// </summary>
        /// <returns>Info about the current game.</returns>
        public GameInfo GetCurrentGame()
        {
            GameInfo gameInfo = new GameInfo(ParseJson(CallServer()));
            CurrentGameID = gameInfo.GameID;

            return gameInfo;
        }

        /// <summary>
        /// The client sends a chess move to the server.
        /// </summary>
        /// <param name="move">The chess move to the server.</param>
        /// <returns>Info about the current game.</returns>
        public GameInfo SendMove(string move)
        {
            string json = CallServer(CurrentGameID + "/" + move);
            var list = ParseJson(json);
            GameInfo gameInfo = new GameInfo(list);

            return gameInfo;
        }

        /// <summary>
        /// Request to the server.
        /// </summary>
        /// <param name="param">The request params.</param>
        /// <returns>Response from the server.</returns>
        private string CallServer(string param = "")
        {
            WebRequest request = WebRequest.Create(Host + User + "/" + param);
            WebResponse response = request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Parsing json-format string.
        /// </summary>
        /// <param name="json">json-format string.</param>
        /// <returns>List of key-value pairs.</returns>
        private NameValueCollection ParseJson(string json)
        {
            NameValueCollection list = new NameValueCollection();

            // Regular expression.
            string pattern = @"""(\w+)"":""?([^,""}]*)""?";

            foreach (Match m in Regex.Matches(json, pattern))
            {
                if (m.Groups.Count == 3)
                {
                    list[m.Groups[1].Value] = m.Groups[2].Value;
                }
            }

            return list;
        }
    }
}
