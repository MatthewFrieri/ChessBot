using System;

static class Zobrist
{
    const int seed = 236192;
    static Random random = new Random(seed);
    static ulong[,,] piecesAndSquares = new ulong[6, 2, 64];
    static ulong[] castlingRights = new ulong[4];
    static ulong[] enPassantFiles = new ulong[8];
    static ulong whiteToMove;

    static Zobrist()
    {
        RecordRandomNumbers();
    }

    public static ulong GetZobristHash()
    {
        ulong hash = 0;
        for (int i = 0; i < 64; i++)
        {
            int piece = Board.PieceAt(i);
            if (piece == Piece.None) { continue; }
            int pieceType = Piece.Type(piece);
            int colorIndex = Piece.Color(piece) == Piece.White ? 0 : 1;
            hash ^= piecesAndSquares[pieceType - 1, colorIndex, i];
        }
        foreach (int square in GameState.CastleSquares)
        {
            switch (square)
            {
                case 6:
                    hash ^= castlingRights[0];
                    break;
                case 2:
                    hash ^= castlingRights[1];
                    break;
                case 62:
                    hash ^= castlingRights[2];
                    break;
                case 58:
                    hash ^= castlingRights[3];
                    break;
            }
        }
        if (GameState.VulnerableEnPassantSquare is int vulnerableSquare)
        {
            hash ^= enPassantFiles[Board.File(vulnerableSquare)];
        }
        if (GameState.ColorToMove == Piece.White)
        {
            hash ^= whiteToMove;
        }

        return hash;
    }

    private static void RecordRandomNumbers()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < 64; k++)
                {
                    piecesAndSquares[i, j, k] = GenerateRandomUlong();
                }
            }
        }
        for (int i = 0; i < 4; i++)
        {
            castlingRights[i] = GenerateRandomUlong();
        }
        for (int i = 0; i < 8; i++)
        {
            enPassantFiles[i] = GenerateRandomUlong();
        }
        whiteToMove = GenerateRandomUlong();
    }

    private static ulong GenerateRandomUlong()
    {
        byte[] bytes = new byte[8];
        random.NextBytes(bytes);
        return BitConverter.ToUInt64(bytes, 0);
    }
}