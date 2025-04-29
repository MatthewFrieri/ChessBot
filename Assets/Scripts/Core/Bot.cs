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

    public static bool CanUseBook
    {
        get { return canUseBook; }
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
                canUseBook = false;
            }
        }


        float timeLeft;
        if (Color == Piece.White)
        {
            timeLeft = GameState.WhiteTime;
        }
        else
        {
            timeLeft = GameState.BlackTime;
        }

        float maxPercent = timeLeft * 0.50f;
        float baseTime = 10.0f;

        float timeToThink = Math.Min(maxPercent, baseTime);


        timeToThink = 180 / 80;


        DateTime endTime = DateTime.Now + TimeSpan.FromMilliseconds(timeToThink * 1000);

        Move moveToPlay = Search.IterativeDeepeningSearch(endTime);
        return moveToPlay;
    }

    private static Move? TryFindMoveFromBook()
    {
        string algebraic = Opening.NextMoveAlgebraic();
        if (algebraic == null) { return null; }

        return PgnUtility.AlgebraicToMove(algebraic);
    }

}