using System.Collections.Generic;
using UnityEngine;
using System.Linq; // <-- Make sure this is included!

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    // References to the other systems
    private InputHandler inputHandler;
    private TurnManager turnManager;
    private ChessBoardPlacementHandler boardHighlighter;

    private ChessPiece selectedPiece;
    private List<MoveData> validMovePositions = new List<MoveData>();

    private void Awake()
    {
        Instance = this;
        inputHandler = FindObjectOfType<InputHandler>();
        turnManager = FindObjectOfType<TurnManager>();
        boardHighlighter = FindObjectOfType<ChessBoardPlacementHandler>();
    }

    private void OnEnable()
    {
        inputHandler.OnPieceClicked += HandlePieceSelection;
        inputHandler.OnTileClicked += HandleMoveAttempt;
        turnManager.OnTurnChanged += OnTurnChanged;
    }

    private void OnDisable()
    {
        inputHandler.OnPieceClicked -= HandlePieceSelection;
        inputHandler.OnTileClicked -= HandleMoveAttempt;
        turnManager.OnTurnChanged -= OnTurnChanged;
    }

    private void HandlePieceSelection(ChessPiece piece)
    {
        // Enforce "Pawn First" rule on the very first move of the game
        if (turnManager.TurnNumber == 1 && turnManager.CurrentTurn == "White" && !(piece is Pawn))
        {
            Debug.Log("First move of the game must be a Pawn!");
            return;
        }

        // Check if it's the correct turn
        if (piece.teamColor != turnManager.CurrentTurn)
        {
            Debug.Log("Wrong turn! It's " + turnManager.CurrentTurn + "'s turn.");
            return;
        }

        // Select the piece and get its moves, passing the current board state
        selectedPiece = piece;
        validMovePositions = selectedPiece.GetValidMoves(boardHighlighter.GetBoardState());

        // Delegate highlighting to the board highlighter
        boardHighlighter.ClearHighlights();
        foreach (var move in validMovePositions)
        {
            // We now pass the move type to the Highlight method
            boardHighlighter.Highlight(move.Position.x, move.Position.y, move.Type);
        }
    }

    private void HandleMoveAttempt(int row, int col)
    {
        if (selectedPiece == null) return;

        Vector2Int movePos = new Vector2Int(row, col);

        // Instead of a simple .Contains, we check if any move's position matches.
        if (validMovePositions.Any(move => move.Position == movePos))
        {
            // Tell the board handler to move the piece. This updates the board state array
            // and handles captures.
            boardHighlighter.MovePieceOnBoard(selectedPiece, row, col);

            turnManager.EndTurn();
        }
        else
        {
            Debug.Log("Invalid move. That tile isn't highlighted.");
            ClearSelection();
        }
    }

    private void OnTurnChanged(string newTurn)
    {
        ClearSelection();
    }

    private void ClearSelection()
    {
        selectedPiece = null;
        validMovePositions.Clear();
        if (boardHighlighter != null)
        {
            boardHighlighter.ClearHighlights();
        }
    }
}