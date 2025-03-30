using System;
using System.Collections.Generic;
using UnityEditor;


public class Bot
{
    private Game game;
    public int color;

    public Bot(Game game, int color)
    {
        this.game = game;
        this.color = color;
    }

    public void MakeMove()
    {
        Move randomMove = FindBestMove();
        game.ExecuteMove(randomMove);
    }

    public Move FindBestMove()
    {
        List<Move> moves = LegalMoves.GetLegalMoves(game.Board, game.GameState);
        Random rand = new Random();
        Move moveToPlay = moves[rand.Next(moves.Count)];  // Randomly selects a legal move
        return moveToPlay;
    }
}
