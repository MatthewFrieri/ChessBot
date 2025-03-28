using Chess;
using System.Collections.Generic;


public static class Bot
{

    public const int botColor;

    public static void PlayTurn()
    {
        List<Move> moves = LegalMoves.GetAllLegalMoves(Board.Squares, botColor);
        Random rand = new Random();
        Move moveToPlay = moves[rand.Next(moves.Count)];  // Randomly selects a legal move
        Board.ExecuteMove(moveToPlay);
    }

}
