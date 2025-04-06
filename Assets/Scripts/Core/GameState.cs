using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

static class GameState
{
    private static Stack<int> colorToMoveStack;
    private static Stack<int?> vulnerableEnPassantSquareStack;
    private static Stack<List<int>> castleSquaresStack;
    private static Stack<int> halfMoveClockStack;
    private static Stack<int> fullMoveNumberStack;

    public static void Init(string fen)
    {
        colorToMoveStack = new Stack<int>();
        vulnerableEnPassantSquareStack = new Stack<int?>();
        castleSquaresStack = new Stack<List<int>>();
        halfMoveClockStack = new Stack<int>();
        fullMoveNumberStack = new Stack<int>();
        LoadFromFEN(fen);
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

        int newFullMoveNumber = ColorToMove == Piece.White ? FullMoveNumber + 1 : FullMoveNumber;
        fullMoveNumberStack.Push(newFullMoveNumber);

        bool shouldResetClock = Piece.Type(Board.PieceAt(move.StartSquare)) == Piece.Pawn || Board.PieceAt(move.TargetSquare) != Piece.None;
        halfMoveClockStack.Push(shouldResetClock ? 0 : HalfMoveClock + 1);

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

    }

    private static void LoadFromFEN(string fen)
    {
        string[] elements = fen.Split(" ");

        // Fix a half fen
        if (elements.Length == 5)
        {
            elements = elements.Prepend("").ToArray();
        }

        string colorToMove = elements[1];
        string castlingRights = elements[2];
        string vulnerableEnPassantAlgebraic = elements[3];
        string halfMoveClock = elements[4];   // NEED TO USE
        string fullMoveNumber = elements[5];   // NEED TO USE

        colorToMoveStack.Push(colorToMove == "w" ? Piece.White : Piece.Black);

        List<int> castleSquares = new List<int>();
        if (castlingRights.Contains("K")) { castleSquares.Add(6); }
        if (castlingRights.Contains("Q")) { castleSquares.Add(2); }
        if (castlingRights.Contains("k")) { castleSquares.Add(62); }
        if (castlingRights.Contains("q")) { castleSquares.Add(58); }
        castleSquaresStack.Push(castleSquares);

        vulnerableEnPassantSquareStack.Push(vulnerableEnPassantAlgebraic == "-" ? null : Helpers.AlgebraicToSquare(vulnerableEnPassantAlgebraic));

        halfMoveClockStack.Push(int.Parse(halfMoveClock));

        fullMoveNumberStack.Push(int.Parse(fullMoveNumber));

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