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

                HighlightMarker marker = clickedObject.GetComponentInChildren<HighlightMarker>();
                if (marker != null && selectedPiece != null)
                {
                    Vector2Int movePos = new Vector2Int(marker.targetRow, marker.targetCol);
                    if (validMovePositions.Contains(movePos))
                    {
                        MoveSelectedPiece(marker.targetRow, marker.targetCol);
                    }
                }
            }
        }
    }

    private void TrySelectPiece(ChessPieceBase piece)
    {
        if (piece.teamColor != currentTurn)
        {
            Debug.Log("Wrong turn! It's " + currentTurn + "'s turn.");
            return;
        }

        selectedPiece = piece;
        validMovePositions.Clear();
        ChessBoardPlacementHandler.Instance.ClearHighlights();

        validMovePositions = selectedPiece.GetMoves();

        foreach (var pos in validMovePositions)
        {
            ChessBoardPlacementHandler.Instance.Highlight(pos.x, pos.y);
        }
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
