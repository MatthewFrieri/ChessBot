using UnityEngine;

static class Helpers
{

    public static string SquareToAlgebraic(int square)
    {
        string file = ((char)('a' + Board.File(square))).ToString();
        string rank = Board.Rank(square).ToString();

        return file + rank;
    }

    public static int AlgebraicToSquare(string algebraic)
    {
        int file = algebraic[0] - 'a';
        int rank = algebraic[1];

        return 8 * rank + file;
    }

    public static Vector2 SquareToLocation(int square)
    {
        return new Vector2(Board.File(square) + 0.5f, Board.Rank(square) + 0.5f);
    }

    public static int LocationToSquare(Vector2 location)
    {
        int file = (int)location.x;
        int rank = (int)location.y;

        return 8 * rank + file;
    }
}