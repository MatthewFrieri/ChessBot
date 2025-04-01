using System.Collections.Generic;

static class Evaluate
{
    private static Dictionary<int, int> pieceTypeToValue = new Dictionary<int, int>{
        { 1, 100 },
        { 2, 500 },
        { 3, 300 },
        { 4, 320 },
        { 5, 900 },
        { 6, 0 }
    };

    public static int EvaluatePosition(Board board, GameState gameState)
    {

        List<Move> legalMoves = LegalMoves.GetLegalMoves(board, gameState);
        if (legalMoves.Count == 0)
        {
            return int.MinValue;
        }

        int perspective = gameState.ColorToMove == Piece.White ? 1 : -1;

        int whiteValue = GetMaterialValue(board, Piece.White);
        int blackValue = GetMaterialValue(board, Piece.Black);

        return (whiteValue - blackValue) * perspective;
    }

    private static int GetMaterialValue(Board board, int color)
    {
        int totalValue = 0;

        for (int i = 0; i < 64; i += 1)
        {
            int piece = board.PieceAt(i);
            if (Piece.Color(piece) == color)
            {
                totalValue += pieceTypeToValue[Piece.Type(piece)];
            }
        }

        return totalValue;
    }
}
