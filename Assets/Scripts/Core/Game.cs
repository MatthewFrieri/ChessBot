using System.Collections.Generic;
using System.Linq;
using UnityEngine;

static class Game
{
    private static Dictionary<int, GameObject> pieceToGameObject;
    private static GameManager gameManager;
    private static BotManager botManager;
    private static bool isCheckmate;
    private static bool isDraw;

    public static void Init(float time, int botColor, Dictionary<int, GameObject> pieceToGameObject, string pgn = "")
    {
        Game.pieceToGameObject = pieceToGameObject;
        gameManager = Object.FindObjectOfType<GameManager>();
        botManager = Object.FindObjectOfType<BotManager>();
        isCheckmate = false;
        isDraw = false;

        Board.Init();
        GameState.Init(time);
        Player.Init(Piece.OppositeColor(botColor));
        Bot.Init(botColor);
        ObjectBoard.Init();  // ObjectBoard.Init() must be called after Board.Init() and Player.Init()

        RecordStartPgn(pgn);


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

    private static void RecordStartPgn(string pgn)
    {
        if (pgn == "") { return; }

        List<string> algebraics = new List<string>();

        // Remove the move numbers
        foreach (string algebraic in pgn.Split(' '))
        {
            if (char.IsDigit(algebraic[0])) { continue; }
            algebraics.Add(algebraic);
        }

        // Execute moves
        foreach (string algebraic in algebraics)
        {
            Move move = PgnUtility.AlgebraicToMove(algebraic);
            ExecuteMove(move, true);
        }
    }


    public static Dictionary<int, GameObject> PieceToGameObject
    {
        get { return pieceToGameObject; }
    }

    public static bool IsCheckmate
    {
        get { return isCheckmate; }
    }

    public static bool IsDraw
    {
        get { return isDraw; }
    }


    public static bool IsGameOver
    {
        get { return isCheckmate || isDraw; }
    }

    public static string Fen()
    {
        return Board.HalfFen() + " " + GameState.HalfFen();
    }

    public static void ExecuteMove(Move move, bool isSetup = false)
    {
        // These all must happen in this order
        ObjectBoard.RecordMove(move);
        GameState.RecordMove(move);
        GameState.UpdatePgn(move);  // Only needs to happen once when we decide to execute
        Board.RecordMove(move);

        if (isSetup) { return; }

        Debug.Log("PGN: " + string.Join(" ", GameState.Pgn));

        PlayCorrectSound();

        string algebraic = GameState.Pgn.Last();
        if (algebraic.Contains("#"))
        {
            Debug.Log("GAME OVER!");
            isCheckmate = true;
        }
        else
        {
            StartNextMove();
        }

    }

    private static void StartNextMove()
    {
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

    private static void PlayCorrectSound()
    {
        string algebraic = GameState.Pgn.Last();
        if (algebraic.Contains("#"))
        {
            gameManager.PlayGameEndSound();
        }
        else if (algebraic.Contains("+"))
        {
            gameManager.PlayCheckSound();
        }
        else if (algebraic.Contains("x"))
        {
            gameManager.PlayCaptureSound();
        }
        else if (algebraic.Contains("O-O"))
        {
            gameManager.PlayCastleSound();
        }
        else
        {
            gameManager.PlayMoveSound();
        }
    }
}
