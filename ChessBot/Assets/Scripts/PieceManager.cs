using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chess;

public class PieceManager : MonoBehaviour
{

    public GameObject pawnWhite;
    public GameObject rookWhite;
    public GameObject knightWhite;
    public GameObject bishopWhite;
    public GameObject queenWhite;
    public GameObject kingWhite;
    public GameObject pawnBlack;
    public GameObject rookBlack;
    public GameObject knightBlack;
    public GameObject bishopBlack;
    public GameObject queenBlack;
    public GameObject kingBlack;

    private Dictionary<int, GameObject> pieceToGameObject = new Dictionary<int, GameObject>();

    void Start()
    {
        pieceToGameObject[Piece.Pawn | Piece.White] = pawnWhite;
        pieceToGameObject[Piece.Rook | Piece.White] = rookWhite;
        pieceToGameObject[Piece.Knight | Piece.White] = knightWhite;
        pieceToGameObject[Piece.Bishop | Piece.White] = bishopWhite;
        pieceToGameObject[Piece.Queen | Piece.White] = queenWhite;
        pieceToGameObject[Piece.King | Piece.White] = kingWhite;
        pieceToGameObject[Piece.Pawn | Piece.Black] = pawnBlack;
        pieceToGameObject[Piece.Rook | Piece.Black] = rookBlack;
        pieceToGameObject[Piece.Knight | Piece.Black] = knightBlack;
        pieceToGameObject[Piece.Bishop | Piece.Black] = bishopBlack;
        pieceToGameObject[Piece.Queen | Piece.Black] = queenBlack;
        pieceToGameObject[Piece.King | Piece.Black] = kingBlack;
        InstantiatePieces();
    }


    void InstantiatePieces()
    {

        Board.LoadFromFEN("2kr2nr/Qpq3bp/1B2p1p1/3pP3/2p5/2P2N2/PP3PPP/R3R1K1 b - - 0 17");

        int[] squares = Board.GetSquares();
        for (int i = 0; i < 64; i += 1)
        {
            int piece = squares[i];
            if (piece == 0) continue;

            Vector2 location = new Vector2(i % 8, i / 8);
            Vector2 centeringOffset = new Vector2(0.5f, 0.5f);

            Instantiate(pieceToGameObject[piece], location + centeringOffset, Quaternion.identity);
        }
    }

}
