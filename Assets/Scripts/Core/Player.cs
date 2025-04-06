using System.Collections.Generic;

static class Player
{
    private static int color;
    private static List<Move> currentLegalMoves;

    public static void Init(int color)
    {
        Player.color = color;
    }

    public static int Color
    {
        get { return color; }
    }

    public static List<Move> CurrentLegalMoves
    {
        set { currentLegalMoves = value; }
    }

    public static void MakeMove(int startSquare, int targetSquare)
    {
        // TODO Need a way of deciding how to promote
        // Right now auto promotes to queen
        foreach (Move move in currentLegalMoves)
        {
            if (move.StartSquare == startSquare && move.TargetSquare == targetSquare)
            {
                Game.ExecuteMove(move);
                break;
            }
        }

    }

    public static List<int> GetLegalTargetSquares(int startSquare)
    {
        List<int> targetSquares = new List<int>();

        foreach (Move move in currentLegalMoves)
        {
            if (move.StartSquare == startSquare)
            {
                targetSquares.Add(move.TargetSquare);
            }
        }

        return targetSquares;
    }

}