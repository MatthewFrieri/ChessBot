using System.Collections.Generic;
using UnityEngine;

static class Game
{
    private static Dictionary<int, GameObject> pieceToGameObject;

    public static void Init(string fen, int botColor, Dictionary<int, GameObject> pieceToGameObject)
    {
        Game.pieceToGameObject = pieceToGameObject;

        Board.Init(fen);
        GameState.Init(fen);
        Player.Init(Piece.OppositeColor(botColor));
        Bot.Init(botColor);
        ObjectBoard.Init();  // ObjectBoard.Init() must be called after Board.Init() and Player.Init()

        if (GameState.ColorToMove == botColor)
        {
            // Bot's turn
            Bot.MakeMove();
        }
        else
        {
            // Player's turn
            Player.CurrentLegalMoves = LegalMoves.GetLegalMoves();
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
        // RecordMove() must happen in this order
        ObjectBoard.RecordMove(move);
        GameState.RecordMove(move);
        Board.RecordMove(move);

        if (GameState.ColorToMove == Player.Color)
        {
            // Now its player's turn
            Player.CurrentLegalMoves = LegalMoves.GetLegalMoves();
        }
        else
        {
            // Now its bot's turn
            Bot.MakeMove();
        }
    }


}
