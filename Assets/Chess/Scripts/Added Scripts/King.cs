using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public override List<MoveData> GetValidMoves(ChessPiece[,] boardState)
    {
        List<MoveData> moves = new List<MoveData>();

        for (int rowOffset = -1; rowOffset <= 1; rowOffset++)
        {
            for (int colOffset = -1; colOffset <= 1; colOffset++)
            {
                // Skip the square the king is currently on
                if (rowOffset == 0 && colOffset == 0) continue;

                int newRow = row + rowOffset;
                int newCol = col + colOffset;

                if (IsInsideBoard(newRow, newCol))
                {
                    ChessPiece pieceOnTile = boardState[newRow, newCol];
                    if (pieceOnTile == null)
                    {
                        moves.Add(new MoveData(new Vector2Int(newRow, newCol), MoveType.Normal));
                    }
                    else if (pieceOnTile.teamColor != this.teamColor)
                    {
                        moves.Add(new MoveData(new Vector2Int(newRow, newCol), MoveType.Capture));
                    }
                }
            }
        }
        // Castling logic would be added here
        return moves;
    }
}