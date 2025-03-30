using System.Collections.Generic;

public class Player
{
    private Game game;
    public readonly int color;
    private List<Move> currentLegalMoves;

    public Player(Game game, int color)
    {
        this.game = game;
        this.color = color;
    }

    public List<Move> CurrentLegalMoves
    {
        set
        {
            currentLegalMoves = value;
        }
    }

    public void MakeMove(int startSquare, int targetSquare)
    {
        // TODO Need a way of deciding how to promote
        // Right now auto promotes to queen
        foreach (Move move in currentLegalMoves)
        {
            if (move.StartSquare == startSquare && move.TargetSquare == targetSquare)
            {
                game.ExecuteMove(move);
                break;
            }
        }

    }

    public List<int> GetLegalTargetSquares(int startSquare)
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