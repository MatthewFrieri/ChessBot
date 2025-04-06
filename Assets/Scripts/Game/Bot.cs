using System;
using System.Collections.Generic;
using UnityEngine;


static class Bot
{
    private static int color;
    private static Move invalidMove = new Move(0, 0);
    private static Move moveToPlay;

    public static void Init(int color)
    {
        Bot.color = color;
    }

    public static int Color
    {
        get { return color; }
    }


    public static void MakeMove()
    {
        int depth = 5;  // Must be at least 1
        // Can solve a mate in (depth + 1) // 2

        Search(depth, 0, Helpers.NegativeInfinity, Helpers.PositiveInfinity);

        Game.ExecuteMove(moveToPlay);
    }

    private static int Search(int depth, int plyFromRoot, int alpha, int beta)
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
        Move bestMove = invalidMove;

        foreach (Move move in legalMoves)
        {
            // Pretend to make the move
            Board.RecordMove(move);
            GameState.RecordMove(move);

            // Check transposition table for an evaluation
            int? ttEvaluation = TranspositionTable.TryLookupPosition(depth - 1);

            int evaluation = ttEvaluation is int eval
            ? evaluation = eval
            : -Search(depth - 1, plyFromRoot + 1, -beta, -alpha);

            // Update transposition table if needed
            if (ttEvaluation is null)
            {
                TranspositionTable.StorePosition(evaluation, depth - 1);
            }

            // Undo the pretend move
            Board.UnRecordMove();
            GameState.UnRecordMove();

            // Remember the best move and evaluation
            if (evaluation > bestEvaluation)
            {
                bestEvaluation = evaluation;
                bestMove = move;
            }

            alpha = Math.Max(alpha, bestEvaluation);
            if (alpha >= beta) { break; }  // Prune branch
        }

        moveToPlay = bestMove;
        return bestEvaluation;
    }

}