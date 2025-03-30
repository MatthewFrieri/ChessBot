using System.Collections.Generic;
using System.Linq;

public class GameState
{
    private int colorToMove;
    private int vulnerableEnPassantSquare;

    public GameState()
    {
        LoadFromFEN(Game.StartingFEN);
    }

    public GameState(string fen)
    {
        LoadFromFEN(fen);
    }


    public int ColorToMove
    {
        get
        {
            return colorToMove;
        }
    }

    public int VulnerableEnPassantSquare
    {
        get
        {
            return vulnerableEnPassantSquare;
        }
    }

    public void RecordMove(Move move)
    {
        colorToMove = Piece.OppositeColor(colorToMove);

        if (move.MoveFlag == Move.Flag.PawnTwoForward)
        {
            vulnerableEnPassantSquare = move.StartSquare < move.TargetSquare ? move.StartSquare + 8 : move.StartSquare - 8;
        }
        else
        {
            vulnerableEnPassantSquare = -1;
        }
    }

    private void LoadFromFEN(string fen)
    {
        string[] elements = fen.Split(" ");

        // Fix a half fen
        if (elements.Length == 5)
        {
            elements = elements.Prepend("").ToArray();
        }

        string activeColor = elements[1];
        string castlingRights = elements[2];   // NEED TO USE
        string vulnerableEnPassantAlgebraic = elements[3];
        string halfMoveClock = elements[4];   // NEED TO USE
        string fullMove = elements[5];   // NEED TO USE

        colorToMove = activeColor == "w" ? Piece.White : Piece.Black;
        vulnerableEnPassantSquare = vulnerableEnPassantAlgebraic == "-" ? -1 : Helpers.AlgebraicToSquare(vulnerableEnPassantAlgebraic);
    }

    public string HalfFEN()
    {
        string fen = "";

        fen += Piece.IsWhite(colorToMove) ? "w" : "b";
        fen += " ";
        fen += "-";  // TEMPORARY
        fen += " ";
        fen += vulnerableEnPassantSquare == -1 ? "-" : Helpers.SquareToAlgebraic(vulnerableEnPassantSquare);
        fen += " ";
        fen += "999";  // TEMPORARY
        fen += " ";
        fen += "999";  // TEMPORARY

        return fen;
    }

    public static GameState Copy(GameState gameState)
    {
        return new GameState(gameState.HalfFEN());
    }

}