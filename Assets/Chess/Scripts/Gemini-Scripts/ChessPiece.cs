using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour
{
    [Header("Piece Setup")]
    public string teamColor;
    public int row;
    public int col;

    [HideInInspector] public bool hasMoved = false;

    // --- SIGNATURE CHANGE HERE ---
    public abstract List<MoveData> GetValidMoves(ChessPiece[,] boardState);

    private void Start()
    {
        transform.position = ChessBoardPlacementHandler.Instance.GetTile(row, col).transform.position;
        ChessBoardPlacementHandler.Instance.RegisterPiece(this);
    }

    public void MoveTo(int newRow, int newCol)
    {
        this.row = newRow;
        this.col = newCol;
        this.hasMoved = true;
        transform.position = ChessBoardPlacementHandler.Instance.GetTile(newRow, newCol).transform.position;
    }

    protected bool IsInsideBoard(int r, int c)
    {
        return r >= 0 && r < 8 && c >= 0 && c < 8;
    }
}