using UnityEngine;

static class Helpers
{

    public static Vector2 SquareToLocation(int square)
    {
        return new Vector2(Board.File(square) + 0.5f, Board.Rank(square) + 0.5f);
    }


}