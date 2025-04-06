using System;
using System.Collections.Generic;
using UnityEngine;

static class MoveOrdering
{

    private const int captureBonus = 1000000000;
    private const int ttBonus = 100000;


    public static void OrderMoves(List<Move> moves, int depth)
    {
        Dictionary<Move, int> moveToPriority = new Dictionary<Move, int>();

        foreach (Move move in moves)
        {
            int priotiy = GetPriority(move, depth);

            moveToPriority[move] = priotiy;
        }

        moves.Sort((moveA, moveB) => moveToPriority[moveB] - moveToPriority[moveA]);
    }

    private static int GetPriority(Move move, int depth)
    {
        // Assign higher priority to low value pieces capturing high value pieces
        int targetPiece = Board.PieceAt(move.TargetSquare);

        if (targetPiece != Piece.None)
        {
            int friendlyPiece = Board.PieceAt(move.StartSquare);
            int valueDifference = Evaluate.Value(targetPiece) - Evaluate.Value(friendlyPiece);
            return captureBonus + valueDifference;
        }

        // Pretend to make the move
        Board.RecordMove(move);
        GameState.RecordMove(move);

        // Assign higher priority to positions already in the transposition table
        if (TranspositionTable.TryLookupPosition(depth) is int evaluation)
        {
            // Undo the pretend move
            Board.UnRecordMove();
            GameState.UnRecordMove();

            return ttBonus + evaluation;
        }

        // Undo the pretend move
        Board.UnRecordMove();
        GameState.UnRecordMove();

        return 0;  // Low priority move
    }


}