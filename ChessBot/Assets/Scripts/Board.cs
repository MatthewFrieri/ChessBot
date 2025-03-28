using System;
using System.Collections.Generic;

namespace Chess
{
    public static class Board
    {
        static readonly int[] Squares;

        static Board()
        {
            Squares = new int[64];
        }


        public static void LoadFromFEN(string fen)
        {

            Dictionary<char, int> symbolToPiece = new Dictionary<char, int>();
            symbolToPiece['p'] = Piece.Pawn;
            symbolToPiece['r'] = Piece.Rook;
            symbolToPiece['n'] = Piece.Knight;
            symbolToPiece['b'] = Piece.Bishop;
            symbolToPiece['q'] = Piece.Queen;
            symbolToPiece['k'] = Piece.King;

            int rank = 7;
            int file = 0;

            foreach (char symbol in fen.Split(' ')[0])
            {

                if (symbol == '/')
                {
                    rank -= 1;
                    file = 0;
                    continue;
                }

                if (char.IsDigit(symbol))
                {
                    file += (int)char.GetNumericValue(symbol);
                    continue;
                }


                int pieceType = symbolToPiece[char.ToLower(symbol)];
                int pieceColor = char.IsUpper(symbol) ? Piece.White : Piece.Black;
                int piece = pieceType | pieceColor;

                Squares[rank * 8 + file] = piece;

                file += 1;
            }
        }

        public static int[] GetSquares()
        {
            return Squares;
        }

    }
}

