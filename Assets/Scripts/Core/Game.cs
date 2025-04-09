using System.Collections.Generic;
using UnityEngine;

static class Game
{
    private static Dictionary<int, GameObject> pieceToGameObject;
    private static BotManager botManager;

    public static void Init(string fen, int botColor, Dictionary<int, GameObject> pieceToGameObject)
    {
        Game.pieceToGameObject = pieceToGameObject;
        botManager = Object.FindObjectOfType<BotManager>();

        Board.Init(fen);
        GameState.Init(fen);
        Player.Init(Piece.OppositeColor(botColor));
        Bot.Init(botColor);
        ObjectBoard.Init();  // ObjectBoard.Init() must be called after Board.Init() and Player.Init()

        if (GameState.ColorToMove == Player.Color)
        {
            // Player's turn
            Player.CurrentLegalMoves = LegalMoves.GetLegalMoves();
        }
        else
        {
            // Bot's turn
            botManager.StartBotTurn();
        }
    }

    public static Dictionary<int, GameObject> PieceToGameObject
    {
        get { return pieceToGameObject; }
    }

    public static string Fen()
    {
        return Board.HalfFen() + " " + GameState.HalfFen();
    }

    public static void ExecuteMove(Move move)
    {

        // These all must happen in this order
        ObjectBoard.RecordMove(move);
        GameState.RecordMove(move);
        GameState.UpdatePgn(move);  // Only needs to happen once when we decide to execute
        Board.RecordMove(move);

        Debug.Log("PGN: " + string.Join(" ", GameState.Pgn));

        if (GameState.ColorToMove == Player.Color)
        {
            // Now its player's turn
            Player.CurrentLegalMoves = LegalMoves.GetLegalMoves();
        }
        else
        {
            // Now its bot's turn
            botManager.StartBotTurn();
        }
    }
}
