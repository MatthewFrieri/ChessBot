using System;
using System.Collections.Generic;
using UnityEngine;


public class Bot
{
    private Game game;
    public int color;
    private Move invalidMove = new Move(0, 0);
    private Move moveToPlay;

    public Bot(Game game, int color)
    {
        this.game = game;
        this.color = color;
    }

    public void MakeMove()
    {
        int depth = 5;  // Must be at least 1
        // Can solve a mate in (depth + 1) // 2

        Search(game.Board, game.GameState, depth, 0, Helpers.NegativeInfinity, Helpers.PositiveInfinity);

        Debug.Log(moveToPlay);

        game.ExecuteMove(moveToPlay);

    }

    private int Search(Board board, GameState gameState, int depth, int plyFromRoot, int alpha, int beta)
    {
        List<Move> legalMoves = LegalMoves.GetLegalMoves(board, gameState);

        if (depth == 0 || legalMoves.Count == 0)
        {
            int eval = Evaluate.EvaluatePosition(board, gameState, legalMoves);
            return eval == -Helpers.CheckmateEval ? eval + plyFromRoot : eval;  // plyFromRoot prioritizes mates that happen sooner 
        }

        int bestEvaluation = int.MinValue;
        Move bestMove = invalidMove;

        foreach (Move move in legalMoves)
        {
            // Pretend to make the move
            Board boardCopy = Board.Copy(board);
            GameState gameStateCopy = GameState.Copy(gameState);
            boardCopy.RecordMove(move);
            gameStateCopy.RecordMove(move);

            int evaluation = -Search(boardCopy, gameStateCopy, depth - 1, plyFromRoot + 1, -beta, -alpha);

            if (evaluation > bestEvaluation)
            {
                bestEvaluation = evaluation;
                bestMove = move;
            }

            alpha = Math.Max(alpha, bestEvaluation);
            if (alpha >= beta) { break; }  // Prune branch
        }

        if (depth == 5)
        {
            Debug.Log(bestEvaluation);
        }

        moveToPlay = bestMove;
        return bestEvaluation;
    }

}