// This example is for Rook.cs. Apply the same change to Bishop.cs and Queen.cs
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{
    public override List<MoveData> GetValidMoves(ChessPiece[,] boardState)
    {
        List<MoveData> moves = new List<MoveData>();
        CheckDirection(moves, 1, 0, boardState);
        CheckDirection(moves, -1, 0, boardState);
        CheckDirection(moves, 0, 1, boardState);
        CheckDirection(moves, 0, -1, boardState);
        return moves;
    }

    private void CheckDirection(List<MoveData> moves, int rowDir, int colDir, ChessPiece[,] boardState)
    {
        for (int i = 1; i < 8; i++)
        {
            int newRow = row + (i * rowDir);
            int newCol = col + (i * colDir);

            if (!IsInsideBoard(newRow, newCol)) break;

            ChessPiece pieceOnTile = boardState[newRow, newCol];
            if (pieceOnTile == null)
            {
                moves.Add(new MoveData(new Vector2Int(newRow, newCol), MoveType.Normal));
            }
            else
            {
                if (pieceOnTile.teamColor != this.teamColor)
                {
                    moves.Add(new MoveData(new Vector2Int(newRow, newCol), MoveType.Capture));
                }
                break;
            }
        }
    }
}