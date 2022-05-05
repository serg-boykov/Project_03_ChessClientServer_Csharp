using ChessDll;
using ChessClientDll;
using System;
using UnityEngine;

/// <summary>
/// Class of chess rules for Unity using namespace ChessDll.
/// </summary>
public class Rules : MonoBehaviour
{
    private const string HOST = "https://localhost:44333/api/Games";
    public string USER;

    readonly DragAndDrop m_DragAndDrop;
    Chess chess;
    ChessClient chessClient;

    /// <summary>
    /// The class Rules constructor.
    /// </summary>
    public Rules()
    {
        m_DragAndDrop = new DragAndDrop();
    }

    /// <summary>
    /// This method is called before the first frame update.
    /// </summary>
    public void Start()
    {
        USER = ""; // SystemInfo.deviceUniqueIdentifier.Substring(0, 8);
        string userTest = SystemInfo.deviceUniqueIdentifier[..8];
        Debug.Log("UserID = " + userTest);

        chessClient = new ChessClient(HOST, USER);

        GameInfo gameInfo = chessClient.GetCurrentGame();
        Debug.Log(gameInfo);
        chess = new Chess(gameInfo.FEN);

        ShowFigures();
        MarkValidFigure();
    }

    /// <summary>
    /// This method is called once per frame.
    /// </summary>
    void Update()
    {
        if (m_DragAndDrop.Action())
        {
            // It needs to get a string line like "PE2E4" and make a move.

            // The chess figure made a move FROM the chess square.
            string from = GetSquare(m_DragAndDrop.PickPosition);
            // The chess figure made a move TO the chess square.
            string to = GetSquare(m_DragAndDrop.DropPosition);

            // Shift of the coordinate X.
            int shiftX = 3;
            // Shift of the coordinate Y.
            int shiftY = 3;

            // Getting the coordinates x and y of the chess square
            // from where the chess figure is taken.
            int x = Convert.ToInt32(m_DragAndDrop.PickPosition.x / 2.0) + shiftX;
            int y = Convert.ToInt32(m_DragAndDrop.PickPosition.y / 2.0) + shiftY;

            /*
            // Ñommentation:
            // The left lower chess square should have the coordinates in the scene of objects:
            // m_DragAndDrop.PickPosition.x = 0 / 2.0 + shiftX -> x = 0
            // m_DragAndDrop.PickPosition.y = 0 / 2.0 + shiftY -> y = 0
            //
            // The right upper chess square should have the coordinates in the scene of objects.
            // m_DragAndDrop.PickPosition.x = 14 / 2.0 + shiftX -> x = 7
            // m_DragAndDrop.PickPosition.y = 14 / 2.0 + shiftY -> y = 7
            */

            // Testing of coordinate values.
            //Debug.Log("x !" + x + "! " + "y !" + y);

            // Getting the char of the chess figure on the coordinates.
            string figure = chess.GetFigureAt(x, y).ToString();

            // Geting a string line like "PE2E4".
            string move = figure + from + to;
            //Debug.Log("move !" + move + "! " + "figure !" + figure + "! " +
            //          "from !" + from + "! " + "to !" + to + "! ");


            // To make the move.
            string fenAfterMove = chessClient.SendMove(move).FEN;
            chess = new Chess(fenAfterMove);
            chess = chess.Move(move);
            ShowFigures();
            MarkValidFigure();

            // Refreshing the screen in Unity after the opponent's move
            // in the multiplayer version of the game.
            InvokeRepeating(nameof(Refresh), 2, 2);
        }
    }

    /// <summary>
    /// Refreshing the screen in Unity after the opponent's move
    /// in the multiplayer version of the game.
    /// </summary>
    void Refresh()
    {
        chess = new Chess(chessClient.GetCurrentGame().FEN);
        ShowFigures();
        MarkValidFigure();
    }

