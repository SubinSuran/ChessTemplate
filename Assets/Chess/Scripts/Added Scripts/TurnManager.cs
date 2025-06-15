using UnityEngine;
using System;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public string CurrentTurn { get; private set; } = "White";
    public int TurnNumber { get; private set; } = 1;

    public event Action<string> OnTurnChanged;

    private void Awake()
    {
        Instance = this;
    }

    public void EndTurn()
    {
        CurrentTurn = CurrentTurn == "White" ? "Black" : "White";
        // A full turn completes after Black moves
        if (CurrentTurn == "White")
        {
            TurnNumber++;
        }
        Debug.Log("Turn " + TurnNumber + ". It's " + CurrentTurn + "'s turn.");
        OnTurnChanged?.Invoke(CurrentTurn);
    }
}