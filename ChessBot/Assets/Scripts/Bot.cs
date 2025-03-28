using Chess;
using System;
using System.Collections.Generic;


public static class Bot
{

    public static int botColor;

    public static void PlayTurn()
    {
        List<Move> moves = LegalMoves.GetLegalMoves(Board.Squares, botColor);
        Random rand = new Random();
        Move moveToPlay = moves[rand.Next(moves.Count)];  // Randomly selects a legal move
        Board.ExecuteMove(moveToPlay);
    }

}
