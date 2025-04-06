using System;
using System.Collections.Generic;
using UnityEngine;


static class LegalMoves
{

    public static List<Move> GetLegalMoves()
    {
        List<Move> pseudoLegalMoves = GetPseudoLegalMoves();
        List<Move> legalMoves = RemoveSelfCheckMoves(pseudoLegalMoves);
        return legalMoves;
    }

    private static List<Move> GetPseudoLegalMoves()
    {
        List<Move> moves = new List<Move>();

        for (int startSquare = 0; startSquare < 64; startSquare++)
        {

            int piece = Board.PieceAt(startSquare);
            if (Piece.Color(piece) != GameState.ColorToMove) continue;

            switch (Piece.Type(piece))
            {
                case Piece.Pawn:
                    moves.AddRange(GetPawnMoves(startSquare));
                    break;
                case Piece.Rook:
                    moves.AddRange(GetRookMoves(startSquare));
                    break;
                case Piece.Knight:
                    moves.AddRange(GetKnightMoves(startSquare));
                    break;
                case Piece.Bishop:
                    moves.AddRange(GetBishopMoves(startSquare));
                    break;
                case Piece.Queen:
                    moves.AddRange(GetRookMoves(startSquare));
                    moves.AddRange(GetBishopMoves(startSquare));
                    break;
                case Piece.King:
                    moves.AddRange(GetKingMoves(startSquare));
                    break;
            }
        }
        return moves;
    }


    private static List<Move> RemoveSelfCheckMoves(List<Move> moves)
    {
        List<Move> safeMoves = new List<Move>();

        int friendlyKingSquare = FindFriendlyKingSquare();

        foreach (Move move in moves)
        {
            // Pretend to make the move
            GameState.RecordMove(move);
            Board.RecordMove(move);

            // Use the TargetSquare if the king is moving
            if (move.StartSquare == friendlyKingSquare)
            {
                if (!IsSquareUnderAttack(move.TargetSquare, GameState.ColorToMove))
                {
                    safeMoves.Add(move);
                }
            }

            // The king is not moving so friendlyKingSquare is acurate
            else if (!IsSquareUnderAttack(friendlyKingSquare, GameState.ColorToMove))
            {
                safeMoves.Add(move);
            }

            // Undo the pretend move
            GameState.UnRecordMove();
            Board.UnRecordMove();
        }

        return safeMoves;
    }

    public static int FindFriendlyKingSquare()
    {
        for (int i = 0; i < 64; i++)
        {
            if (Board.PieceAt(i) == (Piece.King | GameState.ColorToMove))
            {
                return i;
            }
        }
        return -1;  // This line will never get reached
    }

    public static bool IsSquareUnderAttack(int square, int enemyColor)
    {
        int[] knightOffsets = { -10, -17, -15, -6, 10, 17, 15, 6 };
        int[] pawnOffsets = enemyColor == Piece.White ? new int[] { -7, -9 } : new int[] { 7, 9 };
        int[] kingOffsets = { 1, 7, 8, 9, -1, -7, -8, -9 };
        int[] rookDirs = { 1, 8, -1, -8 };
        int[] bishopDirs = { 7, 9, -7, -9 };

        foreach (int offset in knightOffsets)
        {
            int targetSquare = square + offset;
            if (targetSquare < 0 || targetSquare >= 64) continue;  // Stops going off the top and bottom of the board
            if (Math.Abs(Board.File(square) - Board.File(targetSquare)) > 2) continue;  // Stops going off the sides of the board 
            if (Board.PieceAt(targetSquare) == (Piece.Knight | enemyColor)) { return true; }
        }
        foreach (int offset in pawnOffsets)
        {
            int targetSquare = square + offset;
            if (targetSquare < 0 || targetSquare >= 64) continue;  // Stops going off the top and bottom of the board
            if (Math.Abs(Board.File(square) - Board.File(targetSquare)) > 1) continue;  // Stops going off the sides of the board 
            if (Board.PieceAt(targetSquare) == (Piece.Pawn | enemyColor)) { return true; }
        }
        foreach (int offset in kingOffsets)
        {
            int targetSquare = square + offset;
            if (targetSquare < 0 || targetSquare >= 64) continue;  // Stops going off the top and bottom of the board
            if (Math.Abs(Board.File(square) - Board.File(targetSquare)) > 1) continue;  // Stops going off the sides of the board 
            if (Board.PieceAt(targetSquare) == (Piece.King | enemyColor)) { return true; }
        }
        foreach (int dir in rookDirs)
        {
            for (int i = 1; i < 8; i++)
            {
                int targetSquare = square + dir * i;
                if (targetSquare < 0 || targetSquare >= 64) break;  // Stops going off the top and bottom of the board
                if (Math.Abs(dir) == 1 && Board.Rank(square) != Board.Rank(targetSquare)) break;  // Stops from going accross sides of the board
                int targetPiece = Board.PieceAt(targetSquare);
                if (targetPiece == (Piece.Rook | enemyColor) || targetPiece == (Piece.Queen | enemyColor)) { return true; }
                if (targetPiece != Piece.None) break;  // Stop this direction if a piece is hit
            }
        }
        foreach (int dir in bishopDirs)
        {
            for (int i = 1; i < 8; i++)
            {
                int targetSquare = square + dir * i;
                if (targetSquare < 0 || targetSquare >= 64) break;  // Stops going off the top and bottom of the board
                if (Math.Abs(Board.Rank(square) - Board.Rank(targetSquare)) != i) break;  // Stops from going accross sides of the board
                int targetPiece = Board.PieceAt(targetSquare);
                if (targetPiece == (Piece.Bishop | enemyColor) || targetPiece == (Piece.Queen | enemyColor)) { return true; }
                if (targetPiece != Piece.None) break;  // Stop this direction if a piece is hit
            }
        }
        return false;
    }


