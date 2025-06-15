using UnityEngine;

// Defines the type of move
public enum MoveType { Normal, Capture }

// A structure to hold all information about a potential move
public struct MoveData
{
    public readonly Vector2Int Position;
    public readonly MoveType Type;

    public MoveData(Vector2Int position, MoveType type)
    {
        Position = position;
        Type = type;
    }
}