using System;
using UnityEngine;

static class Bot
{
    private static bool isThinking;
    private static int color;
    private static bool canUseBook;

    public static void Init(int color)
    {
        Bot.color = color;
        canUseBook = true;
    }

    public static bool IsThinking
    {
        get { return isThinking; }
        set { isThinking = value; }
    }

    public static int Color
    {
        get { return color; }
    }

    public static Move Think()
    {

        if (canUseBook)
        {
            Move? move = TryFindMoveFromBook();
            if (move is Move bookMove)
            {
                return bookMove;
            }
            else
            {
                Debug.Log("found nothing :(");
                canUseBook = false;
            }
        }


        DateTime endTime = DateTime.Now + TimeSpan.FromMilliseconds(20000);

        Move moveToPlay = Search.IterativeDeepeningSearch(endTime);
        return moveToPlay;
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