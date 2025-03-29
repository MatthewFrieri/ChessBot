using System.Collections.Generic;

static class Evaluate
{
    private static Dictionary<int, int> pieceTypeToValue = new Dictionary<int, int>{
        { 1, 100 },
        { 2, 500 },
        { 3, 300 },
        { 4, 320 },
        { 5, 900 },
        { 6, 0 }
    };

    public static int EvaluatePosition(int[] squares)
    {
        int whiteValue = GetMaterialValue(squares, Piece.White);
        int blackValue = GetMaterialValue(squares, Piece.Black);

        return whiteValue - blackValue;
    }

    private static int GetMaterialValue(int[] squares, int color)
    {
        int totalValue = 0;

        foreach (int piece in squares)
        {
            if (Piece.Color(piece) == color)
            {
                totalValue += pieceTypeToValue[Piece.Type(piece)];
            }
        }

        return totalValue;
    }
}
