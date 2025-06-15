using UnityEngine;
using System;

public class InputHandler : MonoBehaviour
{
    // Events to notify other systems about what was clicked.
    public event Action<ChessPiece> OnPieceClicked;
    public event Action<int, int> OnTileClicked;

    private void HandleClick()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider == null) return;

        GameObject clickedObject = hit.collider.gameObject;

        // --- NEW LOGIC START ---

        // STATE 1: A piece is already selected, so this click is a MOVE attempt.
        if (GameController.Instance.SelectedPiece != null)
        {
            // We prioritize getting tile data, whether from a marker or the piece itself.
            HighlightMarker marker = clickedObject.GetComponentInChildren<HighlightMarker>();
            if (marker != null)
            {
                OnTileClicked?.Invoke(marker.targetRow, marker.targetCol);
                return; // Move handled
            }

            // If we didn't click a marker, check if we clicked directly on another piece
            ChessPiece pieceOnTile = clickedObject.GetComponent<ChessPiece>();
            if (pieceOnTile != null)
            {
                // This is the key: we treat a click on ANY piece as a click on its TILE.
                OnTileClicked?.Invoke(pieceOnTile.row, pieceOnTile.col);
                return; // Move handled
            }
        }

        // STATE 2: No piece is selected, so this click is a SELECTION attempt.
        // This code only runs if the 'if' block above doesn't handle the click.
        ChessPiece pieceToSelect = clickedObject.GetComponent<ChessPiece>();
        if (pieceToSelect != null)
        {
            OnPieceClicked?.Invoke(pieceToSelect);
        }
        // --- NEW LOGIC END ---
    }

    // Your Update method stays the same
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }
}