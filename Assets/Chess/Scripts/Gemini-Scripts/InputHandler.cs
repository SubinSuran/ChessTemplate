using UnityEngine;
using System;

public class InputHandler : MonoBehaviour
{
    // Events to notify other systems about what was clicked.
    public event Action<ChessPiece> OnPieceClicked;
    public event Action<int, int> OnTileClicked;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    private void HandleClick()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider == null) return;

        GameObject clickedObject = hit.collider.gameObject;

        ChessPiece piece = clickedObject.GetComponent<ChessPiece>();
        if (piece != null)
        {
            OnPieceClicked?.Invoke(piece);
            return;
        }

        HighlightMarker marker = clickedObject.GetComponentInChildren<HighlightMarker>();
        if (marker != null)
        {
            OnTileClicked?.Invoke(marker.targetRow, marker.targetCol);
            return;
        }
    }
}