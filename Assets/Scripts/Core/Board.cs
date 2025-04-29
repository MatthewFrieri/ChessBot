using System.Collections.Generic;
using System.Linq;
using UnityEngine;

static class Board
{
    private static Stack<int[]> squaresStack;
    private static Dictionary<string, int> boardFenToFrequency;


    public static void Init()
    {
        squaresStack = new Stack<int[]>();

        int[] squares = new int[64];
        squaresStack.Push(squares);

        // Set up the starting position
        squares[0] = squares[7] = Piece.White | Piece.Rook;
        squares[1] = squares[6] = Piece.White | Piece.Knight;
        squares[2] = squares[5] = Piece.White | Piece.Bishop;
        squares[3] = Piece.White | Piece.Queen;
        squares[4] = Piece.White | Piece.King;
        for (int i = 8; i < 16; i++) { squares[i] = Piece.White | Piece.Pawn; }
        squares[56] = squares[63] = Piece.Black | Piece.Rook;
        squares[57] = squares[62] = Piece.Black | Piece.Knight;
        squares[58] = squares[61] = Piece.Black | Piece.Bishop;
        squares[59] = Piece.Black | Piece.Queen;
        squares[60] = Piece.Black | Piece.King;
        for (int i = 48; i < 56; i++) { squares[i] = Piece.Black | Piece.Pawn; }

        boardFenToFrequency = new Dictionary<string, int>();
        boardFenToFrequency[HalfFen()] = 1;
    }

    public static int PieceAt(int square)
    {
        return squaresStack.Peek()[square];
    }

    public static bool Has3MoveRepetitioned()
    {
        return boardFenToFrequency.Values.Max() >= 3;
    }

    public static void UnRecordMove()
    {
        string fen = HalfFen();
        if (boardFenToFrequency[fen] == 1)
        {
            boardFenToFrequency.Remove(fen);
        }
        else
        {
            boardFenToFrequency[fen] -= 1;
        }

        squaresStack.Pop();
    }

    // Board.RecordMove() must happen after GameState.RecordMove()
    public static void RecordMove(Move move)
    {
        int[] squares = (int[])squaresStack.Peek().Clone();
        squaresStack.Push(squares);

        int friendlyColor = Piece.OppositeColor(GameState.ColorToMove);
        squares[move.TargetSquare] = PieceAt(move.StartSquare);
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
                switch (move.TargetSquare)
                {
                    case 6:  // White king side castle
                        squares[5] = PieceAt(7);
                        squares[7] = Piece.None;
                        break;
                    case 2:  // White queen side castle
                        squares[3] = PieceAt(0);
                        squares[0] = Piece.None;
                        break;
                    case 62:  // Black king side castle
                        squares[61] = PieceAt(63);
                        squares[63] = Piece.None;
                        break;
                    case 58:  // Black queen side castle
                        squares[59] = PieceAt(56);
                        squares[56] = Piece.None;
                        break;
                }
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

        string fen = HalfFen();
        if (boardFenToFrequency.ContainsKey(fen))
        {
            boardFenToFrequency[fen] += 1;
        }
        else
        {
            boardFenToFrequency[fen] = 1;
        }
    }


    public static string HalfFen()
    {
        Dictionary<int, char> pieceTypeToSymbol = new Dictionary<int, char>{
            {Piece.Pawn, 'p'},
            {Piece.Rook, 'r'},
            {Piece.Knight, 'n'},
            {Piece.Bishop, 'b'},
            {Piece.Queen, 'q'},
            {Piece.King, 'k'}
        };

        string fen = "";

        for (int rank = 7; rank >= 0; rank--)
        {
            int emptySquareStreak = 0;
            for (int file = 0; file < 8; file++)
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

                char baseSymbol = pieceTypeToSymbol[Piece.Type(piece)];
                char symbol = Piece.IsWhite(piece) ? char.ToUpper(baseSymbol) : baseSymbol;
                fen += symbol;
            }

            if (emptySquareStreak > 0)
            {
                fen += emptySquareStreak;
            }

            if (rank != 0)
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
}