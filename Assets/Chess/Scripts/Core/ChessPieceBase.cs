using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPieceBase : MonoBehaviour
{
    public int row, col;
    public bool hasMoved = false;
    public string teamColor; // white or black
}
