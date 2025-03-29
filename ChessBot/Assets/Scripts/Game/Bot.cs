using System;
using System.Collections.Generic;


public class Bot
{
    public int color;

    public Bot(int color)
    {
        this.color = color;
    }

    public Move FindBestMove(Board board, GameState gameState)
    {
        List<Move> moves = LegalMoves.GetLegalMoves(board, gameState);
        Random rand = new Random();
        Move moveToPlay = moves[rand.Next(moves.Count)];  // Randomly selects a legal move
        return moveToPlay;
    }
}
