using UnityEngine;

static class Bot
{
    private static int color;
    private static Move moveToPlay;

    public static void Init(int color)
    {
        Bot.color = color;
    }

    public static int Color
    {
        get { return color; }
    }

    public static Move MoveToPlay
    {
        set { moveToPlay = value; }
    }

    public static void MakeMove()
    {
        int depth = 5;  // Must be at least 1
        // Can solve a mate in (depth + 1) // 2

        Search.RecursiveSearch(depth, 0, Helpers.NegativeInfinity, Helpers.PositiveInfinity);

        Game.ExecuteMove(moveToPlay);
    }
}