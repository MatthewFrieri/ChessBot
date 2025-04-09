using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

static class Evaluate
{
    private const int CheckmateEval = 10000;
    private static Dictionary<int, int> pieceTypeToValue = new Dictionary<int, int>{
        { 1, 100 },
        { 2, 500 },
        { 3, 300 },
        { 4, 320 },
        { 5, 900 },
        { 6, 0 }
    };

    public static int Value(int piece)
    {
        return pieceTypeToValue[Piece.Type(piece)];
    }

    public static int EvaluatePosition(List<Move> legalMoves)
    {
        if (legalMoves.Count == 0)
        {
            int friendlyKingSquare = LegalMoves.FindFriendlyKingSquare();

            if (LegalMoves.IsSquareUnderAttack(friendlyKingSquare, Piece.OppositeColor(GameState.ColorToMove)))
            {
                return -CheckmateEval;  // Checkmate
            }
            return 0;  // Draw
        }

        int perspective = GameState.ColorToMove == Piece.White ? 1 : -1;


        int whiteValue = GetMaterialValue(Piece.White) + GetWeightValue(Piece.White);
        int blackValue = GetMaterialValue(Piece.Black) + GetWeightValue(Piece.Black);

        return (whiteValue - blackValue) * perspective;
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
