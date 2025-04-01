using System;
using System.Collections.Generic;
using UnityEngine;


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
        List<Move> legalMoves = LegalMoves.GetLegalMoves(game.Board, game.GameState);
        int depth = 2;

        int bestEvaluation = int.MinValue;
        Move bestMove = legalMoves[0];


        foreach (Move move in legalMoves)
        {
            Board boardCopy = Board.Copy(game.Board);
            GameState gameStateCopy = GameState.Copy(game.GameState);

            boardCopy.RecordMove(move);
            gameStateCopy.RecordMove(move);

            int evaluation = -Search(depth, boardCopy, gameStateCopy);

            if (evaluation > bestEvaluation)
            {
                bestEvaluation = evaluation;
                bestMove = move;
            }
        }

        game.ExecuteMove(bestMove);
    }

    private int Search(int depth, Board board, GameState gameState)
    {
        if (depth == 0)
        {
            return Evaluate.EvaluatePosition(board, gameState);
        }

        int bestEvaluation = int.MinValue;

        List<Move> legalMoves = LegalMoves.GetLegalMoves(board, gameState);

        foreach (Move move in legalMoves)
        {
            Board boardCopy = Board.Copy(board);
            GameState gameStateCopy = GameState.Copy(gameState);

            boardCopy.RecordMove(move);
            gameStateCopy.RecordMove(move);

            int evaluation = -Search(depth - 1, boardCopy, gameStateCopy);

            bestEvaluation = Math.Max(bestEvaluation, evaluation);
        }
        return bestEvaluation;
    }

}