    private static List<Move> GetOffsetPieceMoves(int startSquare, int[] offsets)
    {

        List<Move> moves = new List<Move>();

        foreach (int offset in offsets)
        {
            int targetSquare = startSquare + offset;
            if (targetSquare < 0 || targetSquare >= 64) continue;  // Stops going off the top and bottom of the board
            if (Math.Abs(Board.File(startSquare) - Board.File(targetSquare)) > 2) continue;  // Stops going off the sides of the board 

            int targetPiece = Board.PieceAt(targetSquare);
            if (Piece.Color(targetPiece) == GameState.ColorToMove) continue;  // Stops capturing friendly piece

            moves.Add(new Move(startSquare, targetSquare));
        }

        return moves;
    }

    private static List<Move> GetKingMoves(int startSquare)
    {
        int enemyColor = Piece.OppositeColor(GameState.ColorToMove);
        int[] offsets = { 1, 7, 8, 9, -1, -7, -8, -9 };
        List<Move> kingMoves = GetOffsetPieceMoves(startSquare, offsets);

        // Add castle moves
        foreach (int castleSquare in GameState.CastleSquares)
        {
            if (Board.Rank(castleSquare) == Board.Rank(startSquare))
            {

                // Make sure the path is not blocked
                switch (castleSquare)
                {
                    case 6:
                        if (Board.PieceAt(5) == Piece.None &&
                        Board.PieceAt(6) == Piece.None &&
                        !IsSquareUnderAttack(startSquare, enemyColor) &&
                        !IsSquareUnderAttack(5, enemyColor) &&
                        !IsSquareUnderAttack(6, enemyColor))
                        {
                            kingMoves.Add(new Move(startSquare, castleSquare, Move.Flag.Castling));
                        }
                        break;
                    case 2:
                        if (Board.PieceAt(3) == Piece.None &&
                        Board.PieceAt(2) == Piece.None &&
                        Board.PieceAt(1) == Piece.None &&
                        !IsSquareUnderAttack(startSquare, enemyColor) &&
                        !IsSquareUnderAttack(3, enemyColor) &&
                        !IsSquareUnderAttack(2, enemyColor))
                        {
                            kingMoves.Add(new Move(startSquare, castleSquare, Move.Flag.Castling));
                        }
                        break;
                    case 62:
                        if (Board.PieceAt(61) == Piece.None &&
                        Board.PieceAt(62) == Piece.None &&
                        !IsSquareUnderAttack(startSquare, enemyColor) &&
                        !IsSquareUnderAttack(61, enemyColor) &&
                        !IsSquareUnderAttack(62, enemyColor))
                        {
                            kingMoves.Add(new Move(startSquare, castleSquare, Move.Flag.Castling));
                        }
                        break;
                    case 58:
                        if (Board.PieceAt(59) == Piece.None &&
                        Board.PieceAt(58) == Piece.None &&
                        Board.PieceAt(57) == Piece.None &&
                        !IsSquareUnderAttack(startSquare, enemyColor) &&
                        !IsSquareUnderAttack(59, enemyColor) &&
                        !IsSquareUnderAttack(58, enemyColor))
                        {
                            kingMoves.Add(new Move(startSquare, castleSquare, Move.Flag.Castling));
                        }
                        break;
                }
            }
        }

        return kingMoves;
    }

    private static List<Move> GetKnightMoves(int startSquare)
    {
        int[] offsets = new int[] { -10, -17, -15, -6, 10, 17, 15, 6 };
        return GetOffsetPieceMoves(startSquare, offsets);
    }

    private static List<Move> GetRookMoves(int startSquare)
    {
        List<Move> rookMoves = new List<Move>();
        int[] dirs = { 1, 8, -1, -8 };


        foreach (int dir in dirs)
        {
            for (int i = 1; i < 8; i++)
            {
                int targetSquare = startSquare + dir * i;
                if (targetSquare < 0 || targetSquare >= 64) break;  // Stops going off the top and bottom of the board
                if (Math.Abs(dir) == 1 && Board.Rank(startSquare) != Board.Rank(targetSquare)) break;  // Stops from going accross sides of the board

                int targetPiece = Board.PieceAt(targetSquare);
                if (Piece.Color(targetPiece) == GameState.ColorToMove) break;  // Blocked by same color piece

                rookMoves.Add(new Move(startSquare, targetSquare));

                if (Piece.Color(targetPiece) == Piece.OppositeColor(GameState.ColorToMove)) break;  // Blocked by capture
            }
        }

        return rookMoves;
    }

