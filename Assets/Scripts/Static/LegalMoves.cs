using System;
using System.Collections.Generic;


static class LegalMoves
{

    public static List<Move> GetLegalMoves(Board board, GameState gameState)
    {
        List<Move> pseudoLegalMoves = GetPseudoLegalMoves(board, gameState);
        List<Move> legalMoves = RemoveSelfCheckMoves(board, gameState, pseudoLegalMoves);
        return legalMoves;
    }

    private static List<Move> GetPseudoLegalMoves(Board board, GameState gameState)
    {
        List<Move> moves = new List<Move>();

        for (int startSquare = 0; startSquare < 64; startSquare += 1)
        {

            int piece = board.PieceAt(startSquare);
            if (Piece.Color(piece) != gameState.ColorToMove) continue;

            switch (Piece.Type(piece))
            {
                case Piece.Pawn:
                    moves.AddRange(GetPawnMoves(board, gameState, startSquare));
                    break;
                case Piece.Rook:
                    moves.AddRange(GetRookMoves(board, gameState, startSquare));
                    break;
                case Piece.Knight:
                    moves.AddRange(GetKnightMoves(board, gameState, startSquare));
                    break;
                case Piece.Bishop:
                    moves.AddRange(GetBishopMoves(board, gameState, startSquare));
                    break;
                case Piece.Queen:
                    moves.AddRange(GetRookMoves(board, gameState, startSquare));
                    moves.AddRange(GetBishopMoves(board, gameState, startSquare));
                    break;
                case Piece.King:
                    moves.AddRange(GetKingMoves(board, gameState, startSquare));
                    break;
            }
        }
        return moves;
    }


    private static List<Move> RemoveSelfCheckMoves(Board board, GameState gameState, List<Move> moves)
    {
        List<Move> safeMoves = new List<Move>();

        foreach (Move move in moves)
        {

            // Simulate making the move
            Board boardCopy = Board.Copy(board);
            GameState gameStateCopy = GameState.Copy(gameState);

            boardCopy.RecordMove(move);
            gameStateCopy.RecordMove(move);


            if (!IsInCheck(boardCopy, gameStateCopy))
            {
                safeMoves.Add(move);
            }
        }
        return safeMoves;
    }

    private static bool IsInCheck(Board board, GameState gameState)
    {
        // This can be optimized to start from the king and go out
        // I reused GetPseudoLegalMoves because im lazy

        List<Move> allMoves = GetPseudoLegalMoves(board, gameState);

        foreach (Move move in allMoves)
        {
            int targetPiece = board.PieceAt(move.TargetSquare);
            if (Piece.Type(targetPiece) == Piece.King) return true;
        }
        return false;
    }

    private static bool IsSquareUnderEnemyAttack(Board board, GameState gameState, int square)
    {
        int enemyColor = Piece.OppositeColor(gameState.ColorToMove);
        int[] knightOffsets = { -10, -17, -15, -6, 10, 17, 15, 6 };
        int[] pawnOffsets = enemyColor == Piece.White ? new int[] { -7, -9 } : new int[] { 7, 9 };
        int[] kingOffsets = { 1, 7, 8, 9, -1, -7, -8, -9 };
        int[] rookDirs = { 1, 8, -1, -8 };
        int[] bishopDirs = { 7, 9, -7, -9 };

        foreach (int offset in knightOffsets)
        {
            int targetSquare = square + offset;
            if (targetSquare < 0 || targetSquare >= 64) continue;  // Stops going off the top and bottom of the board
            if (Math.Abs(Board.File(targetSquare) - Board.File(targetSquare)) > 2) continue;  // Stops going off the sides of the board 
            if (board.PieceAt(targetSquare) == (Piece.Knight | enemyColor)) { return true; }
        }
        foreach (int offset in pawnOffsets)
        {
            int targetSquare = square + offset;
            if (targetSquare < 0 || targetSquare >= 64) continue;  // Stops going off the top and bottom of the board
            if (Math.Abs(Board.File(targetSquare) - Board.File(targetSquare)) > 1) continue;  // Stops going off the sides of the board 
            if (board.PieceAt(targetSquare) == (Piece.Pawn | enemyColor)) { return true; }
        }
        foreach (int offset in kingOffsets)
        {
            int targetSquare = square + offset;
            if (targetSquare < 0 || targetSquare >= 64) continue;  // Stops going off the top and bottom of the board
            if (Math.Abs(Board.File(targetSquare) - Board.File(targetSquare)) > 1) continue;  // Stops going off the sides of the board 
            if (board.PieceAt(targetSquare) == (Piece.King | enemyColor)) { return true; }
        }
        foreach (int dir in rookDirs)
        {
            for (int i = 1; i < 8; i += 1)
            {
                int targetSquare = square + dir * i;
                if (targetSquare < 0 || targetSquare >= 64) break;  // Stops going off the top and bottom of the board
                if (Math.Abs(dir) == 1 && Board.Rank(square) != Board.Rank(targetSquare)) break;  // Stops from going accross sides of the board
                int targetPiece = board.PieceAt(targetSquare);
                if (targetPiece == (Piece.Rook | enemyColor) || targetPiece == (Piece.Queen | enemyColor)) { return true; }
                if (targetPiece != Piece.None) break;  // Stop this direction if a piece is hit
            }
        }
        foreach (int dir in bishopDirs)
        {
            for (int i = 1; i < 8; i += 1)
            {
                int targetSquare = square + dir * i;
                if (targetSquare < 0 || targetSquare >= 64) break;  // Stops going off the top and bottom of the board
                if (Math.Abs(Board.Rank(square) - Board.Rank(targetSquare)) != i) break;  // Stops from going accross sides of the board
                int targetPiece = board.PieceAt(targetSquare);
                if (targetPiece == (Piece.Bishop | enemyColor) || targetPiece == (Piece.Queen | enemyColor)) { return true; }
                if (targetPiece != Piece.None) break;  // Stop this direction if a piece is hit
            }
        }
        return false;
    }


