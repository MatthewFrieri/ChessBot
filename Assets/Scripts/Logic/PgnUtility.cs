using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

static class PgnUtility
{
    private static Dictionary<int, string> pieceTypeToSymbol = new Dictionary<int, string>
    {
        {Piece.Pawn, ""},
        {Piece.Rook, "R"},
        {Piece.Knight, "N"},
        {Piece.Bishop, "B"},
        {Piece.Queen, "Q"},
        {Piece.King, "K"}
    };
    private static Dictionary<char, int> symbolToPieceType = new Dictionary<char, int>
    {
        {'R', Piece.Rook},
        {'N', Piece.Knight},
        {'B', Piece.Bishop},
        {'Q', Piece.Queen},
        {'K', Piece.King}
    };
    private static Dictionary<char, int> symbolToPromotionFlag = new Dictionary<char, int>
    {
        {'R', Move.Flag.PromoteToRook},
        {'N', Move.Flag.PromoteToKnight},
        {'B', Move.Flag.PromoteToBishop},
        {'Q', Move.Flag.PromoteToQueen},
    };

    public static Move AlgebraicToMove(string algebraic)
    {
        // Castling
        if (algebraic[0] == 'O')
        {
            int castleStartSquare = GameState.ColorToMove == Piece.White ? 4 : 60;
            int castleTargetSquare;

            if (algebraic == "O-O")  // King side castle
            {
                castleTargetSquare = GameState.ColorToMove == Piece.White ? 6 : 62;
            }
            else  // Queen side castle
            {
                castleTargetSquare = GameState.ColorToMove == Piece.White ? 2 : 58;
            }

            return new Move(castleStartSquare, castleTargetSquare, Move.Flag.Castling);
        }

        // Find the target square
        int lastDigitIndex = -1;  // Will get assigned
        for (int i = 0; i < algebraic.Length; i++)
        {
            if (char.IsDigit(algebraic[i]))
            {
                lastDigitIndex = i;
            }
        }
        string targetAlgebraic = algebraic.Substring(lastDigitIndex - 1, 2);
        int targetSquare = Helpers.AlgebraicToSquare(targetAlgebraic);

        // Find the friendly piece
        char pieceSymbol = algebraic[0];
        int friendlyPieceType = char.IsUpper(pieceSymbol) ? symbolToPieceType[pieceSymbol] : Piece.Pawn;
        int friendlyPiece = friendlyPieceType | GameState.ColorToMove;


        // Find the start square
        List<int> startSquareOptions = new List<int>();
        if (friendlyPieceType == Piece.Pawn && !algebraic.Contains('x'))  // Consider pawns that move forward seperately because they dont "target" the target square
        {
            int dir = GameState.ColorToMove == Piece.White ? -1 : 1;

            int backOneSquare = targetSquare + dir * 8;
            int backTwoSquare = targetSquare + dir * 16;
            int backTwoRank = (int)(3.5 + dir * 2.5);

            if (Board.PieceAt(backOneSquare) == friendlyPiece)
            {
                startSquareOptions.Add(backOneSquare);
            }
            else if (Board.PieceAt(backTwoSquare) == friendlyPiece && Board.Rank(backTwoSquare) == backTwoRank && Board.PieceAt(backOneSquare) == Piece.None)
            {
                startSquareOptions.Add(backTwoSquare);
            }
        }
        else
        {
            startSquareOptions = LegalMoves.SquaresThatSquareIsTargettedBy(targetSquare, friendlyPiece);
        }


        int startSquare = -1;  // Will get overwritten


        // Find start square
        if (startSquareOptions.Count == 1)
        {
            startSquare = startSquareOptions[0];
        }
        else
        {
            List<char> differentiators = new List<char>();
            for (int i = 0; i < lastDigitIndex - 1; i++)
            {
                char c = algebraic[i];
                if (char.IsUpper(c)) { continue; }
                if (c == 'x') { break; }
                differentiators.Add(c);
            }

            if (differentiators.Count == 2)  // Exact startSquare is known
            {
                startSquare = Helpers.AlgebraicToSquare(string.Join("", differentiators));
            }
            else  // At this point differentiator.Count == 1
            {
                char differentiator = differentiators[0];
                if (char.IsLetter(differentiator))  // File is known
                {
                    foreach (int square in startSquareOptions)
                    {
                        if (Helpers.SquareToAlgebraic(square)[0] == differentiator) { startSquare = square; break; }
                    }
                }
                else  // Rank is known
                {
                    foreach (int square in startSquareOptions)
                    {
                        if (Board.Rank(square) == char.GetNumericValue(differentiator)) { startSquare = square; break; }
                    }
                }
            }
        }

        // Find flag
        int flag = Move.Flag.None;

        int equalSignIndex = algebraic.IndexOf('=');
        if (equalSignIndex != -1)  // Promotion
        {
            char promotionSymbol = algebraic[equalSignIndex + 1];
            flag = symbolToPromotionFlag[promotionSymbol];
        }

        if (friendlyPieceType == Piece.Pawn)
        {
            if (Math.Abs(Board.Rank(startSquare) - Board.Rank(targetSquare)) > 1)  // Pawn pushed two
            {
                flag = Move.Flag.PawnTwoForward;
            }

            if (targetSquare == GameState.VulnerableEnPassantSquare)  // En passant capture
            {
                flag = Move.Flag.EnPassantCapture;
            }
        }

        return new Move(startSquare, targetSquare, flag);
    }

