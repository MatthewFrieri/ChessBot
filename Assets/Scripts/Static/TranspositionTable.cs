using System.Collections.Generic;

static class TranspositionTable
{

    private static Dictionary<ulong, (int evaluation, int depth)> zobristToEval = new Dictionary<ulong, (int, int)>();

    public static int? TryLookupPosition(Board board, GameState gameState, int depth)
    {

        ulong key = Zobrist.GetZobristHash(board, gameState);
        if (zobristToEval.TryGetValue(key, out var entry))
        {
            if (entry.depth >= depth)
            {
                return entry.evaluation;
            }
        }
        return null;
    }

    public static void StorePosition(Board board, GameState gameState, int evaluation, int depth)
    {
        ulong key = Zobrist.GetZobristHash(board, gameState);
        zobristToEval[key] = (evaluation, depth);
    }


    public static int Size()
    {
        return zobristToEval.Count;
    }
}