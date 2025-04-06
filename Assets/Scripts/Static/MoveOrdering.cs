using System.Collections.Generic;

static class MoveOrdering
{

    private const int ttBonus = 100000;

    public static void OrderMoves(List<Move> moves, Board board, GameState gameState, int depth)
    {
        Dictionary<Move, int> moveToPriority = new Dictionary<Move, int>();

        foreach (Move move in moves)
        {
            int priotiy = GetPriority(move, board, gameState, depth);

            moveToPriority[move] = priotiy;
        }

        moves.Sort((moveA, moveB) => moveToPriority[moveA] - moveToPriority[moveB]);
    }

    private static int GetPriority(Move move, Board board, GameState gameState, int depth)
    {
        // Pretend to make the move
        Board boardCopy = Board.Copy(board);
        GameState gameStateCopy = GameState.Copy(gameState);
        boardCopy.RecordMove(move);
        gameStateCopy.RecordMove(move);

        if (TranspositionTable.TryLookupPosition(boardCopy, gameStateCopy, depth) is int evaluation)
        {
            return ttBonus + evaluation;
        }

        return 0;
    }


}