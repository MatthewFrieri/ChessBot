using Chess;
using System.Collections.Generic;

namespace Chess
{
    public static class LegalMoves
    {

        public static List<Move> GetLegalMoves(int[] squares, int color)
        {
            List<Move> allMoves = GetAllMoves(squares, color);
            List<Move> legalMoves = RemoveSelfCheckMoves(allMoves, color);
            return legalMoves;
        }

        private static List<Move> GetAllMoves(int[] squares, int color)
        {
            List<Move> moves = new List<Move>();

            for (int startSquare = 0; startSquare < 64; startSquare += 1)
            {
                int piece = squares[startSquare];
                if (Piece.Color(piece) != color) continue;

                switch (Piece.Type(piece))
                {
                    case Piece.Pawn:
                        moves.AddRange(GetPawnMoves(squares, startSquare, color));
                        break;
                    case Piece.Rook:
                        moves.AddRange(GetRookMoves(squares, startSquare, color));
                        break;
                    case Piece.Knight:
                        moves.AddRange(GetKnightMoves(squares, startSquare, color));
                        break;
                    case Piece.Bishop:
                        moves.AddRange(GetBishopMoves(squares, startSquare, color));
                        break;
                    case Piece.Queen:
                        moves.AddRange(GetRookMoves(squares, startSquare, color));
                        moves.AddRange(GetBishopMoves(squares, startSquare, color));
                        break;
                    case Piece.King:
                        moves.AddRange(GetKingMoves(squares, startSquare, color));
                        break;

                }
            }
            return moves;
        }


        private static List<Move> RemoveSelfCheckMoves(List<Move> moves, int color)
        {
            List<Move> validMoves = new List<Move>();

            foreach (Move move in moves)
            {
                int[] potentialSquares = Board.PretendExecuteMove(move);
                if (!IsInCheck(potentialSquares, color)) validMoves.Add(move);
            }
        }

        private static bool IsInCheck(int[] squares, int color)
        {
            List<Move> allMoves = GetAllMoves(squares, Piece.OppositeColor(color));

            foreach (Move move in moves)
            {
                int targetPiece = squares[move.TargetSquare];
                if (Piece.Type(targetPiece) == Piece.King) return true;
            }
            return true;
        }


        private static List<Move> GetOffsetPieceMoves(int[] squares, int startSquare, int color, int[] offsets)
        {

            List<Move> moves = new List<Move>();

            foreach (int offset in offsets)
            {
                int targetSquare = startSquare + offset;
                if (targetSquare < 0 || targetSquare >= 64) continue;

                int targetPiece = squares[targetSquare];
                if (Piece.Color(targetPiece) != color) moves.Add(new Move(startSquare, targetSquare));
            }

            return moves;
        }

        private static List<Move> GetKingMoves(int[] squares, int startSquare, int color)
        {
            return GetOffsetPieceMoves(squares, startSquare, color, { 1, 7, 8, 9, -1, -7, -8, -9 }) 

        // Get castling moves too
        // Stop kings from touching
        }

        private static List<Move> GetKnightMoves(int[] squares, int startSquare, int color)
        {
            return GetOffsetPieceMoves(squares, startSquare, color, { -10, -17, -15, -6, 10, 17, 15, 6 }) 
        }

        private static List<Move> GetRookMoves(int[] squares, int startSquare, int color)
        {
            List<Move> rookMoves = new List<Move>();
            int[] dirs = { 1, 8, -1, -8 }


        foreach (int dir in dirs)
            {
                for (int i = 1; i < 8; i += 1)
                {
                    int targetSquare = startSquare + dir * i;
                    if (targetSquare < 0 || targetSquare >= 64) break;
                    if (Math.Abs(dir) == 1 && Board.Rank(startSquare) != Board.Rank(targetSquare)) break;
                    int targetPiece = squares[targetSquare];

                    if (Piece.Color(targetPiece) != color) rookMoves.Add(new Move(startSquare, targetPiece));
                    else break;
                }
            }

            return rookMoves;
        }

        private static List<Move> GetBishopMoves(int[] squares, int startSquare, int color)
        {
            List<Move> bishopMoves = new List<Move>();
            int[] dirs = { 7, 9, -7, -9 }


        foreach (int dir in dirs)
            {
                for (int i = 1; i < 8; i += 1)
                {
                    int targetSquare = startSquare + dir * i;
                    if (targetSquare < 0 || targetSquare >= 64) break;
                    if (Math.Abs(Board.Rank(startSquare) - Board.Rank(targetSquare)) != i) break;
                    int targetPiece = squares[targetSquare];

                    if (Piece.Color(targetPiece) != color) bishopMoves.Add(new Move(startSquare, targetPiece));
                    else break;
                }
            }

            return bishopMoves;
        }

        private static List<Move> GetPawnMoves(int[] squares, int startSquare, int color)
        {

            List<Move> pawnMoves = new List<Move>();
            int colorToCapture = Piece.OppositeColor(color);
            int forwardDir = color == Piece.White ? 1 : -1;
            int initialPawnRank = color == Piece.White ? 1 : 6;

            int forwardSquare = startSquare + 8 * forwardDir;
            int forwardPiece = squares[forwardSquare];
            if (forwardPiece == Piece.None)
            {
                pawnMoves.Add(new Move(startSquare, forwardSquare));

                int doubleForwardSquare = startSquare + 16 * forwardDir;
                if (Board.Rank(startSquare) == initialPawnRank)
                {
                    int doubleForwardPiece = squares[doubleForwardSquare];
                    if (doubleForwardPiece == Piece.None) pawnMoves.Add(new Move(startSquare, doubleForwardSquare));
                }
            }

            if ((color == Piece.White && Board.File(startSquare) != 0) || (color == Piece.Black && Board.File(startSquare) != 7))
            {
                int diagonalSquare = startSquare + 7 * forwardDir;
                int diagonalPiece = squares[diagonalSquare];
                if (Piece.Color(diagonalPiece) == colorToCapture) pawnMoves.Add(new Move(startSquare, diagonalSquare));
            }

            if ((color == Piece.White && Board.File(startSquare) != 7) || (color == Piece.Black && Board.File(startSquare) != 0))
            {
                int diagonalSquare = startSquare + 9 * forwardDir;
                int diagonalPiece = squares[diagonalSquare];
                if (Piece.Color(diagonalPiece) == colorToCapture) pawnMoves.Add(new Move(startSquare, diagonalSquare));
            }

            return pawnMoves;

            // Get en passant moves too
            // Handle promotion?
        }
    }
}
