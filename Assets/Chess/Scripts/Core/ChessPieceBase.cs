using System.Collections.Generic;
using UnityEngine;

public class ChessPieceBase : MonoBehaviour
{
    public string teamColor; // "White" or "Black"
    public int row;
    public int col;
    public bool hasMoved = false;

    public IMoveProvider moveProvider;

    public List<Vector2Int> GetMoves()
    {
        return moveProvider?.GetValidMoves(this) ?? new List<Vector2Int>();
    }
}