    private static List<Move> GetBishopMoves(int startSquare)
    {
        List<Move> bishopMoves = new List<Move>();
        int[] dirs = { 7, 9, -7, -9 };
        int colorToCapture = Piece.OppositeColor(GameState.ColorToMove);


        foreach (int dir in dirs)
        {
            for (int i = 1; i < 8; i++)
            {
                int targetSquare = startSquare + dir * i;
                if (targetSquare < 0 || targetSquare >= 64) break;  // Stops going off the top and bottom of the board
                if (Math.Abs(Board.Rank(startSquare) - Board.Rank(targetSquare)) != i) break;  // Stops from going accross sides of the board

                int targetPiece = Board.PieceAt(targetSquare);
                if (Piece.Color(targetPiece) == GameState.ColorToMove) break;  // Blocked by same color piece

                bishopMoves.Add(new Move(startSquare, targetSquare));

                if (Piece.Color(targetPiece) == colorToCapture) break;  // Blocked by capture
            }
        }

        return bishopMoves;
    }

    private static List<Move> GetPawnMoves(int startSquare)
    {

        List<Move> pawnMoves = new List<Move>();
        bool whiteToMove = Piece.IsWhite(GameState.ColorToMove);
        int colorToCapture = Piece.OppositeColor(GameState.ColorToMove);
        int forwardDir = whiteToMove ? 1 : -1;
        int initialPawnRank = whiteToMove ? 1 : 6;
        int promotionRank = whiteToMove ? 7 : 0;

        int forwardSquare = startSquare + 8 * forwardDir;
        int forwardPiece = Board.PieceAt(forwardSquare);
        if (forwardPiece == Piece.None)
        {
            if (Board.Rank(forwardSquare) == promotionRank)
            {
                pawnMoves.Add(new Move(startSquare, forwardSquare, Move.Flag.PromoteToQueen));  // Promote to queen 
                pawnMoves.Add(new Move(startSquare, forwardSquare, Move.Flag.PromoteToKnight));  // Promote to knight 
                pawnMoves.Add(new Move(startSquare, forwardSquare, Move.Flag.PromoteToRook));  // Promote to rook
                pawnMoves.Add(new Move(startSquare, forwardSquare, Move.Flag.PromoteToBishop));  // Promote to bishop 
            }
            else pawnMoves.Add(new Move(startSquare, forwardSquare));  // Normal advance

            int twoForwardSquare = startSquare + 16 * forwardDir;
            if (Board.Rank(startSquare) == initialPawnRank)
            {
                int twoForwardPiece = Board.PieceAt(twoForwardSquare);
                if (twoForwardPiece == Piece.None) { pawnMoves.Add(new Move(startSquare, twoForwardSquare, Move.Flag.PawnTwoForward)); }  // Advance two
            }
        }

        void HandlePawnDiagonal(int offset, int startSquare, List<Move> pawnMoves, int forwardDir, int colorToCapture, int promotionRank)
        {
            int diagonalSquare = startSquare + offset * forwardDir;
            int diagonalPiece = Board.PieceAt(diagonalSquare);
            if (Piece.Color(diagonalPiece) == colorToCapture)
            {
                if (Board.Rank(diagonalSquare) == promotionRank)
                {
                    pawnMoves.Add(new Move(startSquare, diagonalSquare, Move.Flag.PromoteToQueen));  // Capture and promote to queen 
                    pawnMoves.Add(new Move(startSquare, diagonalSquare, Move.Flag.PromoteToKnight));  // Capture and promote to knight 
                    pawnMoves.Add(new Move(startSquare, diagonalSquare, Move.Flag.PromoteToRook));  // Capture and promote to rook
                    pawnMoves.Add(new Move(startSquare, diagonalSquare, Move.Flag.PromoteToBishop));  // Capture and promote to bishop 
                }
                else
                {
                    pawnMoves.Add(new Move(startSquare, diagonalSquare));  // Normal capture
                }
            }
            if (diagonalSquare == GameState.VulnerableEnPassantSquare) { pawnMoves.Add(new Move(startSquare, diagonalSquare, Move.Flag.EnPassantCapture)); } // En passant capture
        }

        // Left diagonal
        if ((whiteToMove && Board.File(startSquare) != 0) || (!whiteToMove && Board.File(startSquare) != 7))
        {
            HandlePawnDiagonal(7, startSquare, pawnMoves, forwardDir, colorToCapture, promotionRank);
        }

        // Right diagonal
        if ((whiteToMove && Board.File(startSquare) != 7) || (!whiteToMove && Board.File(startSquare) != 0))
        {
            HandlePawnDiagonal(9, startSquare, pawnMoves, forwardDir, colorToCapture, promotionRank);
        }

        return pawnMoves;
    }
}

