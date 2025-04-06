using System;
using System.Collections.Generic;
using System.Collections;
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
        int depth = 3;  // Must be at least 1
        // Can solve a mate in (depth + 1) // 2

        Search(game.Board, game.GameState, depth, 0, Helpers.NegativeInfinity, Helpers.PositiveInfinity);

        Debug.Log(moveToPlay);

        game.ExecuteMove(moveToPlay);

        Debug.Log("TT size = " + TranspositionTable.Size());
    }

    private int Search(Board board, GameState gameState, int depth, int plyFromRoot, int alpha, int beta)
    {
        List<Move> legalMoves = LegalMoves.GetLegalMoves(board, gameState);

        if (depth == 0 || legalMoves.Count == 0)
        {
            int eval = Evaluate.EvaluatePosition(board, gameState, legalMoves);
            return eval == -Helpers.CheckmateEval ? eval + plyFromRoot : eval;  // plyFromRoot prioritizes mates that happen sooner 
        }

        // Order legalMoves so that better moves are searched first. This improves alpha beta pruning
        MoveOrdering.OrderMoves(legalMoves, board, gameState, depth - 1);

        int bestEvaluation = int.MinValue;
        Move bestMove = invalidMove;

        foreach (Move move in legalMoves)
        {
            // Pretend to make the move
            Board boardCopy = Board.Copy(board);
            GameState gameStateCopy = GameState.Copy(gameState);
            boardCopy.RecordMove(move);
            gameStateCopy.RecordMove(move);


            // Check transposition table for an evaluation
            int? ttEvaluation = TranspositionTable.TryLookupPosition(boardCopy, gameStateCopy, depth - 1);

            int evaluation = ttEvaluation is int eval
            ? evaluation = eval
            : -Search(boardCopy, gameStateCopy, depth - 1, plyFromRoot + 1, -beta, -alpha);

            // Update transposition table if needed
            if (ttEvaluation is null)
            {
                TranspositionTable.StorePosition(boardCopy, gameStateCopy, evaluation, depth - 1);
            }

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