    private static List<Move> GetOffsetPieceMoves(Board board, GameState gameState, int startSquare, int[] offsets)
    {

        List<Move> moves = new List<Move>();

        foreach (int offset in offsets)
        {
            int targetSquare = startSquare + offset;
            if (targetSquare < 0 || targetSquare >= 64) continue;  // Stops going off the top and bottom of the board
            if (Math.Abs(Board.File(startSquare) - Board.File(targetSquare)) > 2) continue;  // Stops going off the sides of the board 

            int targetPiece = board.PieceAt(targetSquare);
            if (Piece.Color(targetPiece) == gameState.ColorToMove) continue;  // Stops capturing friendly piece

            moves.Add(new Move(startSquare, targetSquare));
        }

        return moves;
    }

    private static List<Move> GetKingMoves(Board board, GameState gameState, int startSquare)
    {
        int[] offsets = { 1, 7, 8, 9, -1, -7, -8, -9 };
        List<Move> kingMoves = GetOffsetPieceMoves(board, gameState, startSquare, offsets);

        // Add castle moves
        foreach (int castleSquare in gameState.CastleSquares)
        {
            if (Board.Rank(castleSquare) == Board.Rank(startSquare))
            {

                // Make sure the path is not blocked
                switch (castleSquare)
                {
                    case 6:
                        if (board.PieceAt(5) == Piece.None &&
                        board.PieceAt(6) == Piece.None &&
                        !IsSquareUnderEnemyAttack(board, gameState, startSquare) &&
                        !IsSquareUnderEnemyAttack(board, gameState, 5) &&
                        !IsSquareUnderEnemyAttack(board, gameState, 6))
                        {
                            kingMoves.Add(new Move(startSquare, castleSquare, Move.Flag.Castling));
                        }
                        break;
                    case 2:
                        if (board.PieceAt(3) == Piece.None &&
                        board.PieceAt(2) == Piece.None &&
                        board.PieceAt(1) == Piece.None &&
                        !IsSquareUnderEnemyAttack(board, gameState, startSquare) &&
                        !IsSquareUnderEnemyAttack(board, gameState, 3) &&
                        !IsSquareUnderEnemyAttack(board, gameState, 2))
                        {
                            kingMoves.Add(new Move(startSquare, castleSquare, Move.Flag.Castling));
                        }
                        break;
                    case 62:
                        if (board.PieceAt(61) == Piece.None &&
                        board.PieceAt(62) == Piece.None &&
                        !IsSquareUnderEnemyAttack(board, gameState, startSquare) &&
                        !IsSquareUnderEnemyAttack(board, gameState, 61) &&
                        !IsSquareUnderEnemyAttack(board, gameState, 62))
                        {
                            kingMoves.Add(new Move(startSquare, castleSquare, Move.Flag.Castling));
                        }
                        break;
                    case 58:
                        if (board.PieceAt(59) == Piece.None &&
                        board.PieceAt(58) == Piece.None &&
                        board.PieceAt(57) == Piece.None &&
                        !IsSquareUnderEnemyAttack(board, gameState, startSquare) &&
                        !IsSquareUnderEnemyAttack(board, gameState, 59) &&
                        !IsSquareUnderEnemyAttack(board, gameState, 58))
                        {
                            kingMoves.Add(new Move(startSquare, castleSquare, Move.Flag.Castling));
                        }
                        break;
                }
            }
        }

        return kingMoves;
    }

