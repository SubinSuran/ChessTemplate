using UnityEngine;
using System.Diagnostics.CodeAnalysis;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public sealed class ChessBoardPlacementHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] _rowsArray;
    [SerializeField] private GameObject _highlightPrefab;
    [SerializeField] private GameObject _captureHighlightPrefab;

    // Your original visual board array
    private GameObject[,] _chessBoard;
    // The new logical board array that tracks pieces
    private readonly ChessPiece[,] _boardState = new ChessPiece[8, 8];

    // Changed from internal to public for broader, safer access
    public static ChessBoardPlacementHandler Instance;

    private void Awake()
    {
        Instance = this;
        GenerateArray();
    }

    // --- NEW METHODS FOR LOGICAL STATE MANAGEMENT ---

    /// <summary>
    /// Gets the logical state of the board.
    /// </summary>
    public ChessPiece[,] GetBoardState()
    {
        return _boardState;
    }

    /// <summary>
    /// Gets the piece at a specific coordinate.
    /// </summary>
    public ChessPiece GetPieceAt(int row, int col)
    {
        if (!IsInsideBoard(row, col)) return null;
        return _boardState[row, col];
    }

    /// <summary>
    /// Allows a piece to register itself with the board state.
    /// This is called by each piece at the start of the game.
    /// </summary>
    public void RegisterPiece(ChessPiece piece)
    {
        if (IsInsideBoard(piece.row, piece.col))
        {
            _boardState[piece.row, piece.col] = piece;
        }
    }

    /// <summary>
    /// Moves a piece logically and visually, handling captures.
    /// </summary>
    public void MovePieceOnBoard(ChessPiece piece, int newRow, int newCol)
    {
        // 1. Check if there is already a piece at the destination.
        ChessPiece pieceAtDestination = _boardState[newRow, newCol];

        // 2. If a piece exists, it must be an enemy. Destroy it!
        if (pieceAtDestination != null)
        {
            // THIS IS THE "KILLING" LOGIC
            Destroy(pieceAtDestination.gameObject);
        }

        // 3. Clear the piece's old logical position.
        _boardState[piece.row, piece.col] = null;

        // 4. Set the piece's new logical position.
        _boardState[newRow, newCol] = piece;

        // 5. Tell the piece to update its visual position on the board.
        piece.MoveTo(newRow, newCol);
    }

    private bool IsInsideBoard(int row, int col)
    {
        return row >= 0 && row < 8 && col >= 0 && col < 8;
    }

    // --- YOUR ORIGINAL METHODS (Changed to public) ---

    private void GenerateArray()
    {
        _chessBoard = new GameObject[8, 8];
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                _chessBoard[i, j] = _rowsArray[i].transform.GetChild(j).gameObject;
            }
        }
    }

    public GameObject GetTile(int i, int j)
    {
        if (!IsInsideBoard(i, j))
        {
            Debug.LogError("Invalid row or column.");
            return null;
        }
        return _chessBoard[i, j];
    }

    public void Highlight(int row, int col, MoveType moveType)
    {
        var tile = GetTile(row, col);
        if (tile == null) return;

        // Choose the correct prefab based on the move type
        GameObject prefabToUse = (moveType == MoveType.Capture) ? _captureHighlightPrefab : _highlightPrefab;

        // The rest of the method is the same
        GameObject marker = Instantiate(prefabToUse, tile.transform.position, Quaternion.identity, tile.transform);

        HighlightMarker highlightMarker = marker.AddComponent<HighlightMarker>();
        highlightMarker.targetRow = row;
        highlightMarker.targetCol = col;
    }

    public void ClearHighlights()
    {
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                var tile = GetTile(i, j);
                if (tile.transform.childCount <= 0) continue;

                // Simplified loop to destroy children
                foreach (Transform childTransform in tile.transform)
                {
                    if (childTransform.GetComponent<HighlightMarker>() != null)
                    {
                        Destroy(childTransform.gameObject);
                    }
                }
            }
        }
    }
}

// You will need this script if it's not already defined elsewhere
