using System.Collections.Generic;
using UnityEngine;

public class PawnMovement : MonoBehaviour, IMoveProvider
{
    public List<Vector2Int> GetValidMoves(ChessPieceBase piece)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        int dir = piece.teamColor == "White" ? 1 : -1;

        AddMove(piece.row + dir, piece.col);
        if (!piece.hasMoved)
            AddMove(piece.row + 2 * dir, piece.col);

        return moves;

        void AddMove(int r, int c)
        {
            if (IsInsideBoard(r, c))
                moves.Add(new Vector2Int(r, c));
        }
    }

    private bool IsInsideBoard(int row, int col) => row >= 0 && row < 8 && col >= 0 && col < 8;
}
