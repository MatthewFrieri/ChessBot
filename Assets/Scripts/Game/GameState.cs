using System.Collections.Generic;
using System.Linq;

static class GameState
{
    private static int colorToMove;
    private static int? vulnerableEnPassantSquare;
    private static List<int> castleSquares = new List<int>();
    private static int halfMoveClock;
    private static int fullMoveNumber;

    public static void Init(string fen)
    {
        LoadFromFEN(fen);
    }

    public static int ColorToMove
    {
        get { return colorToMove; }
    }

    public static int? VulnerableEnPassantSquare
    {
        get { return vulnerableEnPassantSquare; }
    }

    public static List<int> CastleSquares
    {
        get { return castleSquares; }
    }

    private static void ToggleColorToMove()
    {
        colorToMove = Piece.OppositeColor(colorToMove);
    }

    public static void UnRecordMove(Move move)
    {

    }

    public static void RecordMove(Move move)
    {
        ToggleColorToMove();

        // TODO update halfMoveClock 

        if (colorToMove == Piece.White)
        {
            fullMoveNumber += 1;
        }

        if (move.MoveFlag == Move.Flag.PawnTwoForward)
        {
            vulnerableEnPassantSquare = move.StartSquare < move.TargetSquare ? move.StartSquare + 8 : move.StartSquare - 8;
        }
        else
        {
            vulnerableEnPassantSquare = null;
        }


        switch (move.StartSquare)
        {
            case 4:  // White king moved
                castleSquares.Remove(6);
                castleSquares.Remove(2);
                break;
            case 60:  // Black king moved
                castleSquares.Remove(62);
                castleSquares.Remove(58);
                break;
            case 0:  // a1 rook moved
                castleSquares.Remove(2);
                break;
            case 7:  // h1 rook moved
                castleSquares.Remove(6);
                break;
            case 56:  // a8 rook moved
                castleSquares.Remove(58);
                break;
            case 63:  // h8 rook moved
                castleSquares.Remove(62);
                break;
        }


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

        GameState.colorToMove = colorToMove == "w" ? Piece.White : Piece.Black;

        if (castlingRights.Contains("K")) { castleSquares.Add(6); }
        if (castlingRights.Contains("Q")) { castleSquares.Add(2); }
        if (castlingRights.Contains("k")) { castleSquares.Add(62); }
        if (castlingRights.Contains("q")) { castleSquares.Add(58); }

        vulnerableEnPassantSquare = vulnerableEnPassantAlgebraic == "-" ? null : Helpers.AlgebraicToSquare(vulnerableEnPassantAlgebraic);

        GameState.halfMoveClock = int.Parse(halfMoveClock);

        GameState.fullMoveNumber = int.Parse(fullMoveNumber);

    }

    public static string HalfFen()
    {
        string fen = "";

        fen += Piece.IsWhite(colorToMove) ? "w" : "b";
        fen += " ";

        string castlingRights = "";
        if (castleSquares.Contains(6)) { castlingRights += "K"; }
        if (castleSquares.Contains(2)) { castlingRights += "Q"; }
        if (castleSquares.Contains(62)) { castlingRights += "k"; }
        if (castleSquares.Contains(58)) { castlingRights += "q"; }
        fen += castleSquares.Count == 0 ? "-" : castlingRights;
        fen += " ";

        fen += vulnerableEnPassantSquare is int vulnerableSquare
        ? Helpers.SquareToAlgebraic(vulnerableSquare)
        : "-";

        fen += " ";
        fen += halfMoveClock;

        fen += " ";
        fen += fullMoveNumber;

        return fen;
    }
}