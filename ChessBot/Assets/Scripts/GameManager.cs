using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
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

    private Game game;
    private GameObject[] pieceObjects;

    private void Start()
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

        game = new Game(Piece.White);

        pieceObjects = new GameObject[64];
        InstantiatePieceObjects();

        List<Move> legalMoves = LegalMoves.GetLegalMoves(game.Board, game.GameState);

        foreach (Move move in legalMoves)
        {
            Debug.Log(move);
        }

    }


    private void InstantiatePieceObjects()
    {
        for (int i = 0; i < 64; i += 1)
        {
            int piece = game.Board.PieceAt(i);

            if (piece == Piece.None) continue;

            GameObject pieceObject = pieceToGameObject[piece];

            pieceObjects[i] = Instantiate(pieceObject, Helpers.SquareToLocation(i), Quaternion.identity);
        }
    }

}
