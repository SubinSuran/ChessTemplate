using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public override List<MoveData> GetValidMoves(ChessPiece[,] boardState)
    {
        List<MoveData> moves = new List<MoveData>();
        int dir = teamColor == "White" ? 1 : -1;

        // --- Forward Movement ---
        int oneStepRow = row + dir;
        if (IsInsideBoard(oneStepRow, col) && boardState[oneStepRow, col] == null)
        {
            moves.Add(new MoveData(new Vector2Int(oneStepRow, col), MoveType.Normal));

            int twoStepRow = row + 2 * dir;
            if (!hasMoved && IsInsideBoard(twoStepRow, col) && boardState[twoStepRow, col] == null)
            {
                moves.Add(new MoveData(new Vector2Int(twoStepRow, col), MoveType.Normal));
            }
        }

        // --- Diagonal Captures ---
        if (IsInsideBoard(oneStepRow, col + 1))
        {
            ChessPiece pieceOnTile = boardState[oneStepRow, col + 1];
            if (pieceOnTile != null && pieceOnTile.teamColor != this.teamColor)
            {
                moves.Add(new MoveData(new Vector2Int(oneStepRow, col + 1), MoveType.Capture));
            }
        }
        if (IsInsideBoard(oneStepRow, col - 1))
        {
            ChessPiece pieceOnTile = boardState[oneStepRow, col - 1];
            if (pieceOnTile != null && pieceOnTile.teamColor != this.teamColor)
            {
                moves.Add(new MoveData(new Vector2Int(oneStepRow, col - 1), MoveType.Capture));
            }
        }

        return moves;
    }
}