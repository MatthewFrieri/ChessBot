using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

static class Evaluate
{
    private const int checkmateEval = 10000;
    private static Dictionary<int, int> pieceTypeToValue = new Dictionary<int, int>{
        { 1, 100 },
        { 2, 500 },
        { 3, 300 },
        { 4, 320 },
        { 5, 900 },
        { 6, 0 }
    };

    public static int CheckMateEval
    {
        get { return checkmateEval; }
    }

    public static int Value(int piece)
    {
        return pieceTypeToValue[Piece.Type(piece)];
    }

    public static int EvaluatePosition(int legalMovesCount)
    {
        if (IsCheckmate(legalMovesCount)) { return -checkmateEval; }
        if (IsDraw(legalMovesCount)) { return 0; }

        int perspective = GameState.ColorToMove == Piece.White ? 1 : -1;

        int whiteValue = GetMaterialValue(Piece.White) + GetWeightValue(Piece.White);
        int blackValue = GetMaterialValue(Piece.Black) + GetWeightValue(Piece.Black);

        return (whiteValue - blackValue) * perspective;
    }

    public static bool IsCheckmate(int legalMovesCount)
    {
        if (legalMovesCount == 0)
        {
            int friendlyKingSquare = LegalMoves.FindFriendlyKingSquare();

            if (LegalMoves.IsSquareUnderAttack(friendlyKingSquare, Piece.OppositeColor(GameState.ColorToMove)))
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsDraw(int legalMovesCount)
    {
        if (legalMovesCount == 0)
        {
            int friendlyKingSquare = LegalMoves.FindFriendlyKingSquare();

            if (!LegalMoves.IsSquareUnderAttack(friendlyKingSquare, Piece.OppositeColor(GameState.ColorToMove)))
            {
                return true;  // Draw by stalemate
            }
        }

        if (GameState.HalfMoveClock >= 50)
        {
            return true;  // Draw by 50 move rule
        }

        if (Board.Has3MoveRepetitioned())
        {
            return true;  // Draw by 3 move repetition
        }

        return false;
    }


    private static int GetMaterialValue(int color)
    {
        int totalValue = 0;

        for (int i = 0; i < 64; i++)
        {
            int piece = Board.PieceAt(i);
            if (Piece.Color(piece) == color)
            {
                totalValue += Value(piece);
            }
        }

        return totalValue;
    }

    private static int GetWeightValue(int color)
    {
        int totalValue = 0;

        for (int i = 0; i < 64; i++)
        {
            int piece = Board.PieceAt(i);
            if (Piece.Color(piece) == color)
            {
                totalValue += Weights.GetPieceSquareWeight(piece, i);
            }
        }

        return totalValue;
    }
}
