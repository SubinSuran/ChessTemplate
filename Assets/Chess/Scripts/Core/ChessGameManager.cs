
using System.Collections.Generic;
using UnityEngine;

public class ChessGameManager : MonoBehaviour
{
    public static ChessGameManager Instance;

    private string currentTurn = "White";
    private ChessPieceBase selectedPiece;

    private List<Vector2Int> validMovePositions = new List<Vector2Int>();

    private void Awake()
    {
        Instance = this;
        Debug.Log("ChessGameManager Awake: Turn = " + currentTurn);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;

                ChessPieceBase piece = clickedObject.GetComponent<ChessPieceBase>();
                if (piece != null)
                {
                    TrySelectPiece(piece);
                    return;
                }

                // Try move to valid highlighted tile
                HighlightMarker marker = clickedObject.GetComponentInChildren<HighlightMarker>();
                if (marker != null && selectedPiece != null)
                {
                    Vector2Int movePos = new Vector2Int(marker.targetRow, marker.targetCol);
                    if (validMovePositions.Contains(movePos))
                    {
                        MoveSelectedPiece(marker.targetRow, marker.targetCol);
                        return;
                    }
                    else
                    {
                        Debug.Log("Invalid move. That tile isn't highlighted.");
                    }
                }
            }
        }
    }

    private void TrySelectPiece(ChessPieceBase piece)
    {
        Debug.Log("Trying to select: " + piece.name + " | Team: " + piece.teamColor + " | Turn: " + currentTurn);

        if (piece.teamColor != currentTurn)
        {
            Debug.Log("Wrong turn! It's " + currentTurn + "'s turn.");
            return;
        }

        selectedPiece = piece;
        validMovePositions.Clear();
        ChessBoardPlacementHandler.Instance.ClearHighlights();

        if (piece.name.Contains("Pawn")) ShowPawnMoves(piece);
        else if (piece.name.Contains("Rook")) ShowRookMoves(piece);
        else if (piece.name.Contains("Knight")) ShowKnightMoves(piece);
        else if (piece.name.Contains("Bishop")) ShowBishopMoves(piece);
        else if (piece.name.Contains("Queen"))
        {
            ShowRookMoves(piece);
            ShowBishopMoves(piece);
        }
        else if (piece.name.Contains("King")) ShowKingMoves(piece);
    }

    private void ShowPawnMoves(ChessPieceBase pawn)
    {
        int dir = pawn.teamColor == "White" ? 1 : -1;

        TryHighlight(pawn.row + dir, pawn.col);
        if (!pawn.hasMoved)
            TryHighlight(pawn.row + 2 * dir, pawn.col);
    }

    private void ShowRookMoves(ChessPieceBase piece)
    {
        for (int i = 1; i < 8; i++)
        {
            TryHighlight(piece.row + i, piece.col);
            TryHighlight(piece.row - i, piece.col);
            TryHighlight(piece.row, piece.col + i);
            TryHighlight(piece.row, piece.col - i);
        }
    }

    private void ShowBishopMoves(ChessPieceBase piece)
    {
        for (int i = 1; i < 8; i++)
        {
            TryHighlight(piece.row + i, piece.col + i);
            TryHighlight(piece.row - i, piece.col - i);
            TryHighlight(piece.row + i, piece.col - i);
            TryHighlight(piece.row - i, piece.col + i);
        }
    }

    private void ShowKnightMoves(ChessPieceBase knight)
    {
        int[,] offsets = new int[,]
        {
            { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 },
            { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 }
        };

        for (int i = 0; i < offsets.GetLength(0); i++)
        {
            int newRow = knight.row + offsets[i, 0];
            int newCol = knight.col + offsets[i, 1];
            TryHighlight(newRow, newCol);
        }
    }

    private void ShowKingMoves(ChessPieceBase king)
    {
        for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
        {
            for (int colOffset = -1; colOffset <= 1; colOffset++)
            {
                if (rowOffset != 0 || colOffset != 0)
                    TryHighlight(king.row + rowOffset, king.col + colOffset);
            }
        }
    }

    private void TryHighlight(int row, int col)
    {
        if (IsInsideBoard(row, col))
        {
            ChessBoardPlacementHandler.Instance.Highlight(row, col);
            validMovePositions.Add(new Vector2Int(row, col));
        }
    }

    private bool IsInsideBoard(int row, int col)
    {
        return row >= 0 && row < 8 && col >= 0 && col < 8;
    }

    private void MoveSelectedPiece(int newRow, int newCol)
    {
        GameObject tile = ChessBoardPlacementHandler.Instance.GetTile(newRow, newCol);
        selectedPiece.transform.position = tile.transform.position;

        selectedPiece.row = newRow;
        selectedPiece.col = newCol;
        selectedPiece.hasMoved = true;

        validMovePositions.Clear();
        EndTurn();
    }

    private void EndTurn()
    {
        currentTurn = currentTurn == "White" ? "Black" : "White";
        selectedPiece = null;
        ChessBoardPlacementHandler.Instance.ClearHighlights();
        Debug.Log("Turn ended. Now it's " + currentTurn + "'s turn.");
    }
}