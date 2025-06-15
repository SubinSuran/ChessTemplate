using System.Collections.Generic;
using UnityEngine;

public interface IMoveProvider
{
    List<Vector2Int> GetValidMoves(ChessPieceBase piece);
}
