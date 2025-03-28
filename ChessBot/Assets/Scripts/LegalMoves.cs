using Chess;
using System.Collections.Generic;

public static class LegalMoves
{

    public static List<Move> GetLegalMoves(int[] squares, int color)
    {

        List<Move> moves = new List<Move>();

        for (int startSquare = 0; startSquare < 64; startSquare += 1)
        {
            int piece = squares[startSquare];

            if (Piece.Color(piece) != color) continue;

            int pieceType = Piece.Type(piece);

            switch (pieceType)
            {
                case Piece.Pawn:
                    moves.AddRange(GetPawnMoves(squares, startSquare, color));
                    break;
                case Piece.Rook:
                    break;
                case Piece.Knight:
                    moves.AddRange(GetKnightMoves(squares, startSquare, color));
                    break;
                case Piece.Bishop:
                    break;
                case Piece.Queen:
                    break;
                case Piece.King:
                    break;

            }
        }

        return moves;

    }

    private static List<Move> GetKnightMoves(int[] squares, int startSquare, int color)
    {

        List<Move> knightMoves = new List<Move>();
        int[] offsets = { -10, -17, -15, -6, 10, 17, 15, 6 };

        foreach (int offset in offsets)
        {
            int targetSquare = startSquare + offset;
            if (targetSquare < 0 || targetSquare >= 64) continue;

            int targetPiece = squares[targetSquare];

            if (Piece.Color(targetPiece) != color)
            {
                knightMoves.Add(new Move(startSquare, targetSquare));
            }
        }

        return knightMoves;
    }


    private static List<Move> GetPawnMoves(int[] squares, int startSquare, int color)
    {

        List<Move> pawnMoves = new List<Move>();
        int colorToCapture = Piece.OppositeColor(color);
        int dir = color == Piece.White ? 1 : -1;
        int initialPawnRank = color == Piece.White ? 1 : 6;

        int forwardSquare = startSquare + 8 * dir;
        int forwardPiece = squares[forwardSquare];
        if (forwardPiece == Piece.None)
        {
            pawnMoves.Add(new Move(startSquare, forwardSquare));

            int doubleForwardSquare = startSquare + 16 * dir;
            if (Board.Rank(startSquare) == initialPawnRank)
            {
                int doubleForwardPiece = squares[doubleForwardSquare];
                if (doubleForwardPiece == Piece.None) pawnMoves.Add(new Move(startSquare, doubleForwardSquare));
            }
        }

        if ((color == Piece.White && Board.File(startSquare) != 0) || (color == Piece.Black && Board.File(startSquare) != 7))
        {
            int diagonalSquare = startSquare + 7 * dir;
            int diagonalPiece = squares[diagonalSquare];
            if (Piece.Color(diagonalPiece) == colorToCapture) pawnMoves.Add(new Move(startSquare, diagonalSquare));
        }

        if ((color == Piece.White && Board.File(startSquare) != 7) || (color == Piece.Black && Board.File(startSquare) != 0))
        {
            int diagonalSquare = startSquare + 9 * dir;
            int diagonalPiece = squares[diagonalSquare];
            if (Piece.Color(diagonalPiece) == colorToCapture) pawnMoves.Add(new Move(startSquare, diagonalSquare));
        }

        return pawnMoves;
    }
}