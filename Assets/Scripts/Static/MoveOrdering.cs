using System.Collections.Generic;

static class MoveOrdering
{

    private const int captureBonus = 1000000000;
    private const int ttBonus = 100000;


    public static void OrderMoves(List<Move> moves, Board board, GameState gameState, int depth)
    {
        Dictionary<Move, int> moveToPriority = new Dictionary<Move, int>();

        foreach (Move move in moves)
        {
            int priotiy = GetPriority(move, board, gameState, depth);

            moveToPriority[move] = priotiy;
        }

        moves.Sort((moveA, moveB) => moveToPriority[moveB] - moveToPriority[moveA]);
    }



    private static int GetPriority(Move move, Board board, GameState gameState, int depth)
    {
        // Pretend to make the move
        Board boardCopy = Board.Copy(board);
        GameState gameStateCopy = GameState.Copy(gameState);
        boardCopy.RecordMove(move);
        gameStateCopy.RecordMove(move);

        // Assign higher priority to low value pieces capturing high value pieces
        int targetPiece = board.PieceAt(move.TargetSquare);
        if (targetPiece != Piece.None)
        {
            int friendlyPiece = board.PieceAt(move.StartSquare);
            int valueDifference = Evaluate.Value(targetPiece) - Evaluate.Value(friendlyPiece);
            return captureBonus + valueDifference;
        }

        // Assign higher priority to positions already in the transposition table
        if (TranspositionTable.TryLookupPosition(boardCopy, gameStateCopy, depth) is int evaluation)
        {
            return ttBonus + evaluation;
        }

        return 0;
    }


}