    private static List<Move> GetKnightMoves(Board board, GameState gameState, int startSquare)
    {
        int[] offsets = new int[] { -10, -17, -15, -6, 10, 17, 15, 6 };
        return GetOffsetPieceMoves(board, gameState, startSquare, offsets);
    }

    private static List<Move> GetRookMoves(Board board, GameState gameState, int startSquare)
    {
        List<Move> rookMoves = new List<Move>();
        int[] dirs = { 1, 8, -1, -8 };


        foreach (int dir in dirs)
        {
            for (int i = 1; i < 8; i += 1)
            {
                int targetSquare = startSquare + dir * i;
                if (targetSquare < 0 || targetSquare >= 64) break;  // Stops going off the top and bottom of the board
                if (Math.Abs(dir) == 1 && Board.Rank(startSquare) != Board.Rank(targetSquare)) break;  // Stops from going accross sides of the board

                int targetPiece = board.PieceAt(targetSquare);
                if (Piece.Color(targetPiece) == gameState.ColorToMove) break;  // Blocked by same color piece

                rookMoves.Add(new Move(startSquare, targetSquare));

                if (Piece.Color(targetPiece) == Piece.OppositeColor(gameState.ColorToMove)) break;  // Blocked by capture
            }
        }

        return rookMoves;
    }

    private static List<Move> GetBishopMoves(Board board, GameState gameState, int startSquare)
    {
        List<Move> bishopMoves = new List<Move>();
        int[] dirs = { 7, 9, -7, -9 };
        int colorToCapture = Piece.OppositeColor(gameState.ColorToMove);


        foreach (int dir in dirs)
        {
            for (int i = 1; i < 8; i += 1)
            {
                int targetSquare = startSquare + dir * i;
                if (targetSquare < 0 || targetSquare >= 64) break;  // Stops going off the top and bottom of the board
                if (Math.Abs(Board.Rank(startSquare) - Board.Rank(targetSquare)) != i) break;  // Stops from going accross sides of the board

                int targetPiece = board.PieceAt(targetSquare);
                if (Piece.Color(targetPiece) == gameState.ColorToMove) break;  // Blocked by same color piece

                bishopMoves.Add(new Move(startSquare, targetSquare));

                if (Piece.Color(targetPiece) == colorToCapture) break;  // Blocked by capture
            }
        }

        return bishopMoves;
    }

    private static List<Move> GetPawnMoves(Board board, GameState gameState, int startSquare)
    {

        List<Move> pawnMoves = new List<Move>();
        bool whiteToMove = Piece.IsWhite(gameState.ColorToMove);
        int colorToCapture = Piece.OppositeColor(gameState.ColorToMove);
        int forwardDir = whiteToMove ? 1 : -1;
        int initialPawnRank = whiteToMove ? 1 : 6;
        int promotionRank = whiteToMove ? 7 : 0;

        int forwardSquare = startSquare + 8 * forwardDir;
        int forwardPiece = board.PieceAt(forwardSquare);
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
                int twoForwardPiece = board.PieceAt(twoForwardSquare);
                if (twoForwardPiece == Piece.None) { pawnMoves.Add(new Move(startSquare, twoForwardSquare, Move.Flag.PawnTwoForward)); }  // Advance two
            }
        }

        void HandlePawnDiagonal(int offset, Board board, GameState gameState, int startSquare, List<Move> pawnMoves, int forwardDir, int colorToCapture, int promotionRank)
        {
            int diagonalSquare = startSquare + offset * forwardDir;
            int diagonalPiece = board.PieceAt(diagonalSquare);
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
            if (diagonalSquare == gameState.VulnerableEnPassantSquare) { pawnMoves.Add(new Move(startSquare, diagonalSquare, Move.Flag.EnPassantCapture)); } // En passant capture
        }

        // Left diagonal
        if ((whiteToMove && Board.File(startSquare) != 0) || (!whiteToMove && Board.File(startSquare) != 7))
        {
            HandlePawnDiagonal(7, board, gameState, startSquare, pawnMoves, forwardDir, colorToCapture, promotionRank);
        }

        // Right diagonal
        if ((whiteToMove && Board.File(startSquare) != 7) || (!whiteToMove && Board.File(startSquare) != 0))
        {
            HandlePawnDiagonal(9, board, gameState, startSquare, pawnMoves, forwardDir, colorToCapture, promotionRank);
        }

        return pawnMoves;

        // Handle promotion
    }
}