    /// <summary>
    /// Getting a chess square like "e2".
    /// </summary>
    /// <param name="position">The position of the chess square.</param>
    /// <returns>The chess square like "e2"</returns>
    private string GetSquare(Vector2 position)
    {
        int x = Convert.ToInt32(position.x / 2.0);
        int y = Convert.ToInt32(position.y / 2.0);

        // Testing of coordinate values.
        //Debug.Log("x !" + x + "! " + "y !" + y);

        // Shift of the coordinate X.
        int shiftX = 3;
        // Shift of the coordinate Y.
        int shiftY = 3;

        /*
        // Ñommentation:
        // The left lower chess square should have the coordinates in the scene of objects:
        // position.x = 0 -> x + shiftX = 0
        // position.y = 0 -> y + 1 + shiftY = 0
        //
        // The right upper chess square should have the coordinates in the scene of objects.
        // position.x = 14 -> x + shiftX = 7
        // position.y = 14 -> y + 1 + shiftY = 7
        */

        return ((char)('a' + x + shiftX)).ToString() + (y + 1 + shiftY).ToString();
    }

    /// <summary>
    /// Show chess figures on the chessboard.
    /// </summary>
    void ShowFigures()
    {
        int nr = 0;

        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                string figure = chess.GetFigureAt(x, y).ToString();

                if (figure == ".") continue;

                PlaceFigure("Box" + nr, figure, x, y);
                nr++;
            }
        }

        for (; nr < 32; nr++)
        {
            PlaceFigure("Box" + nr, "q", 9, 9);
        }

        // Testing the method MarkSquare().
        //MarkSquare(0, 0, true);
        //MarkSquare(0, 1, true);
    }

    /// <summary>
    /// To mark all valid figures that can move.
    /// </summary>
    void MarkValidFigure()
    {
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                // To mark all the squares in the usual color (white, black).
                MarkSquare(x, y, false);
            }
        }

        // Cycle through all valid moves like "Pe2e4".
        foreach (string move in chess.GetAllMoves())
        {
            // Input like: "Pe2e4"

            GetCoordinates(move.Substring(1, 2), out int x, out int y);

            // To mark the required squares in the marked color (whiteMarked, blackMarked).
            MarkSquare(x, y, true);
        }

    }

    /// <summary>
    /// Geting the coordinates of the chess square like the string "e2".
    /// </summary>
    /// <param name="square">The chess square.</param>
    /// <param name="x">The coordinate x.</param>
    /// <param name="y">The coordinate y.</param>
    public void GetCoordinates(string square, out int x, out int y)
    {
        x = 9;
        y = 9;

        if (square.Length == 2 &&
            square[0] >= 'a' && square[0] <= 'h' &&
            square[1] >= '1' && square[1] <= '8')
        {
            x = square[0] - 'a'; // Example: 'a' - 'a' = 0, 'b' - 'a' = 1
            y = square[1] - '1';
        }
    }

    /// <summary>
    /// To put the chess figure and the chess box-square in the required place.
    /// </summary>
    /// <param name="box">The chess box-square.</param>
    /// <param name="figure">The chess figure.</param>
    /// <param name="x">The coordinate x.</param>
    /// <param name="y">The coordinate y.</param>
    private void PlaceFigure(string box, string figure, int x, int y)
    {
        // Testing the input parameters.
        //Debug.Log(box + " " + figure + " " + x + y);

        // Searching for the required objects.
        GameObject goBox = GameObject.Find(box);
        GameObject goFigure = GameObject.Find(figure);
        GameObject goSquare = GameObject.Find("" + y + x);

        // Rendering the required objects.
        var spriteFigure = goFigure.GetComponent<SpriteRenderer>();
        var spriteBox = goBox.GetComponent<SpriteRenderer>();
        spriteBox.sprite = spriteFigure.sprite;

        goBox.transform.position = goSquare.transform.position;
    }

    /// <summary>
    /// To mark the required chess cells-square with the required color.
    /// </summary>
    /// <param name="x">The coordinate X of the chess cell-square.</param>
    /// <param name="y">The coordinate Y of the chess cell-square.</param>
    /// <param name="isMarked">Marked or not.</param>
    void MarkSquare(int x, int y, bool isMarked)
    {
        GameObject goSquare = GameObject.Find("" + y + x);
        
        GameObject goCell;
        string color = (x + y) % 2 == 0 ? "Black" : "White";

        if (isMarked)
        {
            goCell = GameObject.Find(color + "SquareMarked");
        }
        else
        {
            goCell = GameObject.Find(color + "Square");
        }

        var spriteSquare = goSquare.GetComponent<SpriteRenderer>();
        var spriteCell = goCell.GetComponent<SpriteRenderer>();
        spriteSquare.sprite = spriteCell.sprite;
    }

    /// <summary>
    /// Class for drag-and-drop chess figures.
    /// </summary>
    class DragAndDrop
    {
        /// <summary>
        /// The drag-and-drop operation status.
        /// </summary>
        enum State
        {
            none,

            drag
        }

        /// <summary>
        /// The position where the chess figure has been picked.
        /// </summary>
        public Vector2 PickPosition { get; private set; }

        /// <summary>
        /// The position where the chess figure has been droped.
        /// </summary>
        public Vector2 DropPosition { get; private set; }


        /// <summary>
        /// The drag-and-drop operation status.
        /// </summary>
        State state;

        /// <summary>
        /// The chess figure.
        /// </summary>
        GameObject item;

        /// <summary>
        /// The displacement between the position of the figure
        /// and the position of the click by the mouse.
        /// </summary>
        Vector2 offset;

        /// <summary>
        /// Class constructor.
        /// </summary>
        public DragAndDrop()
        {
            state = State.none;
            item = null;
        }

        /// <summary>
        /// The drag-and-drop action.
        /// </summary>
        /// <returns>The chess figure has been droped or not?</returns>
        public bool Action()
        {
            switch (state)
            {
                case State.none:
                    if (IsMouseButtonPressed())
                        PickUp();
                    break;

                case State.drag:
                    if (IsMouseButtonPressed())
                        Drag();
                    else
                    {
                        Drop();
                        return true;
                    }
                    break;
            }

            return false;
        }

        /// <summary>
        /// The chess figure has been droped.
        /// </summary>
        private void Drop()
        {
            DropPosition = item.transform.position;

            state = State.none;
            item = null;
        }

        /// <summary>
        /// The chess figure has been draged.
        /// </summary>
        private void Drag()
        {
            item.transform.position = GetClickPosition() + offset;
        }

        /// <summary>
        /// Are the mouse button pressed?
        /// </summary>
        /// <returns>Yes | No.</returns>
        private bool IsMouseButtonPressed()
        {
            return Input.GetMouseButton(0);
        }

        /// <summary>
        /// The chess figure has been picked up.
        /// </summary>
        private void PickUp()
        {
            Vector2 clickPosition = GetClickPosition();
            var clickedItem = GetItemAt(clickPosition);

            if (clickedItem == null) return;

            PickPosition = clickedItem.position;

            item = clickedItem.gameObject;
            state = State.drag;
            offset = PickPosition - clickPosition;

            // Testing...
            //Debug.Log("picked up " + item.name);
        }

        /// <summary>
        /// Getting the chess figure position.
        /// </summary>
        /// <param name="position">The chess figure position.</param>
        /// <returns>The chess figure position-rotation-scale.</returns>
        private Transform GetItemAt(Vector2 position)
        {
            RaycastHit2D[] figures = Physics2D.RaycastAll(position, position, 0.5f);

            if (figures.Length == 0)
            {
                return null;
            }

            return figures[0].transform;
        }

        /// <summary>
        /// Getting the mouse click position.
        /// </summary>
        /// <returns>The mouse click position.</returns>
        private Vector2 GetClickPosition()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
