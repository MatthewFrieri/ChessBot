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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Move move in Board.tempLegalMoves)
        {
            if (Piece.Type(Board.Squares[move.StartSquare]) == Piece.Pawn)
            {
                Vector2 location = new Vector2(Board.File(move.TargetSquare) + 0.5f, Board.Rank(move.TargetSquare) + 0.5f);
                Gizmos.DrawSphere(location, 0.3f);

            }
        }
    }


    void InstantiatePieces()
    {

        Board.LoadFromFEN("8/8/8/r2Pp2K/8/8/8/8 w - - 0 1");

        int[] squares = Board.Squares;
        for (int square = 0; square < 64; square += 1)
        {
            int piece = squares[square];
            if (piece == Piece.None) continue;

            Vector2 location = new Vector2(Board.File(square), Board.Rank(square));
            Vector2 centeringOffset = new Vector2(0.5f, 0.5f);

            Instantiate(pieceToGameObject[piece], location + centeringOffset, Quaternion.identity);
        }
    }


    public void ShowLegalMoves(List<Move> moves)
    {

    }


}
