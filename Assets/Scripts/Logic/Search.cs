using System;
using System.Collections.Generic;
using UnityEngine;

static class Search
{

    private static int depth;
    private static Move bestMove;
    private static int bestEval;
    private static string bestMoveAlgebraic;
    private static int prevDepthBestEval;
    private static string prevDepthBestMoveAlgebraic;

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
        bestEval = NegativeInfinity;
        bestMove = Move.InvalidMove;

        depth = 0;
        while (DateTime.Now < endTime)
        {
            depth++;

            PvInit(depth);

            prevDepthBestMoveAlgebraic = bestMoveAlgebraic;
            prevDepthBestEval = bestEval;

            // Debug.Log("Starting at depth=" + depth);
            // Debug.Log("bestMove: " + bestMove);
            // Debug.Log("bestEval: " + bestEval);

            RecursiveSearch(depth, 0, NegativeInfinity, PositiveInfinity);
            // Debug.Log("Done");

            if (bestEval == Evaluate.CheckMateEval)
            {
                // Debug.Log("bestMove: " + bestMove);
                // Debug.Log("bestEval: " + bestEval);
                return bestMove;
            }

        }

        depth--;
        bestMoveAlgebraic = prevDepthBestMoveAlgebraic;
        bestEval = prevDepthBestEval;



        return bestMove;
    }

    public static int RecursiveSearch(int depth, int plyFromRoot, int alpha, int beta)
    {

        if (DateTime.Now > endTime) { return RanOutOfTime; }


        List<Move> legalMoves = LegalMoves.GetLegalMoves();


        if (depth == 0)
        {
            return Evaluate.EvaluatePosition(legalMoves.Count);
        }
        if (legalMoves.Count == 0)
        {
            return Evaluate.EvaluatePosition(legalMoves.Count) + plyFromRoot;  // plyFromRoot prioritizes mates that happen sooner 
        }

        // Order legalMoves so that better moves are searched first. This improves alpha beta pruning
        MoveOrdering.OrderMoves(legalMoves, pvMoves, depth - 1);



        Move iterationBestMove = Move.InvalidMove;
        int iterationBestEval = NegativeInfinity;

        foreach (Move move in legalMoves)
        {
            // Pretend to make the move
            GameState.RecordMove(move);
            Board.RecordMove(move);

            // Check transposition table for an evaluation
            int evaluation;
            int? ttEvaluation = TranspositionTable.TryLookupPosition(depth - 1);


            if (ttEvaluation is int ttEval)  // Successful lookup in TT
            {
                evaluation = -ttEval;
            }
            else  // Could not find position in TT
            {
                int childEval = RecursiveSearch(depth - 1, plyFromRoot + 1, -beta, -alpha);
                evaluation = -childEval;

                // if (childEval == Evaluate.CheckMateEval)
                // {
                //     Debug.Log("+ " + (GameState.ColorToMove == Piece.White ? "White to move " : "Black to move ") + string.Join(" ", GameState.Pgn.GetRange(98, GameState.Pgn.Count - 98)));
                // }

                // if (childEval == -Evaluate.CheckMateEval)
                // {
                //     Debug.Log("- " + (GameState.ColorToMove == Piece.White ? "White to move " : "Black to move ") + string.Join(" ", GameState.Pgn.GetRange(98, GameState.Pgn.Count - 98)));
                // }

                TranspositionTable.StorePosition(childEval, depth - 1);
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
            if (alpha > beta) { break; }  // Prune branch
        }


        if (plyFromRoot == 0)
        {
            bestMove = iterationBestMove;
        }

        return iterationBestEval;
    }
}