using System;
using System.Collections.Generic;
using UnityEngine;

static class Search
{

    public static int RecursiveSearch(int depth, int plyFromRoot, int alpha, int beta)
    {
        List<Move> legalMoves = LegalMoves.GetLegalMoves();

        if (depth == 0)
        {
            return Evaluate.EvaluatePosition(legalMoves);
        }
        if (legalMoves.Count == 0)
        {
            return Evaluate.EvaluatePosition(legalMoves) + plyFromRoot;  // plyFromRoot prioritizes mates that happen sooner 
        }

        // Order legalMoves so that better moves are searched first. This improves alpha beta pruning
        MoveOrdering.OrderMoves(legalMoves, depth - 1);


        int bestEvaluation = int.MinValue;
        Move bestMove = Move.InvalidMove;

        foreach (Move move in legalMoves)
        {
            // Pretend to make the move
            GameState.RecordMove(move);
            Board.RecordMove(move);

            // Check transposition table for an evaluation
            int? ttEvaluation = TranspositionTable.TryLookupPosition(depth - 1);

            int evaluation = ttEvaluation is int eval
            ? evaluation = eval
            : -RecursiveSearch(depth - 1, plyFromRoot + 1, -beta, -alpha);

            // Update transposition table if needed
            if (ttEvaluation is null)
            {
                TranspositionTable.StorePosition(evaluation, depth - 1);
            }

            // Undo the pretend move
            GameState.UnRecordMove();
            Board.UnRecordMove();

            // Remember the best move and evaluation
            if (evaluation > bestEvaluation)
            {
                bestMove = move;
                bestEvaluation = evaluation;

                if (plyFromRoot == 0)
                {
                    Bot.MoveToPlay = bestMove;
                    Bot.MoveToPlayEval = bestEvaluation;
                    Bot.MoveToPlayAlgebraic = PgnUtility.MoveToAlgebraic(bestMove);
                }
            }

            alpha = Math.Max(alpha, bestEvaluation);
            if (alpha >= beta) { break; }  // Prune branch
        }

        return bestEvaluation;
    }
}