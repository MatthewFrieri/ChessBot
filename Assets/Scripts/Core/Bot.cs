using UnityEngine;

static class Bot
{
    private static int color;
    private static Move moveToPlay;
    private static int moveToPlayEval;
    private static string moveToPlayAlgebraic;
    private static int depth;

    private static bool canUseBook;
    private const int PositiveInfinity = 999999;
    private const int NegativeInfinity = -PositiveInfinity;

    public static void Init(int color)
    {
        Bot.color = color;
        canUseBook = true;
    }

    public static int Color
    {
        get { return color; }
    }

    public static Move MoveToPlay
    {
        set { moveToPlay = value; }
    }

    public static int MoveToPlayEval
    {
        get { return moveToPlayEval; }
        set { moveToPlayEval = value; }
    }

    public static string MoveToPlayAlgebraic
    {
        get { return moveToPlayAlgebraic; }
        set { moveToPlayAlgebraic = value; }
    }

    public static int Depth
    {
        get { return depth; }
    }

    public static void MakeMove()
    {

        if (canUseBook)
        {
            Move? move = TryFindMoveFromBook();
            if (move is Move bookMove)
            {
                Game.ExecuteMove(bookMove);
                return;
            }
            else
            {
                Debug.Log("found nothing :(");
                canUseBook = false;
            }
        }


        depth = 5;  // Must be at least 1
        // Can solve a mate in (depth + 1) // 2

        Search.RecursiveSearch(depth, 0, NegativeInfinity, PositiveInfinity);

        Game.ExecuteMove(moveToPlay);
    }

    private static Move? TryFindMoveFromBook()
    {

        Debug.Log("Trying to find move from book");
        string algebraic = Opening.NextMoveAlgebraic();
        if (algebraic == null) { return null; }

        Debug.Log("Found: " + algebraic);

        return PgnUtility.AlgebraicToMove(algebraic);
    }

}