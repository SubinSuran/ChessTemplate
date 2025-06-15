using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{
    public override List<MoveData> GetValidMoves(ChessPiece[,] boardState)
    {
        List<MoveData> moves = new List<MoveData>();
        int[,] offsets = new int[,]
        {
            { 1, 2 }, { 1, -2 }, { -1, 2 }, { -1, -2 },
            { 2, 1 }, { 2, -1 }, { -2, 1 }, { -2, -1 }
        };

        for (int i = 0; i < offsets.GetLength(0); i++)
        {
            int newRow = row + offsets[i, 0];
            int newCol = col + offsets[i, 1];

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
        return moves;
    }
}