using System.Collections.Generic;
using UnityEngine;

static class Weights
{
    private static int[] pawnWeights = new int[]{
         0,  0,  0,  0,  0,  0,  0,  0,
        50, 50, 50, 50, 50, 50, 50, 50,
        10, 10, 20, 30, 30, 20, 10, 10,
         5,  5, 10, 25, 25, 10,  5,  5,
         0,  0,  0, 20, 20,  0,  0,  0,
         5, -5,-10,  0,  0,-10, -5,  5,
         5, 10, 10,-20,-20, 10, 10,  5,
         0,  0,  0,  0,  0,  0,  0,  0
    };

    private static int[] knightWeights = new int[]{
        -50,-40,-30,-30,-30,-30,-40,-50,
        -40,-20,  0,  0,  0,  0,-20,-40,
        -30,  0, 10, 15, 15, 10,  0,-30,
        -30,  5, 15, 20, 20, 15,  5,-30,
        -30,  0, 15, 20, 20, 15,  0,-30,
        -30,  5, 10, 15, 15, 10,  5,-30,
        -40,-20,  0,  5,  5,  0,-20,-40,
        -50,-40,-30,-30,-30,-30,-40,-50,
    };

    private static int[] bishopWeights = new int[]{
        -20,-10,-10,-10,-10,-10,-10,-20,
        -10,  0,  0,  0,  0,  0,  0,-10,
        -10,  0,  5, 10, 10,  5,  0,-10,
        -10,  5,  5, 10, 10,  5,  5,-10,
        -10,  0, 10, 10, 10, 10,  0,-10,
        -10, 10, 10, 10, 10, 10, 10,-10,
        -10,  5,  0,  0,  0,  0,  5,-10,
        -20,-10,-10,-10,-10,-10,-10,-20,
    };

    private static int[] rookWeights = new int[]{
         0,  0,  0,  0,  0,  0,  0,  0,
         5, 10, 10, 10, 10, 10, 10,  5,
        -5,  0,  0,  0,  0,  0,  0, -5,
        -5,  0,  0,  0,  0,  0,  0, -5,
        -5,  0,  0,  0,  0,  0,  0, -5,
        -5,  0,  0,  0,  0,  0,  0, -5,
        -5,  0,  0,  0,  0,  0,  0, -5,
         0,  0,  0,  5,  5,  0,  0,  0
    };

    private static int[] queenWeights = new int[]{
        -20,-10,-10, -5, -5,-10,-10,-20,
        -10,  0,  0,  0,  0,  0,  0,-10,
        -10,  0,  5,  5,  5,  5,  0,-10,
         -5,  0,  5,  5,  5,  5,  0, -5,
          0,  0,  5,  5,  5,  5,  0, -5,
        -10,  5,  5,  5,  5,  5,  0,-10,
        -10,  0,  5,  0,  0,  0,  0,-10,
        -20,-10,-10, -5, -5,-10,-10,-20
    };

    private static int[] kingWeights = new int[]{  // TODO This is really kingMiddleWeights
        -30,-40,-40,-50,-50,-40,-40,-30,
        -30,-40,-40,-50,-50,-40,-40,-30,
        -30,-40,-40,-50,-50,-40,-40,-30,
        -30,-40,-40,-50,-50,-40,-40,-30,
        -20,-30,-30,-40,-40,-30,-30,-20,
        -10,-20,-20,-20,-20,-20,-20,-10,
         20, 20,  0,  0,  0,  0, 20, 20,
         20, 30, 10,  0,  0, 10, 30, 20
    };

    // private static int[] kingEndWeights = new int[]{
    //     -50,-40,-30,-20,-20,-30,-40,-50,
    //     -30,-20,-10,  0,  0,-10,-20,-30,
    //     -30,-10, 20, 30, 30, 20,-10,-30,
    //     -30,-10, 30, 40, 40, 30,-10,-30,
    //     -30,-10, 30, 40, 40, 30,-10,-30,
    //     -30,-10, 20, 30, 30, 20,-10,-30,
    //     -30,-30,  0,  0,  0,  0,-30,-30,
    //     -50,-30,-30,-30,-30,-30,-30,-50
    // };

    private static Dictionary<int, int[]> pieceTypeToWeightArray = new Dictionary<int, int[]>{
        { Piece.Pawn, pawnWeights},
        { Piece.Knight, knightWeights},
        { Piece.Bishop, bishopWeights},
        { Piece.Rook, rookWeights},
        { Piece.Queen, queenWeights},
        { Piece.King, kingWeights},
    };


    public static int GetPieceSquareWeight(int piece, int square)
    {

        if (Piece.Color(piece) == Piece.White)
        {
            int file = Board.File(square);
            int rank = Board.Rank(square);

            int newRank = 7 - rank;
            square = 8 * newRank + file;
        }

        int[] weights = pieceTypeToWeightArray[Piece.Type(piece)];

        return weights[square];
    }
}