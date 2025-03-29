using System;
using System.Collections.Generic;


public class Board
{
    private int[] squares;

    public Board()
    {
        squares = new int[64];
    }

    public Board(string fen)
    {
        LoadFromFEN(fen);
    }

    public int PieceAt(int square)
    {
        return squares[square];
    }

    public void RecordMove(Move move)
    {
        int friendlyColor = Piece.Color(squares[move.StartSquare]);
        squares[move.TargetSquare] = squares[move.StartSquare];
        squares[move.StartSquare] = Piece.None;

        switch (move.MoveFlag)
        {
            case Move.Flag.None:
                break;
            case Move.Flag.EnPassantCapture:
                int enPassantCaptureSquare = move.StartSquare < move.TargetSquare ? move.TargetSquare - 8 : move.TargetSquare + 8;
                squares[enPassantCaptureSquare] = Piece.None;
                break;
            case Move.Flag.Castling:
                break;
            case Move.Flag.PromoteToQueen:
                squares[move.TargetSquare] = Piece.Queen | friendlyColor;
                break;
            case Move.Flag.PromoteToKnight:
                squares[move.TargetSquare] = Piece.Knight | friendlyColor;
                break;
            case Move.Flag.PromoteToRook:
                squares[move.TargetSquare] = Piece.Rook | friendlyColor;
                break;
            case Move.Flag.PromoteToBishop:
                squares[move.TargetSquare] = Piece.Bishop | friendlyColor;
                break;
            case Move.Flag.PawnTwoForward:
                break;
        }
    }

    public void LoadFromFEN(string fen)
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

    public string HalfFEN()
    {
        Dictionary<int, char> pieceToSymbol = new Dictionary<int, char>{
            {Piece.Pawn, 'p'},
            {Piece.Rook, 'r'},
            {Piece.Knight, 'n'},
            {Piece.Bishop, 'b'},
            {Piece.Queen, 'q'},
            {Piece.King, 'k'}
        };

        string fen = "";

        for (int rank = 7; rank >= 0; rank -= 1)
        {
            int emptySquareStreak = 0;
            for (int file = 0; file < 8; file += 1)
            {
                int square = 8 * rank + file;
                int piece = PieceAt(square);

                if (piece == Piece.None)
                {
                    emptySquareStreak += 1;
                    continue;
                }

                if (emptySquareStreak > 0)
                {
                    fen += emptySquareStreak;
                    emptySquareStreak = 0;
                }

                char baseSymbol = pieceToSymbol[piece];
                char symbol = Piece.IsWhite(piece) ? char.ToUpper(baseSymbol) : baseSymbol;
                fen += symbol;
            }

            if (emptySquareStreak > 0)
            {
                fen += emptySquareStreak;
            }

            if (rank < 7)
            {
                fen += "/";
            }
        }

        return fen;
    }

    public static int File(int square)
    {
        return square % 8;
    }

    public static int Rank(int square)
    {
        return square / 8;
    }

    public static int AlgebraicToSquare(string algebraic)
    {
        int file = algebraic[0] - 'a';
        int rank = algebraic[1];

        return 8 * rank + file;
    }

    public static string SquareToAlgebraic(int square)
    {
        string file = ((char)('a' + File(square))).ToString();
        string rank = Rank(square).ToString();

        return file + rank;
    }

    public static Board Copy(Board board)
    {
        return new Board(board.HalfFEN());
    }

}
