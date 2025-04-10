using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

static class Search
{

    private static int depth;
    private static Move bestMove;
    private static int bestEval;
    private static string bestMoveAlgebraic;
    private static int prevIterBestEval;
    private static string prevIterBestMoveAlgebraic;

    private static Move[][] pvTable;
    private static Move[] pvMoves;

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

    private static void PvInit(int depth)
    {
        // Save the previous PV
        pvMoves = new Move[depth - 1];
        for (int i = 0; i < depth - 1; i++)
        {
            pvMoves[i] = pvTable[0][i];
        }

        // Reset the PV table
        pvTable = new Move[depth][];
        for (int i = 0; i < depth; i++)
        {
            pvTable[i] = new Move[depth]; // Initialize each row
        }
    }


    public static Move IterativeDeepeningSearch(DateTime endTime)
    {
        Search.endTime = endTime;

        depth = 0;
        while (DateTime.Now < endTime)
        {
            depth++;

            PvInit(depth);

            prevIterBestMoveAlgebraic = bestMoveAlgebraic;
            prevIterBestEval = bestEval;

            RecursiveSearch(depth, 0, NegativeInfinity, PositiveInfinity);

            if (bestEval == Evaluate.CheckMateEval) { return bestMove; }

        }

        depth--;
        bestMoveAlgebraic = prevIterBestMoveAlgebraic;
        bestEval = prevIterBestEval;

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
        MoveOrdering.OrderMoves(legalMoves, pvMoves, depth - 1);


        Move iterationBestMove = Move.InvalidMove;
        int iterationBestEval = int.MinValue;

        foreach (Move move in legalMoves)
        {
            // Pretend to make the move
            GameState.RecordMove(move);
            Board.RecordMove(move);

            // Check transposition table for an evaluation
            int? ttEvaluation = TranspositionTable.TryLookupPosition(depth - 1);

            int evaluation = ttEvaluation is int ttEval
            ? ttEval
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


                // Update PV table
                pvTable[plyFromRoot][0] = move;
                for (int i = 1; i < depth; i++)
                {
                    pvTable[plyFromRoot][i] = pvTable[plyFromRoot + 1][i - 1];
                }


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
            bestMove = iterationBestMove;
        }

        return iterationBestEval;
    }
}