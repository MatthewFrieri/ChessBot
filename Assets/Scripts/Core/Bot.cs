using UnityEngine;

static class Bot
{
    private static int color;
    private static Move moveToPlay;

    private const int PositiveInfinity = 999999;
    private const int NegativeInfinity = -PositiveInfinity;

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
        int depth = 3;  // Must be at least 1
        // Can solve a mate in (depth + 1) // 2

        Search.RecursiveSearch(depth, 0, NegativeInfinity, PositiveInfinity);

        Game.ExecuteMove(moveToPlay);
    }
}