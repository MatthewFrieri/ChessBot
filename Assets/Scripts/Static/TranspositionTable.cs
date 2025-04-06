using System.Collections.Generic;

static class TranspositionTable
{

    private static Dictionary<ulong, (int evaluation, int depth)> zobristToEval = new Dictionary<ulong, (int, int)>();

    public static int? TryLookupPosition(int minDepth)
    {
        ulong key = Zobrist.GetZobristHash();
        if (zobristToEval.TryGetValue(key, out var entry))
        {
            if (entry.depth >= minDepth)
            {
                return entry.evaluation;
            }
        }
        return null;
    }

    public static void StorePosition(int evaluation, int depth)
    {
        ulong key = Zobrist.GetZobristHash();
        zobristToEval[key] = (evaluation, depth);
    }


    public static int Size()
    {
        return zobristToEval.Count;
    }
}