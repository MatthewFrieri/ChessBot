using System;
using System.Collections.Generic;

namespace Chess
{
    public static class Board
    {
        static readonly int[] squares;
        static bool whiteToPlay;

        static Board()
        {
            squares = new int[64];
            whiteToPlay = true;
        }

        public static int[] Squares
        {
            get
            {
                return squares;
            }
        }

        public static int File(int square)
        {
            return square % 8;
        }

        public static int Rank(int square)
        {
            return square / 8;
        }

        public static void LoadFromFEN(string fen)
        {

            Dictionary<char, int> symbolToPiece = new Dictionary<char, int>{
                {'p', Piece.Pawn},
                {'r', Piece.Rook},
                {'n', Piece.Knight},
                {'b', Piece.Bishop},
                {'q', Piece.Queen},
                {'k', Piece.King}
            };

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

                squares[rank * 8 + file] = piece;

                file += 1;
            }
        }





    }
}