    public static string MoveToAlgebraic(Move move)
    {

        // Check and checkmate
        Board.RecordMove(move);  // Pretend to make the move. * This is okay because GameState.RecordMove() essentially already happened *
        int enemyKingSquare = LegalMoves.FindFriendlyKingSquare();
        bool isInCheck = LegalMoves.IsSquareUnderAttack(enemyKingSquare, Piece.OppositeColor(GameState.ColorToMove));
        bool hasMovesAfter = LegalMoves.GetLegalMoves().Count > 0;
        string checkSymbol = isInCheck ? (hasMovesAfter ? "+" : "#") : "";
        Board.UnRecordMove();   // Undo the pretend move

        // Castling
        if (move.MoveFlag == Move.Flag.Castling)
        {
            switch (Board.File(move.TargetSquare))
            {
                case 2:
                    return "O-O-O" + checkSymbol;
                case 6:
                    return "O-O" + checkSymbol;
            }
        }

        // Target square
        string targetSquareAlgebraic = Helpers.SquareToAlgebraic(move.TargetSquare);

        // Friendly piece
        int friendlyPiece = Board.PieceAt(move.StartSquare);
        int friendlyPieceType = Piece.Type(friendlyPiece);
        string friendlyPieceSymbol = pieceTypeToSymbol[friendlyPieceType];

        // Capture
        bool isCapture = Board.PieceAt(move.TargetSquare) != Piece.None || move.MoveFlag == Move.Flag.EnPassantCapture;
        string captureSymbol = isCapture ? "x" : "";

        // Pawn file when capturing
        string startSquareAlgebraic = Helpers.SquareToAlgebraic(move.StartSquare);
        string pawnFileSymbol = isCapture && friendlyPieceType == Piece.Pawn ? startSquareAlgebraic.Substring(0, 1) : "";

        // Promotion
        string promotionSymbol = "";
        switch (move.MoveFlag)
        {
            case Move.Flag.PromoteToQueen:
                promotionSymbol += "=Q";
                break;
            case Move.Flag.PromoteToRook:
                promotionSymbol += "=R";
                break;
            case Move.Flag.PromoteToBishop:
                promotionSymbol += "=B";
                break;
            case Move.Flag.PromoteToKnight:
                promotionSymbol += "=N";
                break;
        }

        // Differentiator for when at least two identical non-pawns can move to the same square 
        string differentiator = "";
        List<int> similarSquares = LegalMoves.SquaresThatSquareIsTargettedBy(move.TargetSquare, friendlyPiece);

        if (friendlyPieceType != Piece.Pawn && similarSquares.Count > 1)
        {
            bool uniqueFile = true;
            bool uniqueRank = true;

            foreach (int square in similarSquares)
            {
                if (square == move.StartSquare) { continue; }
                if (Board.File(square) == Board.File(move.StartSquare)) { uniqueFile = false; }
                if (Board.Rank(square) == Board.Rank(move.StartSquare)) { uniqueRank = false; }
            }

            if (uniqueFile)
            {
                differentiator = startSquareAlgebraic.Substring(0, 1);
            }
            else if (uniqueRank)
            {
                differentiator = startSquareAlgebraic.Substring(1, 1);
            }
            else
            {
                differentiator = startSquareAlgebraic;
            }
        }

        return friendlyPieceSymbol + differentiator + pawnFileSymbol + captureSymbol + targetSquareAlgebraic + promotionSymbol + checkSymbol;
    }


}