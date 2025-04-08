using System.Collections.Generic;

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

    public static Move AlgebraicToMove(string algebraic)
    {
        return new Move(0, 0);
    }

    public static string MoveToAlgebraic(Move move)
    {

        // Castling
        if (move.MoveFlag == Move.Flag.Castling)
        {
            switch (Board.File(move.TargetSquare))
            {
                case 2:
                    return "O-O-O";
                case 6:
                    return "O-O";
            }
        }

        // Check
        Board.RecordMove(move);  // Pretend to make the move. * This is okay because GameState.RecordMove() essentially already happened *
        int enemyKingSquare = LegalMoves.FindFriendlyKingSquare();
        string checkSymbol = LegalMoves.IsSquareUnderAttack(enemyKingSquare, Piece.OppositeColor(GameState.ColorToMove)) ? "+" : "";
        Board.UnRecordMove();   // Undo the pretend move

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

        // Differentiator for when two identical non-pawns can move to the same square 
        string differentiator = "";
        if (friendlyPieceType != Piece.Pawn)
        {
            List<int> similarSquares = LegalMoves.SquaresThatSquareIsTargettedBy(move.TargetSquare, friendlyPiece);
            bool identifyFile = false;
            bool identifyRank = false;
            foreach (int square in similarSquares)
            {
                if (square == move.StartSquare) { continue; }
                if (Board.File(square) == Board.File(move.StartSquare)) { identifyRank = true; continue; }
                if (Board.Rank(square) == Board.Rank(move.StartSquare)) { identifyFile = true; continue; }
                if (Board.File(square) != Board.File(move.StartSquare) && Board.Rank(square) != Board.Rank(move.StartSquare))
                {
                    identifyFile = true;
                    identifyRank = true;
                }
            }
            if (identifyFile) { differentiator += startSquareAlgebraic.Substring(0, 1); }
            if (identifyRank) { differentiator += startSquareAlgebraic.Substring(1, 1); }
        }

        return friendlyPieceSymbol + differentiator + pawnFileSymbol + captureSymbol + targetSquareAlgebraic + promotionSymbol + checkSymbol;
    }


}