using System;
using System.Collections.Generic;

namespace Chess
{
    public static class Board
    {
        static int[] squares;
        static int colorToPlay;
        static List<Move> moveHistory;
        static int vulnerableEnPassantSquare;


        public static List<Move> tempLegalMoves;

        static Board()
        {
            squares = new int[64];
            colorToPlay = Piece.White;
            moveHistory = new List<Move>();
            vulnerableEnPassantSquare = 44;
        }

        public static int[] Squares
        {
            get
            {
                return squares;
            }
        }

        public static int VulnerableEnPassantSquare
        {
            get
            {
                return vulnerableEnPassantSquare;
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

        public static void ExecuteMove(Move move)
        {
            moveHistory.Add(move);
            if (move.MoveFlag == Move.Flag.PawnTwoForward)
            {
                vulnerableEnPassantSquare = move.StartSquare < move.TargetSquare ? move.StartSquare + 8 : move.StartSquare - 8;
            }
            else
            {
                vulnerableEnPassantSquare = -1;
            }
        }

        public static int[] PretendExecuteMove(Move move)
        {
            int[] squaresCopy = (int[])squares.Clone();
            squaresCopy[move.TargetSquare] = squaresCopy[move.StartSquare];
            squaresCopy[move.StartSquare] = Piece.None;

            if (move.MoveFlag == Move.Flag.EnPassantCapture)
            {
                int enPassantCaptureSquare = move.StartSquare < move.TargetSquare ? move.TargetSquare - 8 : move.TargetSquare + 8;
                squaresCopy[enPassantCaptureSquare] = Piece.None;
            }

            return squaresCopy;
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
