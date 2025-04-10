using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

static class Search
{

    private static int depth;
    private static int bestEval;
    private static Move bestMove;
    private static string bestMoveAlgebraic;

    private static DateTime endTime;

    private const int PositiveInfinity = 999999;
    private const int NegativeInfinity = -PositiveInfinity;
    private const int RanOutOfTime = -1;

    public static int Depth
    {
        get { return depth; }
    }

    public static string BestMoveAlgebraic
    {
        get { return bestMoveAlgebraic; }
    }

    public static int BestEval
    {
        get { return bestEval; }
    }


    public static Move IterativeDeepeningSearch(DateTime endTime)
    {
        Search.endTime = endTime;

        depth = 0;
        while (DateTime.Now < endTime)
        {
            depth++;
            RecursiveSearch(depth, 0, NegativeInfinity, PositiveInfinity);

            if (bestEval == Evaluate.CheckMateEval) { return bestMove; }

        }

        depth--;

        return bestMove;
    }

    public static int RecursiveSearch(int depth, int plyFromRoot, int alpha, int beta)
    {

        if (DateTime.Now > endTime) { return RanOutOfTime; }


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


        Move iterationBestMove = Move.InvalidMove;
        int iterationBestEval = int.MinValue;

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

            if (DateTime.Now > endTime) { return RanOutOfTime; }

            // Remember the best move and evaluation
            if (evaluation > iterationBestEval)
            {
                iterationBestMove = move;
                iterationBestEval = evaluation;

                // Save the best moves of the iteration to display on the game
                if (plyFromRoot == 0)
                {
                    bestEval = iterationBestEval;
                    bestMoveAlgebraic = PgnUtility.MoveToAlgebraic(iterationBestMove);
                }
            }

            alpha = Math.Max(alpha, iterationBestEval);
            if (alpha >= beta) { break; }  // Prune branch
        }

        if (plyFromRoot == 0)
        {
            Debug.Log("Finished depth=" + depth);
            bestMove = iterationBestMove;
        }

        return iterationBestEval;
    }
}