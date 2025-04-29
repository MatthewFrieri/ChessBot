using System.Collections.Generic;
using System.Linq;
using UnityEngine;

static class GameState
{
    private static Stack<int> colorToMoveStack;
    private static Stack<int?> vulnerableEnPassantSquareStack;
    private static Stack<List<int>> castleSquaresStack;
    private static Stack<int> halfMoveClockStack;
    private static Stack<int> fullMoveNumberStack;
    private static Stack<List<string>> pgnStack;

    private static float whiteTime;
    private static float blackTime;


    public static void Init(float time)
    {
        colorToMoveStack = new Stack<int>();
        vulnerableEnPassantSquareStack = new Stack<int?>();
        castleSquaresStack = new Stack<List<int>>();
        halfMoveClockStack = new Stack<int>();
        fullMoveNumberStack = new Stack<int>();
        pgnStack = new Stack<List<string>>();

        // Load the starting state
        colorToMoveStack.Push(Piece.White);
        castleSquaresStack.Push(new List<int> { 2, 6, 58, 62 });
        vulnerableEnPassantSquareStack.Push(null);
        halfMoveClockStack.Push(0);
        fullMoveNumberStack.Push(1);

        pgnStack.Push(new List<string>());

        whiteTime = time;
        blackTime = time;
    }

    public static int ColorToMove
    {
        get { return colorToMoveStack.Peek(); }
    }

    public static int? VulnerableEnPassantSquare
    {
        get { return vulnerableEnPassantSquareStack.Peek(); }
    }

    public static List<int> CastleSquares
    {
        get { return castleSquaresStack.Peek(); }
    }

    public static int HalfMoveClock
    {
        get { return halfMoveClockStack.Peek(); }
    }

    public static int FullMoveNumber
    {
        get { return fullMoveNumberStack.Peek(); }
    }

    public static List<string> Pgn
    {
        get { return pgnStack.Peek(); }
    }

    public static float WhiteTime
    {
        get { return whiteTime; }
    }

    public static float BlackTime
    {
        get { return blackTime; }
    }

    public static void DecrementWhiteTime(float deltaTime)
    {
        whiteTime -= deltaTime;
    }

    public static void DecrementBlackTime(float deltaTime)
    {
        blackTime -= deltaTime;
    }

    public static void UpdatePgn(Move move)
    {
        List<string> newPgn = new List<string>(Pgn);
        newPgn.Add(PgnUtility.MoveToAlgebraic(move));
        pgnStack.Push(newPgn);
    }

    public static void UnRecordMove()
    {
        colorToMoveStack.Pop();
        vulnerableEnPassantSquareStack.Pop();
        castleSquaresStack.Pop();
        halfMoveClockStack.Pop();
        fullMoveNumberStack.Pop();
    }

    // GameState.RecordMove() must happen before Board.RecordMove() 
    public static void RecordMove(Move move)
    {
        colorToMoveStack.Push(Piece.OppositeColor(ColorToMove));

        int? newVulnerableEnPassantSquare = move.MoveFlag == Move.Flag.PawnTwoForward
        ? (move.StartSquare < move.TargetSquare ? move.StartSquare + 8 : move.StartSquare - 8)
        : null;
        vulnerableEnPassantSquareStack.Push(newVulnerableEnPassantSquare);

        List<int> newCastleSquares = new List<int>(CastleSquares);
        switch (move.StartSquare)
        {
            case 4:  // White king moved
                newCastleSquares.Remove(6);
                newCastleSquares.Remove(2);
                break;
            case 60:  // Black king moved
                newCastleSquares.Remove(62);
                newCastleSquares.Remove(58);
                break;
            case 0:  // a1 rook moved
                newCastleSquares.Remove(2);
                break;
            case 7:  // h1 rook moved
                newCastleSquares.Remove(6);
                break;
            case 56:  // a8 rook moved
                newCastleSquares.Remove(58);
                break;
            case 63:  // h8 rook moved
                newCastleSquares.Remove(62);
                break;
        }
        castleSquaresStack.Push(newCastleSquares);

        int newFullMoveNumber = ColorToMove == Piece.White ? FullMoveNumber + 1 : FullMoveNumber;
        fullMoveNumberStack.Push(newFullMoveNumber);

        bool shouldResetClock = Piece.Type(Board.PieceAt(move.StartSquare)) == Piece.Pawn || Board.PieceAt(move.TargetSquare) != Piece.None;
        halfMoveClockStack.Push(shouldResetClock ? 0 : HalfMoveClock + 1);
    }


    public static string HalfFen()
    {
        string fen = "";

        fen += Piece.IsWhite(ColorToMove) ? "w" : "b";
        fen += " ";

        string castlingRights = "";
        if (CastleSquares.Contains(6)) { castlingRights += "K"; }
        if (CastleSquares.Contains(2)) { castlingRights += "Q"; }
        if (CastleSquares.Contains(62)) { castlingRights += "k"; }
        if (CastleSquares.Contains(58)) { castlingRights += "q"; }
        fen += CastleSquares.Count == 0 ? "-" : castlingRights;
        fen += " ";

        fen += VulnerableEnPassantSquare is int vulnerableSquare
        ? Helpers.SquareToAlgebraic(vulnerableSquare)
        : "-";

        fen += " ";
        fen += HalfMoveClock;

        fen += " ";
        fen += FullMoveNumber;

        return fen;
    }
}